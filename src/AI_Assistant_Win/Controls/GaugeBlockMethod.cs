using AI_Assistant_Win.Business;
using AI_Assistant_Win.Models;
using AI_Assistant_Win.Models.Enums;
using AI_Assistant_Win.Models.Middle;
using AI_Assistant_Win.Models.Response;
using AI_Assistant_Win.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using YoloDotNet.Extensions;

namespace AI_Assistant_Win.Controls
{
    public partial class GaugeBlockMethod : UserControl
    {
        public static string EDIT_ITEM_ID = string.Empty;

        private readonly MainWindow form;

        private readonly GaugeBlockMethodBLL gaugeBlockMethodBLL;

        private readonly ImageProcessBLL imageProcessBLL;

        private readonly CameraBLL cameraBLL;

        private readonly GaugeBlockPredict gaugeBlockPredict;

        private readonly GaugeBlockResult originalGaugeBlockResult;

        private readonly GaugeBlockResult tempGaugeBlockResult;

        private List<int> sortedIDs = [];

        private readonly ObservableDictionary<string, CalculateScale> scaleList = [];

        public GaugeBlockMethod(MainWindow _form)
        {
            form = _form;
            InitializeComponent();
            gaugeBlockMethodBLL = new GaugeBlockMethodBLL();
            imageProcessBLL = new ImageProcessBLL("GaugeBlock");
            imageProcessBLL.PropertyChanged += ImageProcessBLL_PropertyChanged;
            gaugeBlockPredict = new GaugeBlockPredict(imageProcessBLL);
            gaugeBlockPredict.PropertyChanged += GaugeBlockPredict_PropertyChanged;
            cameraBLL = new CameraBLL("GaugeBlock", avatarOriginImage.Handle);
            cameraBLL.PropertyChanged += CameraBLL_PropertyChanged;
            CameraHelper.CAMERA_DEVICES.Add(cameraBLL);
            originalGaugeBlockResult = new GaugeBlockResult();
            originalGaugeBlockResult.PropertyChanged += OriginalGaugeBlockResult_PropertyChanged;
            tempGaugeBlockResult = new GaugeBlockResult();
            tempGaugeBlockResult.PropertyChanged += TempGaugeBlockResult_PropertyChanged;
            scaleList.Changed += ScaleList_Changed;
            this.Load += async (s, e) => await InitializeAsync();
            this.HandleDestroyed += async (s, e) => await DestoryAsync();
        }

        private void ScaleList_Changed(object sender, ObservableDictionary<string, CalculateScale>.ChangedEventArgs<string, CalculateScale> e)
        {
            selectScale.Items.Clear();
            selectScale.Items.AddRange(scaleList.Select(t => $"{FormatScaleName(t)}").ToArray());
            if (e.Action == ObservableDictionary<string, CalculateScale>.ChangedAction.Add)
            {
                Console.WriteLine($"Added: Key={e.Key}, Value={e.NewValue}");
                var text = FormatScaleName(new KeyValuePair<string, CalculateScale>(e.Key, e.NewValue));
                selectScale.SelectedValue = text;
                btnSetScale.Enabled = true;
                AntdUI.Message.success(form, $"{LocalizeHelper.SCALE_LOAD_SUCCESSED}{text}");
            }
            else if (e.Action == ObservableDictionary<string, CalculateScale>.ChangedAction.Remove)
            {
                Console.WriteLine($"Removed: Key={e.Key}, Value={e.OldValue}");
            }
            else if (e.Action == ObservableDictionary<string, CalculateScale>.ChangedAction.Update)
            {
                Console.WriteLine($"Updated: Key={e.Key}, OldValue={e.OldValue}, NewValue={e.NewValue}");
                var text = FormatScaleName(new KeyValuePair<string, CalculateScale>(e.Key, e.NewValue));
                selectScale.SelectedValue = text;
                AntdUI.Message.success(form, $"{LocalizeHelper.SCALE_LOAD_SUCCESSED}{text}");
            }
        }

        private string FormatScaleName(KeyValuePair<string, CalculateScale> t)
        {
            var text = string.Empty;
            switch (t.Key)
            {
                case "current":
                    text = $"{LocalizeHelper.CURRENT_SCALE_TITLE}[{t.Value.Value:F2}{LocalizeHelper.CIRCULAR_SCALE_CACULATED_RATIO_UNIT}]";
                    break;
                case "atThatTime":
                    text = $"{LocalizeHelper.SCALE_TITLE_AT_THAT_TIME}[{t.Value.Value:F2}{LocalizeHelper.CIRCULAR_SCALE_CACULATED_RATIO_UNIT}]";
                    break;
                default:
                    break;
            }
            return text;
        }

        private void OriginalGaugeBlockResult_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Id")
            {
                tempGaugeBlockResult.Id = originalGaugeBlockResult.Id;
            }
            else if (e.PropertyName == "TestNo")
            {
                selectTestNo.SelectedValue = originalGaugeBlockResult.TestNo;
            }
            else if (e.PropertyName == "CoilNumber")
            {
                inputCoilNumber.Text = originalGaugeBlockResult.CoilNumber;
            }
            else if (e.PropertyName == "Position")
            {
                selectPosition.SelectedValue = originalGaugeBlockResult.Position;
            }
            else if (e.PropertyName == "OriginImagePath")
            {
                imageProcessBLL.OriginImagePath = originalGaugeBlockResult.OriginImagePath;
            }
            else if (e.PropertyName == "RenderImagePath")
            {
                imageProcessBLL.RenderImagePath = originalGaugeBlockResult.RenderImagePath;
            }
            else if (e.PropertyName == "WorkGroup")
            {
                selectWorkGroup.SelectedValue = originalGaugeBlockResult.WorkGroup;
            }
            else if (e.PropertyName == "Analyst")
            {
                inputAnalyst.Text = originalGaugeBlockResult.Analyst;
            }
            else if (e.PropertyName == "CalculateScale")
            {
                if (originalGaugeBlockResult.CalculateScale == null)
                {
                    selectScale.SelectedValue = null;
                    AntdUI.Notification.warn(form, LocalizeHelper.PROMPT, LocalizeHelper.LOST_SCALE, AntdUI.TAlignFrom.BR, Font);
                }
                else
                {
                    // different scale
                    if (!originalGaugeBlockResult.CalculateScale.Equals(scaleList.FirstOrDefault(t => "current".Equals(t.Key)).Value))
                    {
                        scaleList["atThatTime"] = originalGaugeBlockResult.CalculateScale;
                    }
                }
            }
            else if (e.PropertyName == "Item")
            {
                // 观察者模式虽好，但是跳来跳去，不好好考虑，真的容易多写代码
                gaugeBlockPredict.Prediction = originalGaugeBlockResult.Item?.Prediction;
            }
        }
        private void TempGaugeBlockResult_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!originalGaugeBlockResult.Equals(tempGaugeBlockResult))
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
            if (selectScale.SelectedValue == null)
            {
                throw new Exception(LocalizeHelper.PLEASE_SELECT_SCALE);
            }
            if (tempGaugeBlockResult == null || tempGaugeBlockResult.Item == null)
            {
                throw new Exception(LocalizeHelper.PLEASE_PREDICT_BEFORE_SAVING);
            }
            // 比例尺不一致
            if (!tempGaugeBlockResult.CalculateScale.Equals(tempGaugeBlockResult.Item.CalculateScale))
            {
                throw new Exception(LocalizeHelper.PLEASE_PREDICT_WITH_NEW_SCALE_BEFORE_SAVING);
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
            if (selectPosition.SelectedValue == null)
            {
                throw new Exception(LocalizeHelper.PLEASE_SELECT_POSITION);
            }
            if (string.IsNullOrEmpty(inputAnalyst.Text))
            {
                throw new Exception(LocalizeHelper.PLEASE_INPUT_ANALYST);
            }
        }
        private void GaugeBlockPredict_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Prediction")
            {
                if (gaugeBlockPredict.Prediction != null)
                {
                    OutputTexts();
                    btnSetScale.Enabled = true;
                    checkboxRedefine.Enabled = true;
                    // for add scale firstly
                    if (scaleList.Count == 0)
                    {
                        BtnSetScale_Click(null, null);
                    }
                    // call when edit
                    if (!originalGaugeBlockResult.OriginImagePath.Equals(tempGaugeBlockResult.OriginImagePath))
                    {
                        AntdUI.Message.success(form, LocalizeHelper.PREDICTED_SUCCESSFULLY);
                    }
                }
                else
                {
                    checkboxRedefine.Checked = false;
                    checkboxRedefine.Enabled = false;
                    ClearTexts();
                    if (!originalGaugeBlockResult.OriginImagePath.Equals(tempGaugeBlockResult.OriginImagePath))
                    {
                        AntdUI.Notification.warn(form, LocalizeHelper.PROMPT, LocalizeHelper.PLEASE_USE_CORRECT_CIRCULAR_IMAGE, AntdUI.TAlignFrom.BR, Font);
                    }
                }
            }
        }
        private void ImageProcessBLL_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "OriginImagePath")
            {
                tempGaugeBlockResult.OriginImagePath = imageProcessBLL.OriginImagePath;
                btnPredict.Enabled = !string.IsNullOrEmpty(imageProcessBLL.OriginImagePath);
                if (!string.IsNullOrEmpty(imageProcessBLL.OriginImagePath))
                {
                    avatarOriginImage.Image = Image.FromFile(imageProcessBLL.OriginImagePath);
                    // cant predict automatically under the status of editing
                    if (!originalGaugeBlockResult.OriginImagePath.Equals(tempGaugeBlockResult.OriginImagePath))
                    {
                        BtnPredict_Click(btnPredict, null);  // auto predict
                    }
                }
                else
                {
                    avatarOriginImage.Image = Properties.Resources.gauge_template;
                }
            }
            else if (e.PropertyName == "RenderImagePath")
            {
                tempGaugeBlockResult.RenderImagePath = imageProcessBLL.RenderImagePath;
                if (!string.IsNullOrEmpty(imageProcessBLL.RenderImagePath))
                {
                    avatarRenderImage.Image = Image.FromFile(imageProcessBLL.RenderImagePath);
                }
                else
                {
                    avatarRenderImage.Image = Properties.Resources.gauge_template;
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
                    AntdUI.Message.success(form, LocalizeHelper.CAMERA_CONNECTED);
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
            AntdUI.Message.loading(form, LocalizeHelper.LOADING_PAGE, async (config) =>
            {
                // TestNo List
                await InitializeSelectTestNoAsync();
                config.OK(LocalizeHelper.TESTNO_LIST_LOADED_SUCCESS);
                // Scale List
                if (!InitializeSelectScale())
                {
                    // tell client how to do when any scale does not exit.
                    BtnSetScale_Click(null, null);
                }
                // Position List
                InitializeSelectPostion();
                try
                {
                    // 通过观察者模式实现界面数据效果
                    sortedIDs = gaugeBlockMethodBLL.LoadOriginalResultFromDB(originalGaugeBlockResult, EDIT_ITEM_ID);
                    if (!string.IsNullOrEmpty(EDIT_ITEM_ID))
                    {
                        // The order is very important.:)
                        btnNext.Visible = true;
                        btnNext.Enabled = sortedIDs.Count > 0 && sortedIDs.FindIndex(t => t.ToString().Equals(EDIT_ITEM_ID)) != sortedIDs.Count - 1;
                        btnPrint.Visible = true;
                        btnPre.Visible = true;
                        btnPre.Enabled = sortedIDs.Count > 0 && sortedIDs.FindIndex(t => t.ToString().Equals(EDIT_ITEM_ID)) != 0;
                        //AntdUI.Message.success(form, LocalizeHelper.CIRCULAR_AREA_EDIT_MODE(originalGaugeBlockResult));
                        return;
                    }
                    else
                    {
                        btnNext.Visible = false;
                        btnPrint.Visible = false;
                        btnPre.Visible = false;
                        AntdUI.Message.success(form, LocalizeHelper.NEW_MODE);
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
                            await Task.Delay(500);
                            BtnCameraSetting_Click(null, null);
                            break;
                        case CameraBLLStatusKind.NoCameraOpen:
                            AntdUI.Notification.warn(form, LocalizeHelper.PROMPT, LocalizeHelper.NO_CAMERA_OPEN, AntdUI.TAlignFrom.BR, Font);
                            // 延迟1秒
                            await Task.Delay(500);
                            BtnCameraSetting_Click(null, null);
                            break;
                        case CameraBLLStatusKind.NoCameraGrabbing:
                            AntdUI.Notification.warn(form, LocalizeHelper.PROMPT, LocalizeHelper.NO_CAMERA_GRABBING, AntdUI.TAlignFrom.BR, Font);
                            // 延迟1秒
                            await Task.Delay(500);
                            BtnCameraSetting_Click(null, null);
                            break;
                        case CameraBLLStatusKind.TriggerMode:
                            AntdUI.Message.success(form, LocalizeHelper.TRIGGER_MODE);
                            break;
                        case CameraBLLStatusKind.ContinuousMode:
                            AntdUI.Message.success(form, LocalizeHelper.CONTINUOUS_MODE);
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
                config.OK(LocalizeHelper.PAGE_LOADED_SUCCESS);
            }, Font);
            await Task.Delay(50);
        }
        private void InitializeSelectPostion()
        {

            selectPosition.Items.Clear();
            // select position
            var result = gaugeBlockMethodBLL.PositionList.Select(t => t.Value).ToList();
            selectPosition.Items.AddRange([.. result]);
        }
        private bool InitializeSelectScale()
        {
            scaleList.Clear();
            var currentScaleFromDB = gaugeBlockMethodBLL.GetCurrentScale();
            if (currentScaleFromDB == null)
            {
                AntdUI.Notification.warn(form, LocalizeHelper.PROMPT, LocalizeHelper.PLEASE_SET_SCALE, AntdUI.TAlignFrom.BR, Font);
                return false;
            }
            scaleList["current"] = currentScaleFromDB;
            return true;
        }
        private List<GetTestNoListResponse> testNoList;
        private async Task InitializeSelectTestNoAsync()
        {
            try
            {
                selectTestNo.Items.Clear();
                testNoList = await gaugeBlockMethodBLL.GetTestNoListAsync();
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
            if (areaMethod_OpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                AntdUI.Button btn = (AntdUI.Button)sender;
                btn.LoadingWaveValue = 0;
                btn.Loading = true;
                AntdUI.ITask.Run(() =>
                {
                    imageProcessBLL.SaveOriginImage(areaMethod_OpenFileDialog.FileName);
                    // stop realtime image render
                    cameraBLL.StopGrabbing();
                    AntdUI.Message.success(form, LocalizeHelper.UPLOAD_ORIGINAL_IMAGE_SUCCESSFULLY);
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
                // current scale
                gaugeBlockPredict.Predict(CurrentScale);
            }, () =>
            {
                if (btn.IsDisposed) return;
                btn.Loading = false;
            });
        }
        private CalculateScale CurrentScale
        {
            get => checkboxRedefine.Checked || selectScale.SelectedValue == null ? null : scaleList.FirstOrDefault(t => selectScale.SelectedValue.Equals(FormatScaleName(t))).Value;
        }
        private void OutputTexts()
        {
            tempGaugeBlockResult.Item = new GaugeBlock(gaugeBlockPredict.Prediction, CurrentScale);
            inputAreaOfPixels.Text = $"{tempGaugeBlockResult.Item.AreaOfPixels}{LocalizeHelper.AREA_OF_PIXELS}";
            inputConfidence.Text = $"{tempGaugeBlockResult.Item.Confidence.ToPercent()}%";
            OutputScaleTexts(tempGaugeBlockResult.Item.CalculateScale);
            inputCalculatedArea.Text = $"{tempGaugeBlockResult.Item.CalculatedArea:F2}{tempGaugeBlockResult.Item.Unit}";
            inputDiameter.Text = $"{tempGaugeBlockResult.Item.Diameter:F2}{tempGaugeBlockResult.Item.DiameterUnit}";
        }
        private void OutputScaleTexts(CalculateScale calculateScale)
        {
            if (calculateScale != null)
            {
                inputScale.Text = $"{calculateScale.Value:F2}{LocalizeHelper.CIRCULAR_SCALE_CACULATED_RATIO_UNIT}";
                var settings = JsonConvert.DeserializeObject<CircularAreaScaleItem>(calculateScale.Settings);
                if (settings != null)
                {
                    inputGraduations.Text = settings.TopGraduations;
                }
            }
            else
            {
                inputScale.Text = string.Empty;
                inputGraduations.Text = string.Empty;
            }
        }
        private void ClearTexts()
        {
            tempGaugeBlockResult.Item = null;
            inputAreaOfPixels.Text = string.Empty;
            inputConfidence.Text = string.Empty;
            inputScale.Text = string.Empty;
            inputGraduations.Text = string.Empty;
            inputCalculatedArea.Text = string.Empty;
            inputDiameter.Text = string.Empty;
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
            if (AntdUI.Modal.open(form, LocalizeHelper.CONFIRM, originalGaugeBlockResult.IsUploaded ?
                LocalizeHelper.WOULD_RESAVE_CIRCULAR_AREA_RESULT_AFTER_UPLOADING :
                gaugeBlockMethodBLL.GetResultExitsInDB(tempGaugeBlockResult) != null ?
                  LocalizeHelper.WOULD_RESAVE_CIRCULAR_AREA_RESULT_ON_THIS_POSITION(tempGaugeBlockResult.Position) :
                LocalizeHelper.WOULD_SAVE_CIRCULAR_AREA_RESULT) == DialogResult.OK)
            {
                AntdUI.Button btn = (AntdUI.Button)sender;
                btn.LoadingWaveValue = 0;
                btn.Loading = true;
                AntdUI.ITask.Run(async () =>
                {
                    try
                    {
                        var result = gaugeBlockMethodBLL.SaveResult(tempGaugeBlockResult);
                        if (result == 0)
                        {
                            AntdUI.Notification.error(form, LocalizeHelper.ERROR, LocalizeHelper.SAVE_FAILED, AntdUI.TAlignFrom.BR, Font);
                            return;
                        }
                        else
                        {
                            AntdUI.Message.success(form, LocalizeHelper.SAVE_SUCCESSFULLY);
                            EDIT_ITEM_ID = result.ToString();
                            await InitializeAsync();
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
                AntdUI.Message.success(form, LocalizeHelper.CAMERA_CAPTURED_SUCCESSFULLY);
            });
        }
        private void SelectWorkGroup_SelectedIndexChanged(object sender, AntdUI.IntEventArgs e)
        {
            tempGaugeBlockResult.WorkGroup = selectWorkGroup.SelectedValue.ToString();
        }
        private void SelectTestNo_SelectedIndexChanged(object sender, AntdUI.IntEventArgs e)
        {
            tempGaugeBlockResult.TestNo = selectTestNo.SelectedValue.ToString();
            // coilNumber
            var target = testNoList.FirstOrDefault(t => tempGaugeBlockResult.TestNo.Equals(t.TestNo));
            if (target != null)
            {
                inputCoilNumber.Text = string.IsNullOrEmpty(target.CoilNumber) ? target.OtherCoilNumber : target.CoilNumber;
            }
        }
        private void InputCoilNumber_TextChanged(object sender, EventArgs e)
        {
            tempGaugeBlockResult.CoilNumber = inputCoilNumber.Text;
        }
        private void SelectPosition_SelectedIndexChanged(object sender, AntdUI.IntEventArgs e)
        {
            tempGaugeBlockResult.Position = selectPosition.SelectedValue.ToString();
        }
        private void InputAnalyst_TextChanged(object sender, EventArgs e)
        {
            tempGaugeBlockResult.Analyst = inputAnalyst.Text;
        }
        private void BtnClear_Click(object sender, EventArgs e)
        {
            ConfirmUnderUnsaved(async () =>
            {
                btnClear.Loading = true;
                EDIT_ITEM_ID = string.Empty;
                await InitializeAsync();
                btnClear.Loading = false;
            }, LocalizeHelper.CLEAR_PAGE_CONFIRM_WHEN_SOMETHING_IS_UNDONE);
        }
        private void ConfirmUnderUnsaved(Action action, string content)
        {
            if (MainWindow.SOMETHING_IS_UNDONE)
            {
                var result = AntdUI.Modal.open(new AntdUI.Modal.Config(LocalizeHelper.CONFIRM,
                    content,
                    AntdUI.TType.Error)
                {
                    OnButtonStyle = (id, btn) =>
                    {
                        btn.BackExtend = "135, #6253E1, #04BEFE";
                    },
                    CancelText = LocalizeHelper.CANCEL,
                    OkText = LocalizeHelper.CONFIRM
                });
                if (result == DialogResult.OK)
                {
                    BeginInvoke(action);
                };
            }
            else
            {
                // 异步
                BeginInvoke(action);
            }
        }
        private void BtnHistory_Click(object sender, EventArgs e)
        {
            if (AntdUI.Modal.open(form, LocalizeHelper.CONFIRM, LocalizeHelper.JUMP_TO_HISTORY) == DialogResult.OK)
            {
                try
                {
                    form.OpenPage("Historical Record Of Circular Area Measurement");
                }
                catch (Exception error)
                {
                    AntdUI.Notification.error(form, LocalizeHelper.ERROR, error.Message, AntdUI.TAlignFrom.BR, Font);
                }
            }
        }
        private void BtnPre_Click(object sender, EventArgs e)
        {
            ConfirmUnderUnsaved(async () =>
            {
                btnPre.Loading = true;
                var index = sortedIDs.FindIndex(t => t.ToString().Equals(EDIT_ITEM_ID));
                if (index != 0)
                {
                    EDIT_ITEM_ID = (sortedIDs[index - 1]).ToString();
                    await InitializeAsync();
                }
                btnPre.Loading = false;
            }, LocalizeHelper.PRE_RECORD_CONFIRM_WHEN_SOMETHING_IS_UNDONE);
        }
        private void BtnNext_Click(object sender, EventArgs e)
        {
            ConfirmUnderUnsaved(async () =>
            {
                btnNext.Loading = true;
                var index = sortedIDs.FindIndex(t => t.ToString().Equals(EDIT_ITEM_ID));
                if (index != sortedIDs.Count)
                {
                    EDIT_ITEM_ID = (sortedIDs[index + 1]).ToString();
                    await InitializeAsync();
                }
                btnNext.Loading = false;
            }, LocalizeHelper.NEXT_RECORD_CONFIRM_WHEN_SOMETHING_IS_UNDONE);

        }
        private void BtnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                AntdUI.Drawer.open(form, new CircularAreaReport(form, originalGaugeBlockResult.TestNo, () => { })
                {
                    Size = new Size(420, 596)  // 常用到的纸张规格为A4，即21cm×29.7cm（210mm×297mm）
                }, AntdUI.TAlignMini.Right);
            }
            catch (Exception error)
            {
                AntdUI.Notification.error(form, LocalizeHelper.ERROR, error.Message, AntdUI.TAlignFrom.BR, Font);
            }
        }
        private void BtnSetScale_Click(object sender, EventArgs e)
        {
            if ((string.IsNullOrEmpty(tempGaugeBlockResult.RenderImagePath) || tempGaugeBlockResult.Item == null) &&
                    (scaleList.Count == 0 ||   // for the first time
                    checkboxRedefine.Checked)
                )
            {
                AntdUI.Notification.warn(form, LocalizeHelper.PROMPT, LocalizeHelper.PREDICT_FIRSTLY_BEFORE_SETTING_GAUGE_SCALE, AntdUI.TAlignFrom.BR, Font);
                return;
            }
            var setting = new GaugeScaleSetting(form);
            setting.SetCurrentScaleDetails(tempGaugeBlockResult, checkboxRedefine.Checked, scaleList.FirstOrDefault(t => "atThatTime".Equals(t.Key)).Value);
            var result = AntdUI.Modal.open(new AntdUI.Modal.Config(form, LocalizeHelper.GAUGE_SCALE_SETTINGS_MODAL_TITLE, setting)
            {
                OnButtonStyle = (id, btn) =>
                {
                    btn.BackExtend = "135, #6253E1, #04BEFE";
                },
                CancelText = LocalizeHelper.CANCEL,
                OkText = LocalizeHelper.SETTING_SAVE,
                OnOk = config =>
                {
                    try
                    {
                        scaleList["current"] = setting.SaveSettings();   // 同时，正好利用了自带的转圈圈等待效果，简直完美2024年12月25日01点41分
                        return true;
                    }
                    catch (Exception error)
                    {
                        AntdUI.Notification.error(form, LocalizeHelper.FAIL, error.Message, AntdUI.TAlignFrom.BR, Font);
                        return false;
                    }
                }
            });
            if (result == DialogResult.OK)
            {
                checkboxRedefine.Checked = false;
            }
        }
        private void SelectScale_SelectedIndexChanged(object sender, AntdUI.IntEventArgs e)
        {
            if (selectScale.SelectedValue != null)
            {
                var selected = scaleList.FirstOrDefault(t => selectScale.SelectedValue.Equals(FormatScaleName(t))).Value;
                tempGaugeBlockResult.CalculateScale = selected;
            }
            else
            {
                tempGaugeBlockResult.CalculateScale = null;
            }
        }
        private void CheckboxRedefine_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            if (checkboxRedefine.Checked && !string.IsNullOrEmpty(tempGaugeBlockResult.RenderImagePath) && tempGaugeBlockResult.Item != null)
            {
                BtnSetScale_Click(null, null);
            }
        }
    }
}