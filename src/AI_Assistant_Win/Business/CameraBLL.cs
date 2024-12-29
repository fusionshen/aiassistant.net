using AI_Assistant_Win.Models;
using AI_Assistant_Win.Models.Enums;
using AI_Assistant_Win.Utils;
using MvCameraControl;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace AI_Assistant_Win.Business
{
    public class CameraBLL(string application, nint imageHandle) : INotifyPropertyChanged
    {
        private readonly string _application = application;
        private readonly nint _imageHandle = imageHandle;
        CameraBinding binding = null;
        IDevice device = null;

        readonly DeviceTLayerType enumTLayerType = DeviceTLayerType.MvGigEDevice |
            DeviceTLayerType.MvUsbDevice | DeviceTLayerType.MvGenTLGigEDevice | DeviceTLayerType.MvGenTLCXPDevice |
            DeviceTLayerType.MvGenTLCameraLinkDevice | DeviceTLayerType.MvGenTLXoFDevice;

        List<IDeviceInfo> deviceInfoList = [];

        Thread receiveThread = null;    // ch:接收图像线程 | en: Receive image thread
        Thread reconnectThread = null;
        private bool IsOpen { get; set; } = false;
        // ch:是否正在取图 | en: Grabbing flag
        private bool isGrabbing = false;
        public bool IsGrabbing
        {
            get { return isGrabbing; }
            set
            {
                if (isGrabbing != value)
                {
                    isGrabbing = value;
                    OnPropertyChanged(nameof(IsGrabbing));
                }
            }
        }
        public bool IsRecording { get; set; } = false;       // ch:是否正在录像 | en: Video record flag
        public bool IsTriggerMode { get; set; } = false;

        private IFrameOut frameForSave;                         // ch:获取到的帧信息, 用于保存图像 | en:Frame for save image

        private readonly object saveImageLock = new();

        private readonly SQLiteConnection connection = SQLiteHandler.Instance.GetSQLiteConnection();

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CameraBLLStatusKind StartRendering()
        {
            // get device list
            GetDeviceList();
            if (deviceInfoList == null || deviceInfoList.Count == 0)
            {
                return CameraBLLStatusKind.NoCamera;
            }
            // 查询数据库是否存在摄像头绑定
            binding = connection.Table<CameraBinding>().FirstOrDefault(t => t.Application.Equals(_application));
            if (binding == null)
            {
                return CameraBLLStatusKind.NoCameraSettings;
            }
            if (!binding.IsOpen)
            {
                return CameraBLLStatusKind.NoCameraOpen;
            }
            var deviceIndex = deviceInfoList.FindIndex(t => t.SerialNumber.Equals(binding.SerialNumber));
            // open camera TODO: maybe there are exceptions
            OpenDevice(deviceIndex);
            if (binding.IsGrabbing)
            {
                // start grabbing
                StartGrabbing();
                if (binding.IsTriggerMode)
                {
                    // set software trigger
                    IsTriggerMode = true;
                    device.Parameters.SetEnumValueByString("TriggerMode", "On");
                    // ch:触发源设为软触发 | en:Set trigger source as Software
                    device.Parameters.SetEnumValueByString("TriggerSource", "Software");
                    return CameraBLLStatusKind.TriggerMode;
                }
                // set continuous  mode
                // ch:设置采集连续模式 | en:Set Continues Aquisition Mode
                device.Parameters.SetEnumValueByString("AcquisitionMode", "Continuous");
                device.Parameters.SetEnumValueByString("TriggerMode", "Off");
                return CameraBLLStatusKind.ContinuousMode;
            }
            else
            {
                return CameraBLLStatusKind.NoCameraGrabbing;
            }
        }

        public List<IDeviceInfo> GetDeviceList()
        {
            if (deviceInfoList == null || deviceInfoList.Count == 0)
            {
                int nRet = DeviceEnumerator.EnumDevices(enumTLayerType, out deviceInfoList);
                if (nRet != MvError.MV_OK)
                {
                    throw new CameraSDKException(LocalizeHelper.ENUMERATE_DEVICES_FAILED, nRet);
                }
            }
            return deviceInfoList;
        }

        private bool isExited = false;

        private bool isConnected = false;

        public bool IsConnected
        {
            get { return isConnected; }
            set
            {
                if (isConnected != value)
                {
                    isConnected = value;
                    OnPropertyChanged(nameof(IsConnected));
                }
            }
        }

        private int cameraIndex;

        public void OpenDevice(int selectedIndex)
        {
            cameraIndex = selectedIndex;
            OpenCameraReally();
            reconnectThread = new Thread(ReconnectProcess);
            reconnectThread.Start();
        }

        private void ReconnectProcess()
        {
            while (true)
            {
                if (isConnected)
                {
                    Thread.Sleep(1);
                    continue;
                }

                if (isExited)
                {
                    break;
                }

                if (device != null)
                {
                    device.StreamGrabber.StopGrabbing();
                    device.Close();
                    device.Dispose();
                    device = null;
                }
                // open camera continuously
                try
                {
                    OpenCameraReally();
                }
                catch (Exception)
                {
                    continue;
                }
                if (IsGrabbing)
                {
                    // start grabbing
                    StartGrabbing();
                    if (IsTriggerMode)
                    {
                        // set software trigger
                        device.Parameters.SetEnumValueByString("TriggerMode", "On");
                        // ch:触发源设为软触发 | en:Set trigger source as Software
                        device.Parameters.SetEnumValueByString("TriggerSource", "Software");
                    }
                    else
                    {
                        // set continuous  mode
                        // ch:设置采集连续模式 | en:Set Continues Aquisition Mode
                        device.Parameters.SetEnumValueByString("AcquisitionMode", "Continuous");
                        device.Parameters.SetEnumValueByString("TriggerMode", "Off");
                    }
                }
            }
        }

        private void OpenCameraReally()
        {
            try
            {
                // ch:获取选择的设备信息 | en:Get selected device information
                IDeviceInfo deviceInfo = deviceInfoList[cameraIndex];
                // ch:打开设备 | en:Open device
                device = DeviceFactory.CreateDevice(deviceInfo);
            }
            catch (Exception ex)
            {
                throw new CameraSDKException(LocalizeHelper.CREATE_DEVICE_FAILED + ex.Message);
            }
            int result = device.Open();
            if (result != MvError.MV_OK)
            {
                throw new CameraSDKException(LocalizeHelper.OPEN_DEVICE_FAILED, result);
            }

            //ch: 判断是否为gige设备 | en: Determine whether it is a GigE device
            if (device is IGigEDevice)
            {
                //ch: 转换为gigE设备 | en: Convert to Gige device
                IGigEDevice gigEDevice = device as IGigEDevice;

                // ch:探测网络最佳包大小(只对GigE相机有效) | en:Detection network optimal package size(It only works for the GigE camera)
                result = gigEDevice.GetOptimalPacketSize(out int optionPacketSize);
                if (result != MvError.MV_OK)
                {
                    throw new CameraSDKException(LocalizeHelper.GET_PACKET_SIZE_FAILED, result);
                }
                else
                {
                    result = device.Parameters.SetIntValue("GevSCPSPacketSize", (long)optionPacketSize);
                    if (result != MvError.MV_OK)
                    {
                        throw new CameraSDKException(LocalizeHelper.SET_PACKET_SIZE_FAILED, result);
                    }
                }
            }

            // ch:设置采集连续模式 | en:Set Continues Aquisition Mode
            device.Parameters.SetEnumValueByString("AcquisitionMode", "Continuous");
            device.Parameters.SetEnumValueByString("TriggerMode", "Off");
            IsOpen = true;
            IsConnected = true;
            device.DeviceExceptionEvent += ExceptionEventHandler;
        }

        void ExceptionEventHandler(object sender, DeviceExceptionArgs e)
        {
            // will call after 60 secs 
            if (e.MsgType == DeviceExceptionType.DisConnect)
            {
                IsConnected = false;
            }
        }

        public IDevice GetDevice()
        {
            return device;
        }

        public void StartGrabbing()
        {
            try
            {
                // ch:标志位置位true | en:Set position bit true
                IsGrabbing = true;
                receiveThread = new Thread(ReceiveThreadProcess);
                receiveThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(LocalizeHelper.START_THREAD_FAILED + ex.Message);
                throw;
            }

            // ch:开始采集 | en:Start Grabbing
            int result = device.StreamGrabber.StartGrabbing();
            if (result != MvError.MV_OK)
            {
                IsGrabbing = false;
                receiveThread.Join();
                throw new CameraSDKException(LocalizeHelper.START_GRABBING_FAILED, result);
            }

        }

        public void ReceiveThreadProcess()
        {
            int nRet;

            Graphics graphics;   // ch:使用GDI在pictureBox上绘制图像 | en:Display frame using a graphics

            while (IsGrabbing)
            {
                if (!isConnected)
                {
                    break;
                }
                nRet = device.StreamGrabber.GetImageBuffer(1000, out IFrameOut frameOut);
                if (MvError.MV_OK == nRet)
                {
                    if (IsRecording)
                    {
                        device.VideoRecorder.InputOneFrame(frameOut.Image);
                    }

                    lock (saveImageLock)
                    {
                        try
                        {
                            frameForSave = frameOut.Clone() as IFrameOut;
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(LocalizeHelper.IFRAMEOUT_CLONE_FAILED + e.Message);
                            return;
                        }
                    }

#if !GDI_RENDER
                    device.ImageRender.DisplayOneFrame(_imageHandle, frameOut.Image);
#else
                    // 使用GDI绘制图像
                    try
                    {
                        using (Bitmap bitmap = frameOut.Image.ToBitmap())
                        {
                            if (graphics == null)
                            {
                                graphics = pictureBox1.CreateGraphics();
                            }

                            Rectangle srcRect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                            Rectangle dstRect = new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height);
                            graphics.DrawImage(bitmap, dstRect, srcRect, GraphicsUnit.Pixel);
                        }
                    }
                    catch (Exception e)
                    {
                        device.StreamGrabber.FreeImageBuffer(frameOut);
                        MessageBox.Show(e.Message);
                        return;
                    }
#endif

                    device.StreamGrabber.FreeImageBuffer(frameOut);
                }
                else
                {
                    if (IsTriggerMode)
                    {
                        Thread.Sleep(5);
                    }
                }
            }
        }

        public void StopGrabbing()
        {
            if (receiveThread == null ||    // if no camera and try to upload image
                !receiveThread.IsAlive)     // if images uploaded continuously after stopping camera grabbing. 
            {
                return;
            }
            // ch:标志位设为false | en:Set flag bit false
            IsGrabbing = false;
            receiveThread.Join();

            // ch:停止采集 | en:Stop Grabbing
            int result = device.StreamGrabber.StopGrabbing();
            if (result != MvError.MV_OK)
            {
                throw new CameraSDKException(LocalizeHelper.STOP_GRABBING_FAILED, result);
            }

        }

        public void CloseDevice()
        {
            isConnected = false;
            isExited = true;
            reconnectThread?.Join();
            if (device != null)
            {
                device.Close();
                device.Dispose();
                IsOpen = false;
            }
        }

        public CameraBinding GetConfig()
        {
            return binding;
        }

        public int SaveConfig()
        {
            if (device == null)
            {
                return -1;
            }
            binding ??= new CameraBinding
            {
                CreateTime = DateTime.Now
            };
            #region 构造实体
            binding.Application = _application;
            binding.SerialNumber = device.DeviceInfo.SerialNumber;
            binding.DeviceInfo = JsonConvert.SerializeObject(device.DeviceInfo);
            binding.IsOpen = IsOpen;
            binding.IsGrabbing = IsGrabbing;
            int result = device.Parameters.GetEnumValue("TriggerMode", out IEnumValue enumValue);
            if (result == MvError.MV_OK)
            {
                if (enumValue.CurEnumEntry.Symbolic == "On")
                {
                    binding.IsTriggerMode = true;
                }
                else
                {
                    binding.IsTriggerMode = false;
                }
            }
            result = device.Parameters.GetFloatValue("ExposureTime", out IFloatValue floatValue);
            if (result == MvError.MV_OK)
            {
                binding.ExposureTime = floatValue.CurValue;
            }
            result = device.Parameters.GetFloatValue("Gain", out floatValue);
            if (result == MvError.MV_OK)
            {
                binding.Gain = floatValue.CurValue;
            }
            result = device.Parameters.GetFloatValue("ResultingFrameRate", out floatValue);
            if (result == MvError.MV_OK)
            {
                binding.ResultingFrameRate = floatValue.CurValue;
            }
            result = device.Parameters.GetEnumValue("PixelFormat", out enumValue);
            if (result == MvError.MV_OK)
            {
                binding.PixelFormat = enumValue.CurEnumEntry.Symbolic;
            }
            binding.LastModifiedTime = DateTime.Now;
            #endregion
            var ok = binding.Id == 0 ? connection.Insert(binding) : connection.Update(binding);
            return ok;
        }

        public string SaveImage()
        {
            if (frameForSave == null)
            {
                throw new Exception(LocalizeHelper.NO_VAILD_IMAGE);
            }
            ImageFormatInfo imageFormatInfo;
            imageFormatInfo.FormatType = ImageFormatType.Jpeg;
            imageFormatInfo.JpegQuality = 99;

            string directoryPath = $".\\Images\\{_application}\\Origin";
            Directory.CreateDirectory(directoryPath);
            var imageName = $"{DateTime.Now:yyyyMMddHHmmssfff}_w{frameForSave.Image.Width}_h{frameForSave.Image.Height}_fn{frameForSave.FrameNum}.{imageFormatInfo.FormatType}";
            string fullPath = Path.Combine(directoryPath, imageName);
            lock (saveImageLock)
            {
                var result = device.ImageSaver.SaveImageToFile(fullPath, frameForSave.Image, imageFormatInfo, CFAMethod.Equilibrated);
                if (result != MvError.MV_OK)
                {
                    throw new CameraSDKException(LocalizeHelper.SAVE_IAMGE_FAILED, result);
                }
            }
            return fullPath;
        }
    }
}
