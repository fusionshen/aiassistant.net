using AI_Assistant_Win.Business;
using AI_Assistant_Win.Models.Enums;
using AI_Assistant_Win.Models.Middle;
using AI_Assistant_Win.Models.Response;
using AI_Assistant_Win.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AI_Assistant_Win.Controls
{
    public partial class BlacknessMethod : UserControl
    {
        public static string EDIT_ITEM_ID = string.Empty;

        private readonly MainWindow form;

        private readonly BlacknessMethodBLL blacknessMethodBLL;

        private readonly ImageProcessBLL imageProcessBLL;

        private readonly CameraBLL cameraBLL;

        private readonly BlacknessPredict blacknessPredict;

        private readonly BlacknessResult originalBlacknessResult;

        private readonly BlacknessResult tempBlacknessResult;

        private List<int> sortedIDs = [];

        public BlacknessMethod(MainWindow _form)
        {
            form = _form;
            InitializeComponent();
            blacknessMethodBLL = new BlacknessMethodBLL();
            imageProcessBLL = new ImageProcessBLL("Blackness");
            imageProcessBLL.PropertyChanged += ImageProcessBLL_PropertyChanged;
            blacknessPredict = new BlacknessPredict(imageProcessBLL);
            blacknessPredict.PropertyChanged += BlacknessPredict_PropertyChanged;
            cameraBLL = new CameraBLL("Blackness", avatarOriginImage.Handle);
            cameraBLL.PropertyChanged += CameraBLL_PropertyChanged;
            CameraHelper.CAMERA_DEVICES.Add(cameraBLL);
            originalBlacknessResult = new BlacknessResult();
            originalBlacknessResult.PropertyChanged += OriginalBlacknessResult_PropertyChanged;
            tempBlacknessResult = new BlacknessResult();
            tempBlacknessResult.PropertyChanged += TempBlacknessResult_PropertyChanged;
            this.Load += async (s, e) => await InitializeAsync();
            this.HandleDestroyed += async (s, e) => await DestoryAsync();
        }

        private void OriginalBlacknessResult_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Id")
            {
                tempBlacknessResult.Id = originalBlacknessResult.Id;
            }
            else if (e.PropertyName == "OriginImagePath")
            {
                imageProcessBLL.OriginImagePath = originalBlacknessResult.OriginImagePath;
            }
            else if (e.PropertyName == "RenderImagePath")
            {
                imageProcessBLL.RenderImagePath = originalBlacknessResult.RenderImagePath;
            }
            else if (e.PropertyName == "WorkGroup")
            {
                selectWorkGroup.SelectedValue = originalBlacknessResult.WorkGroup;
            }
            else if (e.PropertyName == "TestNo")
            {
                selectTestNo.SelectedValue = originalBlacknessResult.TestNo;
            }
            else if (e.PropertyName == "CoilNumber")
            {
                inputCoilNumber.Text = originalBlacknessResult.CoilNumber;
            }
            else if (e.PropertyName == "Size")
            {
                inputSize.Text = originalBlacknessResult.Size;
            }
            else if (e.PropertyName == "Analyst")
            {
                inputAnalyst.Text = originalBlacknessResult.Analyst;
            }
            else if (e.PropertyName == "Items")
            {
                // 观察者模式虽好，但是跳来跳去，不好好考虑，真的容易多写代码
                blacknessPredict.Predictions = originalBlacknessResult.Items.Select(x => x.Prediction).ToArray();
            }
        }

        private void TempBlacknessResult_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!originalBlacknessResult.Equals(tempBlacknessResult))
            {
                try
                {
                    CheckValid();
                    btnSave.Enabled = true;
                }
                catch (Exception)
                {
                    btnSave.Enabled = false;
                }
            }
            else
            {
                btnSave.Enabled = false;
            }
            MainWindow.SOMETHING_IS_UNDONE = btnSave.Enabled;
        }

        private void CheckValid()
        {
            // 数据验证
            if (tempBlacknessResult == null || tempBlacknessResult.Items.Count == 0)
            {
                throw new Exception(LocalizeHelper.PLEASE_PREDICT_BEFORE_SAVING);
            }
            if (selectWorkGroup.SelectedValue == null)
            {
                throw new Exception(LocalizeHelper.PLEASE_SELECT_WORKBENCH);
            }
            if (selectTestNo.SelectedValue == null)
            {
                throw new Exception(LocalizeHelper.PLEASE_SELECT_TESTNO);
            }
            if (string.IsNullOrEmpty(inputCoilNumber.Text))
            {
                throw new Exception(LocalizeHelper.PLEASE_INPUT_COIL_NUMBER);
            }
            if (string.IsNullOrEmpty(inputSize.Text))
            {
                throw new Exception(LocalizeHelper.PLEASE_INPUT_SIZE);
            }
            if (string.IsNullOrEmpty(inputAnalyst.Text))
            {
                throw new Exception(LocalizeHelper.PLEASE_INPUT_ANALYST);
            }
        }

        private void BlacknessPredict_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Predictions")
            {
                if (blacknessPredict.Predictions != null && blacknessPredict.Predictions.Length != 0)
                {
                    OutputTexts();
                    // call when edit
                    if (!originalBlacknessResult.OriginImagePath.Equals(tempBlacknessResult.OriginImagePath))
                    {
                        AntdUI.Notification.success(form, LocalizeHelper.SUCCESS, LocalizeHelper.PREDICTED_SUCCESSFULLY, AntdUI.TAlignFrom.BR, Font);
                    }
                }
                else
                {
                    ClearTexts();
                    if (!originalBlacknessResult.OriginImagePath.Equals(tempBlacknessResult.OriginImagePath))
                    {
                        AntdUI.Notification.warn(form, LocalizeHelper.PROMPT, LocalizeHelper.PLEASE_USE_CORRECT_IMAGE, AntdUI.TAlignFrom.BR, Font);
                    }
                }
            }
        }

        private void ImageProcessBLL_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "OriginImagePath")
            {
                tempBlacknessResult.OriginImagePath = imageProcessBLL.OriginImagePath;
                btnPredict.Enabled = !string.IsNullOrEmpty(imageProcessBLL.OriginImagePath);
                if (!string.IsNullOrEmpty(imageProcessBLL.OriginImagePath))
                {
                    avatarOriginImage.Image = Image.FromFile(imageProcessBLL.OriginImagePath);
                    // cant aotu predict under the status of editing
                    if (!originalBlacknessResult.OriginImagePath.Equals(tempBlacknessResult.OriginImagePath))
                    {
                        BtnPredict_Click(btnPredict, null);  // auto predict
                    }
                }
                else
                {
                    avatarOriginImage.Image = Properties.Resources.blackness_template;
                }
            }
            else if (e.PropertyName == "RenderImagePath")
            {
                tempBlacknessResult.RenderImagePath = imageProcessBLL.RenderImagePath;
                if (!string.IsNullOrEmpty(imageProcessBLL.RenderImagePath))
                {
                    avatarRenderImage.Image = Image.FromFile(imageProcessBLL.RenderImagePath);
                }
                else
                {
                    avatarRenderImage.Image = Properties.Resources.blackness_template;
                }
            }
        }

        private void CameraBLL_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsGrabbing")
            {
                btnCameraRecover.Enabled = !cameraBLL.IsGrabbing;
                btnCameraCapture.Enabled = cameraBLL.IsGrabbing;
            }
            else if (e.PropertyName == "IsConnected")
            {
                btnCameraRecover.Enabled = cameraBLL.IsConnected && !cameraBLL.IsGrabbing;
                btnCameraCapture.Enabled = cameraBLL.IsConnected && cameraBLL.IsGrabbing;
                if (cameraBLL.IsConnected)
                {
                    AntdUI.Notification.success(form, LocalizeHelper.SUCCESS, LocalizeHelper.CAMERA_CONNECTED, AntdUI.TAlignFrom.BR, Font);
                }
                else
                {
                    AntdUI.Notification.warn(form, LocalizeHelper.PROMPT, LocalizeHelper.CAMERA_DISCONNECTED, AntdUI.TAlignFrom.BR, Font);
                }
            }
        }

        private async Task DestoryAsync()
        {
            sortedIDs = [];
            EDIT_ITEM_ID = string.Empty;
            await Task.Delay(50);
            cameraBLL.CloseDevice();
            CameraHelper.CAMERA_DEVICES.Remove(cameraBLL);
        }

        private async Task InitializeAsync()
        {
            // TestNo List
            await InitializeSelectTestNoListAsync();
            try
            {
                // 通过观察者模式实现界面数据效果
                sortedIDs = blacknessMethodBLL.LoadOriginalResultFromDB(originalBlacknessResult, EDIT_ITEM_ID);
                if (!string.IsNullOrEmpty(EDIT_ITEM_ID))
                {
                    // The order is very important.:)
                    btnNext.Visible = true;
                    btnNext.Enabled = sortedIDs.Count > 0 && sortedIDs.FindIndex(t => t.ToString().Equals(EDIT_ITEM_ID)) != sortedIDs.Count - 1;
                    btnPrint.Visible = true;
                    btnPre.Visible = true;
                    btnPre.Enabled = sortedIDs.Count > 0 && sortedIDs.FindIndex(t => t.ToString().Equals(EDIT_ITEM_ID)) != 0;
                    AntdUI.Notification.success(form, LocalizeHelper.SUCCESS, LocalizeHelper.BLACKNESS_EDIT_MODE(EDIT_ITEM_ID), AntdUI.TAlignFrom.BR, Font);
                    return;
                }
                else
                {
                    btnNext.Visible = false;
                    btnPrint.Visible = false;
                    btnPre.Visible = false;
                    AntdUI.Notification.success(form, LocalizeHelper.SUCCESS, LocalizeHelper.BLACKNESS_NEW_MODE, AntdUI.TAlignFrom.BR, Font);
                }
                var result = cameraBLL.StartRendering();
                switch (result)
                {
                    case CameraBLLStatusKind.NoCamera:
                        AntdUI.Notification.warn(form, LocalizeHelper.PROMPT, LocalizeHelper.NO_CAMERA, AntdUI.TAlignFrom.BR, Font);
                        break;
                    case CameraBLLStatusKind.NoCameraSettings:
                        AntdUI.Notification.warn(form, LocalizeHelper.PROMPT, LocalizeHelper.NO_CAMERA_SETTING, AntdUI.TAlignFrom.BR, Font);
                        // 延迟1秒
                        await Task.Delay(1000);
                        BtnCameraSetting_Click(null, null);
                        break;
                    case CameraBLLStatusKind.NoCameraOpen:
                        AntdUI.Notification.warn(form, LocalizeHelper.PROMPT, LocalizeHelper.NO_CAMERA_OPEN, AntdUI.TAlignFrom.BR, Font);
                        // 延迟1秒
                        await Task.Delay(1000);
                        BtnCameraSetting_Click(null, null);
                        break;
                    case CameraBLLStatusKind.NoCameraGrabbing:
                        AntdUI.Notification.warn(form, LocalizeHelper.PROMPT, LocalizeHelper.NO_CAMERA_GRABBING, AntdUI.TAlignFrom.BR, Font);
                        // 延迟1秒
                        await Task.Delay(1000);
                        BtnCameraSetting_Click(null, null);
                        break;
                    case CameraBLLStatusKind.TriggerMode:
                        AntdUI.Notification.success(form, LocalizeHelper.SUCCESS, LocalizeHelper.TRIGGER_MODE, AntdUI.TAlignFrom.BR, Font);
                        break;
                    case CameraBLLStatusKind.ContinuousMode:
                        AntdUI.Notification.success(form, LocalizeHelper.SUCCESS, LocalizeHelper.CONTINUOUS_MODE, AntdUI.TAlignFrom.BR, Font);
                        break;
                    default:
                        AntdUI.Notification.error(form, LocalizeHelper.ERROR, LocalizeHelper.PLEASE_CONTACT_ADMIN, AntdUI.TAlignFrom.BR, Font);
                        break;
                }
            }
            catch (CameraSDKException error)
            {
                CameraHelper.ShowErrorMsg(form, error.Message, error.ErrorCode);
            }
            catch (Exception error)
            {
                AntdUI.Notification.error(form, LocalizeHelper.ERROR, error.Message, AntdUI.TAlignFrom.BR, Font);
            }
        }

        private List<GetTestNoListResponse> testNoList;
        private async Task InitializeSelectTestNoListAsync()
        {
            try
            {
                selectTestNo.Items.Clear();
                testNoList = await blacknessMethodBLL.GetTestNoList();
            }
            catch (Exception error)
            {
                AntdUI.Notification.error(form, LocalizeHelper.ERROR, error.Message, AntdUI.TAlignFrom.BR, Font);
            }
            // select testNo
            var result = testNoList.Select(t => t.TestNo).Distinct().OrderDescending().ToList();
            if (result != null && result.Count != 0)
            {
                selectTestNo.Items.AddRange([.. result]);
            }
            else
            {
                AntdUI.Notification.warn(form, LocalizeHelper.PROMPT, LocalizeHelper.ONLY_TEST_NO, AntdUI.TAlignFrom.BR, Font);
                selectTestNo.Items.AddRange(["test"]);
            }
        }

        private void AvatarOriginImage_Click(object sender, System.EventArgs e)
        {
            AntdUI.Preview.open(new AntdUI.Preview.Config(form, avatarOriginImage.Image));
        }

        private void AvatarRenderImage_Click(object sender, EventArgs e)
        {
            AntdUI.Preview.open(new AntdUI.Preview.Config(form, avatarRenderImage.Image));
        }

        private void BtnUploadImage_Click(object sender, System.EventArgs e)
        {
            if (blacknessMethod_OpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                AntdUI.Button btn = (AntdUI.Button)sender;
                btn.LoadingWaveValue = 0;
                btn.Loading = true;
                AntdUI.ITask.Run(() =>
                {
                    imageProcessBLL.SaveOriginImage(blacknessMethod_OpenFileDialog.FileName);
                    // stop realtime image render
                    cameraBLL.StopGrabbing();
                    AntdUI.Notification.success(form, LocalizeHelper.SUCCESS, LocalizeHelper.UPLOAD_ORIGINAL_IMAGE_SUCCESSFULLY, AntdUI.TAlignFrom.BR, Font);
                    if (btn.IsDisposed) return;
                    btn.Loading = false;
                });
            }
        }

        private void BtnPredict_Click(object sender, EventArgs e)
        {
            AntdUI.Button btn = (AntdUI.Button)sender;
            btn.LoadingWaveValue = 0;
            btn.Loading = true;
            AntdUI.ITask.Run(() =>
            {
                blacknessPredict.Predict();
            }, () =>
            {
                if (btn.IsDisposed) return;
                btn.Loading = false;
            });
        }

        private void OutputTexts()
        {
            var sorted = blacknessPredict.Predictions.OrderBy(t => t.Rectangle.Y).ToArray();
            // TODO: 现场根据X具体位置判断，因为可能出现未贴完6个部位或者未识别出六个部位的情况
            tempBlacknessResult.Items = [new(BlacknessLocationKind.SURFACE_OP, sorted[0]),
                            new(BlacknessLocationKind.SURFACE_CE, sorted[1]),
                            new(BlacknessLocationKind.SURFACE_DR, sorted[2]),
                            new(BlacknessLocationKind.INSIDE_OP, sorted[3]),
                            new(BlacknessLocationKind.INSIDE_CE, sorted[4]),
                            new(BlacknessLocationKind.INSIDE_DR, sorted[5])];
            inputSurfaceOP.Text = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_OP))?.Description;
            inputSurfaceCE.Text = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_CE))?.Description;
            inputSurfaceDR.Text = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_DR))?.Description;
            inputInsideOP.Text = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_OP))?.Description;
            inputInsideCE.Text = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_CE))?.Description;
            inputInsideDR.Text = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_DR))?.Description;
            radioResultOK.Checked = tempBlacknessResult.Items.All(t => new List<string> { "3", "4", "5" }.Contains(t.Level)); // [1，2] not exit，otherwise，NG is checked
        }

        private void ClearTexts()
        {
            tempBlacknessResult.Items = [];
            inputSurfaceOP.Text = string.Empty;
            inputSurfaceCE.Text = string.Empty;
            inputSurfaceDR.Text = string.Empty;
            inputInsideOP.Text = string.Empty;
            inputInsideCE.Text = string.Empty;
            inputInsideDR.Text = string.Empty;
            radioResultNG.Checked = true;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // double check
            try
            {
                CheckValid();
            }
            catch (Exception error)
            {
                AntdUI.Notification.warn(form, LocalizeHelper.PROMPT, error.Message, AntdUI.TAlignFrom.BR, Font);
                return;
            }
            if (AntdUI.Modal.open(form, LocalizeHelper.CONFIRM, LocalizeHelper.WOULD_SAVE_BLACKNESS_RESULT) == DialogResult.OK)
            {
                AntdUI.Button btn = (AntdUI.Button)sender;
                btn.LoadingWaveValue = 0;
                btn.Loading = true;
                AntdUI.ITask.Run(() =>
                {
                    try
                    {
                        var result = blacknessMethodBLL.SaveResult(tempBlacknessResult);
                        if (result == 0)
                        {
                            AntdUI.Notification.error(form, LocalizeHelper.ERROR, LocalizeHelper.SAVE_FAILED, AntdUI.TAlignFrom.BR, Font);
                            return;
                        }
                        else
                        {
                            AntdUI.Notification.success(form, LocalizeHelper.SUCCESS, LocalizeHelper.SAVE_SUCCESSFULLY, AntdUI.TAlignFrom.BR, Font);
                            EDIT_ITEM_ID = result.ToString();
                            _ = InitializeAsync();
                            btn.Enabled = false;
                            MainWindow.SOMETHING_IS_UNDONE = false;
                            return;
                        }
                    }
                    catch (Exception error)
                    {
                        AntdUI.Notification.error(form, LocalizeHelper.ERROR, error.Message, AntdUI.TAlignFrom.BR, Font);
                        return;
                    }
                }, () =>
                {
                    if (btn.IsDisposed) return;
                    btn.Loading = false;
                });
            }
        }

        private void BtnCameraSetting_Click(object sender, EventArgs e)
        {
            try
            {
                AntdUI.Drawer.open(form, new CameraSetting(form, cameraBLL)
                {
                    Size = new Size(420, 555)
                }, AntdUI.TAlignMini.Right);
            }
            catch (Exception error)
            {
                AntdUI.Notification.error(form, LocalizeHelper.ERROR, error.Message, AntdUI.TAlignFrom.BR, Font);
            }

        }

        private void BtnCameraRecover_Click(object sender, EventArgs e)
        {
            // start realtime image render
            cameraBLL.StartGrabbing();
        }

        private void BtnCameraCapture_Click(object sender, EventArgs e)
        {
            imageProcessBLL.OriginImagePath = cameraBLL.SaveImage();
            AntdUI.Button btn = (AntdUI.Button)sender;
            btn.LoadingWaveValue = 0;
            btn.Loading = true;
            AntdUI.ITask.Run(() =>
            {
                // stop realtime image render
                cameraBLL.StopGrabbing();
                if (btn.IsDisposed) return;
                btn.Loading = false;
                AntdUI.Notification.success(form, LocalizeHelper.SUCCESS, LocalizeHelper.CAMERA_CAPTURED_SUCCESSFULLY, AntdUI.TAlignFrom.BR, Font);
            });
        }

        private void SelectWorkGroup_SelectedIndexChanged(object sender, AntdUI.IntEventArgs e)
        {
            tempBlacknessResult.WorkGroup = selectWorkGroup.SelectedValue.ToString();
        }

        private void SelectTestNo_SelectedIndexChanged(object sender, AntdUI.IntEventArgs e)
        {
            tempBlacknessResult.TestNo = selectTestNo.SelectedValue.ToString();
            // coilNumber
            var target = testNoList.FirstOrDefault(t => tempBlacknessResult.TestNo.Equals(t.TestNo));
            if (target != null)
            {
                inputCoilNumber.Text = string.IsNullOrEmpty(target.CoilNumber) ? target.OtherCoilNumber : target.CoilNumber;
            }
        }

        private void InputCoilNumber_TextChanged(object sender, EventArgs e)
        {
            tempBlacknessResult.CoilNumber = inputCoilNumber.Text;
        }

        private void InputSize_TextChanged(object sender, EventArgs e)
        {
            tempBlacknessResult.Size = inputSize.Text;
        }

        private void InputAnalyst_TextChanged(object sender, EventArgs e)
        {
            tempBlacknessResult.Analyst = inputAnalyst.Text;
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            EDIT_ITEM_ID = string.Empty;
            _ = InitializeAsync();
        }

        private void BtnHistory_Click(object sender, EventArgs e)
        {
            if (AntdUI.Modal.open(form, LocalizeHelper.CONFIRM, LocalizeHelper.JUMP_TO_HISTORY) == DialogResult.OK)
            {
                try
                {
                    form.OpenPage("Historical Record Of V60 Blackness Method");
                }
                catch (Exception error)
                {
                    AntdUI.Notification.error(form, LocalizeHelper.ERROR, error.Message, AntdUI.TAlignFrom.BR, Font);
                }
            }
        }

        private void BtnPre_Click(object sender, EventArgs e)
        {
            var index = sortedIDs.FindIndex(t => t.ToString().Equals(EDIT_ITEM_ID));
            if (index != 0)
            {
                EDIT_ITEM_ID = (sortedIDs[index - 1]).ToString();
                _ = InitializeAsync();
            }
        }

        private void BtnNext_Click(object sender, EventArgs e)
        {
            var index = sortedIDs.FindIndex(t => t.ToString().Equals(EDIT_ITEM_ID));
            if (index != sortedIDs.Count)
            {
                EDIT_ITEM_ID = (sortedIDs[index + 1]).ToString();
                _ = InitializeAsync();
            }
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                AntdUI.Drawer.open(form, new BlacknessReport(form, EDIT_ITEM_ID)
                {
                    Size = new Size(420, 596)  // 常用到的纸张规格为A4，即21cm×29.7cm（210mm×297mm）
                }, AntdUI.TAlignMini.Right);
            }
            catch (Exception error)
            {
                AntdUI.Notification.error(form, LocalizeHelper.ERROR, error.Message, AntdUI.TAlignFrom.BR, Font);
            }
        }
    }
}