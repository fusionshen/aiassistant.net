using AI_Assistant_Win.Models;
using AI_Assistant_Win.Models.Middle;
using AI_Assistant_Win.Utils;
using MvCameraControl;
using SQLite;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.Json;
using System.Threading;
using System.Windows.Forms;

namespace AI_Assistant_Win.Business
{
    public class CameraBLL
    {
        CameraBinding binding = null;
        IDevice device = null;

        readonly DeviceTLayerType enumTLayerType = DeviceTLayerType.MvGigEDevice |
            DeviceTLayerType.MvUsbDevice | DeviceTLayerType.MvGenTLGigEDevice | DeviceTLayerType.MvGenTLCXPDevice |
            DeviceTLayerType.MvGenTLCameraLinkDevice | DeviceTLayerType.MvGenTLXoFDevice;

        List<IDeviceInfo> deviceInfoList = [];

        private CameraGrabbing cameraGrabbing;

        Thread receiveThread = null;    // ch:接收图像线程 | en: Receive image thread
        private bool IsOpen { get; set; } = false;
        public bool IsGrabbing { get; set; } = false;   // ch:是否正在取图 | en: Grabbing flag
        public bool IsRecording { get; set; } = false;       // ch:是否正在录像 | en: Video record flag
        public bool IsTriggerMode { get; set; } = false;

        private IFrameOut frameForSave;                         // ch:获取到的帧信息, 用于保存图像 | en:Frame for save image

        private readonly object saveImageLock = new object();

        private readonly SQLiteConnection connection;
        public CameraBLL()
        {
            SDKSystem.Initialize();
            connection = SQLiteHandler.Instance.GetSQLiteConnection();
        }

        public string StartGrabbing(CameraGrabbing _cameraGrabbing)
        {
            cameraGrabbing = _cameraGrabbing;
            // 查询数据库是否存在摄像头绑定
            binding = connection.Table<CameraBinding>().FirstOrDefault(t => t.Application.Equals(cameraGrabbing.Application));
            if (binding == null)
            {
                return "NoCameraSettings";
            }
            if (!binding.IsOpen)
            {
                return "NoCameraOpen";
            }
            // get device list
            GetDeviceList();
            // open camera
            var deviceIndex = deviceInfoList.FindIndex(t => t.SerialNumber.Equals(binding.SerialNumber));
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
                    return "TriggerMode";
                }
                // set continuous  mode
                // ch:设置采集连续模式 | en:Set Continues Aquisition Mode
                device.Parameters.SetEnumValueByString("AcquisitionMode", "Continuous");
                device.Parameters.SetEnumValueByString("TriggerMode", "Off");
                return "ContinuousMode";
            }
            else
            {
                return "NoCameraGrabbing";
            }
        }

        public CameraGrabbing GetCameraGrabbing()
        {
            return cameraGrabbing;
        }

        public List<IDeviceInfo> GetDeviceList()
        {
            if (deviceInfoList == null || deviceInfoList.Count == 0)
            {
                int nRet = DeviceEnumerator.EnumDevices(enumTLayerType, out deviceInfoList);
                if (nRet != MvError.MV_OK)
                {
                    throw new CameraSDKException("Enumerate devices fail!", nRet);
                }
            }
            return deviceInfoList;
        }

        public void OpenDevice(int selectedIndex)
        {
            // ch:获取选择的设备信息 | en:Get selected device information
            IDeviceInfo deviceInfo = deviceInfoList[selectedIndex];
            try
            {
                // ch:打开设备 | en:Open device
                device = DeviceFactory.CreateDevice(deviceInfo);
            }
            catch (Exception ex)
            {
                throw new CameraSDKException("Create Device fail!" + ex.Message);
            }

            int result = device.Open();
            if (result != MvError.MV_OK)
            {
                throw new CameraSDKException("Open Device fail!", result);
            }

            //ch: 判断是否为gige设备 | en: Determine whether it is a GigE device
            if (device is IGigEDevice)
            {
                //ch: 转换为gigE设备 | en: Convert to Gige device
                IGigEDevice gigEDevice = device as IGigEDevice;

                // ch:探测网络最佳包大小(只对GigE相机有效) | en:Detection network optimal package size(It only works for the GigE camera)
                int optionPacketSize;
                result = gigEDevice.GetOptimalPacketSize(out optionPacketSize);
                if (result != MvError.MV_OK)
                {
                    throw new CameraSDKException("Warning: Get Packet Size failed!", result);
                }
                else
                {
                    result = device.Parameters.SetIntValue("GevSCPSPacketSize", (long)optionPacketSize);
                    if (result != MvError.MV_OK)
                    {
                        throw new CameraSDKException("Warning: Set Packet Size failed!", result);
                    }
                }
            }

            // ch:设置采集连续模式 | en:Set Continues Aquisition Mode
            device.Parameters.SetEnumValueByString("AcquisitionMode", "Continuous");
            device.Parameters.SetEnumValueByString("TriggerMode", "Off");
            IsOpen = true;
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
                MessageBox.Show("Start thread failed!, " + ex.Message);
                throw;
            }

            // ch:开始采集 | en:Start Grabbing
            int result = device.StreamGrabber.StartGrabbing();
            if (result != MvError.MV_OK)
            {
                IsGrabbing = false;
                receiveThread.Join();
                throw new CameraSDKException("Start Grabbing Fail!", result);
            }

        }

        public void ReceiveThreadProcess()
        {
            int nRet;

            Graphics graphics;   // ch:使用GDI在pictureBox上绘制图像 | en:Display frame using a graphics

            while (IsGrabbing)
            {
                IFrameOut frameOut;

                nRet = device.StreamGrabber.GetImageBuffer(1000, out frameOut);
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
                            MessageBox.Show("IFrameOut.Clone failed, " + e.Message);
                            return;
                        }
                    }

#if !GDI_RENDER
                    device.ImageRender.DisplayOneFrame(cameraGrabbing.ImageHandle, frameOut.Image);
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
            // ch:标志位设为false | en:Set flag bit false
            IsGrabbing = false;
            receiveThread.Join();

            // ch:停止采集 | en:Stop Grabbing
            int result = device.StreamGrabber.StopGrabbing();
            if (result != MvError.MV_OK)
            {
                throw new CameraSDKException("Stop Grabbing Fail!", result);
            }

        }

        public void CloseDevice()
        {
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
            if (binding == null)
            {
                binding = new CameraBinding();
                binding.CreateTime = DateTime.Now;
            }
            #region 构造实体
            binding.Application = cameraGrabbing.Application;
            binding.SerialNumber = device.DeviceInfo.SerialNumber;
            binding.DeviceInfo = JsonSerializer.Serialize(device.DeviceInfo);
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
    }
}
