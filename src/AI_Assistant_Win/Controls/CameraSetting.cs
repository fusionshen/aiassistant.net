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
                // ch:�ؼ����� | en:Control operation
                SetCtrlWhenOpen();
                // ch:��ȡ���� | en:Get parameters
                BnGetParam_Click(null, null);
                // ����������ã������ڱ����ϴ�ͼƬ����������Ƭ���²ɼ�ֹͣ�ˣ���ʱҳ�������ʵ����в���
                if (cameraBLL.IsGrabbing)
                {
                    // ch:�ؼ����� | en:Control Operation
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
                    // ch:�ؼ����� | en:Control Operation
                    SetCtrlWhenStopGrab();
                }
            }
            else
            {
                // ch:�ؼ����� | en:Control Operation
                SetCtrlWhenClose();
            }
        }

        private void RefreshDeviceList(int selectedIndex = 0)
        {
            try
            {
                var deviceInfoList = cameraBLL.GetDeviceList();
                // ch:�����豸�б� | en:Create Device List
                cbDeviceList.Items.Clear();
                // ch:�ڴ����б�����ʾ�豸�� | en:Display device name in the form list
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
                // ch:ѡ���һ�� | en:Select the first item
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

            // ch:�ؼ����� | en:Control operation
            SetCtrlWhenOpen();

            // ch:��ȡ���� | en:Get parameters
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
        /// ch:��ȡ����ģʽ | en:Get Trigger Mode
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
            // ch:ȡ����־λ���� | en:Reset flow flag bit
            if (cameraBLL.IsGrabbing)
            {
                BnStopGrab_Click(sender, e);
            }
            // ch:�ر��豸 | en:Close Device
            cameraBLL.CloseDevice();
            // ch:�ؼ����� | en:Control Operation
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
                // ch:�ؼ����� | en:Control Operation
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
                // ch:�ؼ����� | en:Control Operation
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
            // ch:�򿪴���ģʽ | en:Open Trigger Mode
            if (bnTriggerMode.Checked)
            {
                cameraBLL.IsTriggerMode = true;
                device.Parameters.SetEnumValueByString("TriggerMode", "On");
                // ch:����Դѡ��:0 - Line0; | en:Trigger source select:0 - Line0;
                //           0 - Line0;      6-pin P7�ܽŶ���   ��   OPTO_IN   Line 0+  ����������� ��˼�ǿɽ�������豸��
                //           1 - Line1;                        ��   OPTO_OUT  Line 1+  ���������� 
                //           2 - Line2;                        ��   GPIO      Line 2+  �������������� 
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
                // ch:����Դ��Ϊ���� | en:Set trigger source as Software
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
            // ch:�������� | en:Trigger command
            int result = device.Parameters.SetCommandValue("TriggerSoftware");
            if (result != MvError.MV_OK)
            {
                CameraHelper.ShowErrorMsg(form, "Trigger Software Fail!", result);
            }
            AntdUI.Notification.info(form, "��ʾ", "��������������Ч���鿴���������Զ��ϴ���ʶ��", AntdUI.TAlignFrom.BR, Font);
        }

        /// <summary>
        /// ���ʹ�ø��ְ�ť�������棬�����������̫��
        /// </summary>
        /// <returns></returns>
        private async Task SaveConfigAsync()
        {
            await Task.Delay(50);
            var result = cameraBLL.SaveConfig();
            if (result == 0)
            {
                AntdUI.Notification.error(form, "ʧ��", "����ͷ���ñ���ʧ�ܣ�", AntdUI.TAlignFrom.BR, Font);
            }
        }
    }
}