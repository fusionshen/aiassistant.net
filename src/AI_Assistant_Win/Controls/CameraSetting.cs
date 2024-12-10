using AI_Assistant_Win.Business;
using AI_Assistant_Win.Models.Middle;
using MvCameraControl;
using System;
using System.Windows.Forms;

namespace AI_Assistant_Win.Controls
{
    public partial class CameraSetting : UserControl
    {
        private Form form;
        private CameraBLL cameraBLL = null;
        public CameraSetting(Form _form, CameraBLL _cameraBLL)
        {
            form = _form;
            cameraBLL = _cameraBLL;
            InitializeComponent();
            RefreshDeviceList();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void RefreshDeviceList()
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
                    cbDeviceList.SelectedIndex = 0;
                }
            }
            catch (CameraSDKException error)
            {
                ShowErrorMsg(error.Message, error.ErrorCode);
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
                ShowErrorMsg("No device, please select", 0);
                return;
            }
            cameraBLL.OpenDevice(cbDeviceList.SelectedIndex);

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
            cbSoftTrigger.Enabled = false;
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
            cbSoftTrigger.Enabled = false;
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
            IFloatValue floatValue;
            int result = device.Parameters.GetFloatValue("ExposureTime", out floatValue);
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
            IEnumValue enumValue;
            result = device.Parameters.GetEnumValue("PixelFormat", out enumValue);
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
            IEnumValue enumValue;
            var device = cameraBLL.GetDevice();
            int result = device.Parameters.GetEnumValue("TriggerMode", out enumValue);
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
                            cbSoftTrigger.Enabled = true;
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

        /// <summary>
        /// TODO: 关闭应用页面时需要关闭设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            if (cameraBLL.IsRecording)
            {
                //bnStopRecord_Click(sender, e);
            }
            cameraBLL.StopGrabbing();
            // ch:控件操作 | en:Control Operation
            SetCtrlWhenStopGrab();
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
            }
            catch (CameraSDKException error)
            {
                ShowErrorMsg(error.Message, error.ErrorCode);
            }
            // ch:控件操作 | en:Control Operation
            SetCtrlWhenStartGrab();
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
                //           1 - Line1;
                //           2 - Line2;
                //           3 - Line3;
                //           4 - Counter;
                //           7 - Software;
                if (cbSoftTrigger.Checked)
                {
                    device.Parameters.SetEnumValueByString("TriggerSource", "Software");
                    if (cameraBLL.IsGrabbing)
                    {
                        bnTriggerExec.Enabled = true;
                    }
                }
                else
                {
                    device.Parameters.SetEnumValueByString("TriggerSource", "Line0");
                }
                cbSoftTrigger.Enabled = true;
            }
        }


        private void BnContinuesMode_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            var device = cameraBLL.GetDevice();
            if (bnContinuesMode.Checked)
            {
                device.Parameters.SetEnumValueByString("TriggerMode", "Off");
                cbSoftTrigger.Enabled = false;
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
            else
            {
                device.Parameters.SetEnumValueByString("TriggerSource", "Line0");
                bnTriggerExec.Enabled = false;
            }
        }

        private void BnTriggerExec_Click(object sender, EventArgs e)
        {
            var device = cameraBLL.GetDevice();
            // ch:触发命令 | en:Trigger command
            int result = device.Parameters.SetCommandValue("TriggerSoftware");
            if (result != MvError.MV_OK)
            {
                ShowErrorMsg("Trigger Software Fail!", result);
            }
        }

        // ch:显示错误信息 | en:Show error message
        private void ShowErrorMsg(string message, int errorCode)
        {
            string errorMsg;
            if (errorCode == 0)
            {
                errorMsg = message;
            }
            else
            {
                errorMsg = message + ": Error =" + String.Format("{0:X}", errorCode);
            }

            switch (errorCode)
            {
                case MvError.MV_E_HANDLE: errorMsg += " Error or invalid handle "; break;
                case MvError.MV_E_SUPPORT: errorMsg += " Not supported function "; break;
                case MvError.MV_E_BUFOVER: errorMsg += " Cache is full "; break;
                case MvError.MV_E_CALLORDER: errorMsg += " Function calling order error "; break;
                case MvError.MV_E_PARAMETER: errorMsg += " Incorrect parameter "; break;
                case MvError.MV_E_RESOURCE: errorMsg += " Applying resource failed "; break;
                case MvError.MV_E_NODATA: errorMsg += " No data "; break;
                case MvError.MV_E_PRECONDITION: errorMsg += " Precondition error, or running environment changed "; break;
                case MvError.MV_E_VERSION: errorMsg += " Version mismatches "; break;
                case MvError.MV_E_NOENOUGH_BUF: errorMsg += " Insufficient memory "; break;
                case MvError.MV_E_UNKNOW: errorMsg += " Unknown error "; break;
                case MvError.MV_E_GC_GENERIC: errorMsg += " General error "; break;
                case MvError.MV_E_GC_ACCESS: errorMsg += " Node accessing condition error "; break;
                case MvError.MV_E_ACCESS_DENIED: errorMsg += " No permission "; break;
                case MvError.MV_E_BUSY: errorMsg += " Device is busy, or network disconnected "; break;
                case MvError.MV_E_NETER: errorMsg += " Network error "; break;
            }

            AntdUI.Notification.error(form, "失败", errorMsg, AntdUI.TAlignFrom.BR, Font);
        }
    }
}