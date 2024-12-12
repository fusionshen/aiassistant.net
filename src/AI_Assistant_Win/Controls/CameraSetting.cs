using AI_Assistant_Win.Business;
using AI_Assistant_Win.Utils;
using MvCameraControl;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AI_Assistant_Win.Controls
{
    public partial class CameraSetting : UserControl
    {
        private readonly Form form;
        private readonly CameraBLL cameraBLL = null;
        public CameraSetting(Form _form, CameraBLL _cameraBLL)
        {
            form = _form;
            cameraBLL = _cameraBLL;
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            LoadConfig();
            this.HandleDestroyed += async (s, e) => await SaveConfigAsync();
        }

        private void LoadConfig()
        {
            var config = cameraBLL.GetConfig();
            if (config == null)
            {
                RefreshDeviceList();
                return;
            }
            RefreshDeviceList(cameraBLL.GetDeviceList().FindIndex(t => t.SerialNumber.Equals(config.SerialNumber)));
            if (config.IsOpen)
            {
                // ch:控件操作 | en:Control operation
                SetCtrlWhenOpen();
                // ch:获取参数 | en:Get parameters
                BnGetParam_Click(null, null);
                // 如果存在配置，但是在本地上次图片或者拍摄照片后导致采集停止了，此时页面会与真实情况有差异
                if (cameraBLL.IsGrabbing)
                {
                    // ch:控件操作 | en:Control Operation
                    SetCtrlWhenStartGrab();
                    if (config.IsTriggerMode)
                    {
                        cbSoftTrigger.Checked = true;
                        bnTriggerExec.Enabled = true;
                    }
                    else
                    {
                        cbSoftTrigger.Checked = false;
                        bnTriggerExec.Enabled = false;
                    }
                }
                else
                {
                    // ch:控件操作 | en:Control Operation
                    SetCtrlWhenStopGrab();
                }
            }
            else
            {
                // ch:控件操作 | en:Control Operation
                SetCtrlWhenClose();
            }
        }

        private void RefreshDeviceList(int selectedIndex = 0)
        {
            try
            {
                var deviceInfoList = cameraBLL.GetDeviceList();
                // ch:创建设备列表 | en:Create Device List
                cbDeviceList.Items.Clear();
                // ch:在窗体列表中显示设备名 | en:Display device name in the form list
                for (int i = 0; i < deviceInfoList.Count; i++)
                {
                    IDeviceInfo deviceInfo = deviceInfoList[i];
                    if (deviceInfo.UserDefinedName != "")
                    {
                        //cbDeviceList.Items.Add(deviceInfo.TLayerType.ToString() + ": " + deviceInfo.UserDefinedName + " (" + deviceInfo.SerialNumber + ")");
                        cbDeviceList.Items.Add(deviceInfo.UserDefinedName + " (" + deviceInfo.SerialNumber + ")");
                    }
                    else
                    {
                        cbDeviceList.Items.Add(deviceInfo.TLayerType.ToString() + ": " + deviceInfo.ManufacturerName + " " + deviceInfo.ModelName + " (" + deviceInfo.SerialNumber + ")");
                    }
                }
                // ch:选择第一项 | en:Select the first item
                if (deviceInfoList.Count != 0)
                {
                    cbDeviceList.SelectedIndex = selectedIndex;
                }
            }
            catch (CameraSDKException error)
            {
                CameraHelper.ShowErrorMsg(form, error.Message, error.ErrorCode);
            }
        }

        private void BnEnum_Click(object sender, EventArgs e)
        {
            RefreshDeviceList();
        }

        private void BnOpen_Click(object sender, EventArgs e)
        {
            if (cameraBLL.GetDeviceList().Count == 0 || cbDeviceList.SelectedIndex == -1)
            {
                CameraHelper.ShowErrorMsg(form, "No device, please select", 0);
                return;
            }
            try
            {
                cameraBLL.OpenDevice(cbDeviceList.SelectedIndex);
            }
            catch (CameraSDKException error)
            {
                CameraHelper.ShowErrorMsg(form, error.Message, error.ErrorCode);
                return;
            }

            // ch:控件操作 | en:Control operation
            SetCtrlWhenOpen();

            // ch:获取参数 | en:Get parameters
            BnGetParam_Click(null, null);
        }

        private void SetCtrlWhenOpen()
        {
            bnOpen.Enabled = false;
            bnClose.Enabled = true;
            bnStartGrab.Enabled = true;
            bnStopGrab.Enabled = false;
            bnContinuesMode.Enabled = true;
            bnContinuesMode.Checked = true;
            bnTriggerMode.Enabled = true;
            bnTriggerExec.Enabled = false;

            tbExposure.Enabled = true;
            tbGain.Enabled = true;
            tbFrameRate.Enabled = true;
            cbPixelFormat.Enabled = true;
            bnGetParam.Enabled = true;
            bnSetParam.Enabled = true;
        }

        private void SetCtrlWhenClose()
        {
            bnOpen.Enabled = true;
            bnClose.Enabled = false;
            bnStartGrab.Enabled = false;
            bnStopGrab.Enabled = false;
            bnContinuesMode.Enabled = false;
            bnTriggerMode.Enabled = false;
            bnTriggerExec.Enabled = false;

            //bnSaveBmp.Enabled = false;
            //bnSaveJpg.Enabled = false;
            //bnSaveTiff.Enabled = false;
            //bnSavePng.Enabled = false;
            tbExposure.Enabled = false;
            tbGain.Enabled = false;
            tbFrameRate.Enabled = false;
            bnGetParam.Enabled = false;
            bnSetParam.Enabled = false;
            cbPixelFormat.Enabled = false;
            //bnStartRecord.Enabled = false;
            //bnStopRecord.Enabled = false;
        }

        private void BnGetParam_Click(object sender, EventArgs e)
        {
            GetTriggerMode();
            var device = cameraBLL.GetDevice();
            int result = device.Parameters.GetFloatValue("ExposureTime", out IFloatValue floatValue);
            if (result == MvError.MV_OK)
            {
                tbExposure.Text = floatValue.CurValue.ToString("F1");
            }
            result = device.Parameters.GetFloatValue("Gain", out floatValue);
            if (result == MvError.MV_OK)
            {
                tbGain.Text = floatValue.CurValue.ToString("F1");
            }

            result = device.Parameters.GetFloatValue("ResultingFrameRate", out floatValue);
            if (result == MvError.MV_OK)
            {
                tbFrameRate.Text = floatValue.CurValue.ToString("F1");
            }

            cbPixelFormat.Items.Clear();
            result = device.Parameters.GetEnumValue("PixelFormat", out IEnumValue enumValue);
            if (result == MvError.MV_OK)
            {
                foreach (var item in enumValue.SupportEnumEntries)
                {
                    cbPixelFormat.Items.Add(item.Symbolic);
                    if (item.Symbolic == enumValue.CurEnumEntry.Symbolic)
                    {
                        cbPixelFormat.SelectedIndex = cbPixelFormat.Items.Count - 1;
                    }
                }
            }
        }

        /// <summary>
        /// ch:获取触发模式 | en:Get Trigger Mode
        /// </summary>
        private void GetTriggerMode()
        {
            var device = cameraBLL.GetDevice();
            int result = device.Parameters.GetEnumValue("TriggerMode", out IEnumValue enumValue);
            if (result == MvError.MV_OK)
            {
                if (enumValue.CurEnumEntry.Symbolic == "On")
                {
                    bnTriggerMode.Checked = true;
                    cameraBLL.IsTriggerMode = true;
                    bnContinuesMode.Checked = false;

                    result = device.Parameters.GetEnumValue("TriggerSource", out enumValue);
                    if (result == MvError.MV_OK)
                    {
                        if (enumValue.CurEnumEntry.Symbolic == "TriggerSoftware")
                        {
                            cbSoftTrigger.Checked = true;
                            if (cameraBLL.IsGrabbing)
                            {
                                bnTriggerExec.Enabled = true;
                            }
                        }
                    }
                }
                else
                {
                    bnContinuesMode.Checked = true;
                    bnTriggerMode.Checked = false;
                    cameraBLL.IsTriggerMode = false;
                }
            }
        }

        private void BnClose_Click(object sender, EventArgs e)
        {
            // ch:取流标志位清零 | en:Reset flow flag bit
            if (cameraBLL.IsGrabbing)
            {
                BnStopGrab_Click(sender, e);
            }
            // ch:关闭设备 | en:Close Device
            cameraBLL.CloseDevice();
            // ch:控件操作 | en:Control Operation
            SetCtrlWhenClose();
        }

        private void BnStopGrab_Click(object sender, EventArgs e)
        {
            try
            {
                if (cameraBLL.IsRecording)
                {
                    //bnStopRecord_Click(sender, e);
                }
                cameraBLL.StopGrabbing();
                // ch:控件操作 | en:Control Operation
                SetCtrlWhenStopGrab();
            }
            catch (CameraSDKException error)
            {
                CameraHelper.ShowErrorMsg(form, error.Message, error.ErrorCode);
            }
        }

        private void SetCtrlWhenStopGrab()
        {
            bnStartGrab.Enabled = true;
            cbPixelFormat.Enabled = true;
            bnStopGrab.Enabled = false;
            bnTriggerExec.Enabled = false;

            //bnSaveBmp.Enabled = false;
            //bnSaveJpg.Enabled = false;
            //bnSaveTiff.Enabled = false;
            //bnSavePng.Enabled = false;
            //bnStartRecord.Enabled = false;
            //bnStopRecord.Enabled = false;
        }

        private void BnStartGrab_Click(object sender, EventArgs e)
        {
            try
            {
                cameraBLL.StartGrabbing();
                // ch:控件操作 | en:Control Operation
                SetCtrlWhenStartGrab();
            }
            catch (CameraSDKException error)
            {
                CameraHelper.ShowErrorMsg(form, error.Message, error.ErrorCode);
            }
        }

        private void SetCtrlWhenStartGrab()
        {
            bnStartGrab.Enabled = false;
            cbPixelFormat.Enabled = false;
            bnStopGrab.Enabled = true;

            if (bnTriggerMode.Checked && cbSoftTrigger.Checked)
            {
                bnTriggerExec.Enabled = true;
            }

            //bnSaveBmp.Enabled = true;
            //bnSaveJpg.Enabled = true;
            //bnSaveTiff.Enabled = true;
            //bnSavePng.Enabled = true;
            //bnStartRecord.Enabled = true;
            //bnStopRecord.Enabled = false;
        }

        private void BnTriggerMode_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            var device = cameraBLL.GetDevice();
            // ch:打开触发模式 | en:Open Trigger Mode
            if (bnTriggerMode.Checked)
            {
                cameraBLL.IsTriggerMode = true;
                device.Parameters.SetEnumValueByString("TriggerMode", "On");
                // ch:触发源选择:0 - Line0; | en:Trigger source select:0 - Line0;
                //           0 - Line0;      6-pin P7管脚定义   黄   OPTO_IN   Line 0+  光耦隔离输入 意思是可接入快门设备？
                //           1 - Line1;                        蓝   OPTO_OUT  Line 1+  光耦隔离输出 
                //           2 - Line2;                        紫   GPIO      Line 2+  可配置输入或输出 
                //           3 - Line3;
                //           4 - Counter;
                //           7 - Software;
                cbSoftTrigger.Checked = true;
                device.Parameters.SetEnumValueByString("TriggerSource", "Software");
                if (cameraBLL.IsGrabbing)
                {
                    bnTriggerExec.Enabled = true;
                }
            }
        }


        private void BnContinuesMode_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            var device = cameraBLL.GetDevice();
            if (bnContinuesMode.Checked)
            {
                device.Parameters.SetEnumValueByString("TriggerMode", "Off");
                cbSoftTrigger.Checked = false;
                bnTriggerExec.Enabled = false;
            }
        }

        private void CbSoftTrigger_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            var device = cameraBLL.GetDevice();
            if (cbSoftTrigger.Checked)
            {
                // ch:触发源设为软触发 | en:Set trigger source as Software
                device.Parameters.SetEnumValueByString("TriggerSource", "Software");
                if (cameraBLL.IsGrabbing)
                {
                    bnTriggerExec.Enabled = true;
                }
            }
        }

        private void BnTriggerExec_Click(object sender, EventArgs e)
        {
            var device = cameraBLL.GetDevice();
            // ch:触发命令 | en:Trigger command
            int result = device.Parameters.SetCommandValue("TriggerSoftware");
            if (result != MvError.MV_OK)
            {
                CameraHelper.ShowErrorMsg(form, "Trigger Software Fail!", result);
            }
            AntdUI.Notification.info(form, "提示", "软触发仅用于拍照效果查看，并不会自动上传并识别！", AntdUI.TAlignFrom.BR, Font);
        }

        /// <summary>
        /// 如果使用各种按钮触发保存，那样保存次数太多
        /// </summary>
        /// <returns></returns>
        private async Task SaveConfigAsync()
        {
            await Task.Delay(50);
            var result = cameraBLL.SaveConfig();
            if (result == 0)
            {
                AntdUI.Notification.error(form, "失败", "摄像头配置保存失败！", AntdUI.TAlignFrom.BR, Font);
            }
        }
    }
}