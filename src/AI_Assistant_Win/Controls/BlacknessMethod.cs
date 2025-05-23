using AI_Assistant_Win.Business;
using AI_Assistant_Win.Models;
using AI_Assistant_Win.Models.Enums;
using AI_Assistant_Win.Models.Middle;
using AI_Assistant_Win.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AI_Assistant_Win.Controls
{
    public partial class BlacknessMethod : UserControl, IAsyncInit
    {
        public static string EDIT_METHOD_ID = string.Empty;

        private readonly MainWindow form;

        private readonly BlacknessMethodBLL blacknessMethodBLL;

        private readonly GaugeBlockMethodBLL gaugeBlockMethodBLL;

        private readonly ImageProcessBLL imageProcessBLL;

        private readonly CameraBLL cameraBLL;

        private readonly BlacknessPredict blacknessPredict;

        private readonly BlacknessResult originalBlacknessResult;

        private readonly BlacknessResult tempBlacknessResult;

        private List<int> sortedIDs = [];

        private readonly ObservableDictionary<string, CalculateScale> scaleList = [];

        private List<TestItem> testNoList;

        public BlacknessMethod(MainWindow _form)
        {
            form = _form;
            InitializeComponent();
            blacknessMethodBLL = new BlacknessMethodBLL();
            gaugeBlockMethodBLL = new GaugeBlockMethodBLL();
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
            scaleList.Changed += ScaleList_Changed;
            this.HandleDestroyed += async (s, e) => await DestoryAsync();
        }

        public async Task InitializeAsync()
        {
            var sw = Stopwatch.StartNew();
            var tcs = new TaskCompletionSource<bool>();
            AntdUI.Message.loading(form, LocalizeHelper.LOADING_PAGE, async (config) =>
            {
                BeginInvoke(async () =>
                {
                    // TestNo List
                    await InitializeSelectTestNoAsync();
                });
                // Scale List
                if (!InitializeSelectScale())
                {
                    // tell client how to do when any scale does not exit.
                    BtnSetScale_Click(null, null);
                    tcs.SetResult(true); // 这里需要根据业务决定是否设置结果
                    return;
                }
                try
                {
                    // 通过观察者模式实现界面数据效果
                    sortedIDs = blacknessMethodBLL.LoadOriginalResultFromDB(originalBlacknessResult, EDIT_METHOD_ID);
                    if (!string.IsNullOrEmpty(EDIT_METHOD_ID))
                    {
                        // The order is very important.:)
                        btnNext.Visible = true;
                        btnNext.Enabled = sortedIDs.Count > 0 && sortedIDs.FindIndex(t => t.ToString().Equals(EDIT_METHOD_ID)) != sortedIDs.Count - 1;
                        btnPrint.Visible = true;
                        btnPre.Visible = true;
                        btnPre.Enabled = sortedIDs.Count > 0 && sortedIDs.FindIndex(t => t.ToString().Equals(EDIT_METHOD_ID)) != 0;
                        AntdUI.Message.success(form, LocalizeHelper.BLACKNESS_EDIT_MODE(originalBlacknessResult));
                        return;
                    }
                    else
                    {
                        btnNext.Visible = false;
                        btnPrint.Visible = false;
                        btnPre.Visible = false;
                        AntdUI.Message.success(form, LocalizeHelper.NEW_MODE);
                    }
                    // when refresh、pre、next
                    cameraBLL.CloseDevice();
                    var result = cameraBLL.StartRendering();
                    switch (result)
                    {
                        case CameraBLLStatusKind.NoCamera:
                            AntdUI.Notification.warn(form, LocalizeHelper.PROMPT, LocalizeHelper.NO_CAMERA, AntdUI.TAlignFrom.BR, Font);
                            break;
                        case CameraBLLStatusKind.NoCameraSettings:
                            AntdUI.Notification.warn(form, LocalizeHelper.PROMPT, LocalizeHelper.NO_CAMERA_SETTING, AntdUI.TAlignFrom.BR, Font);
                            // 延迟1秒
                            await Task.Delay(1000).ConfigureAwait(true);
                            BtnCameraSetting_Click(null, null);
                            break;
                        case CameraBLLStatusKind.NoCameraOpen:
                            AntdUI.Notification.warn(form, LocalizeHelper.PROMPT, LocalizeHelper.NO_CAMERA_OPEN, AntdUI.TAlignFrom.BR, Font);
                            // 延迟1秒
                            await Task.Delay(1000).ConfigureAwait(true);
                            BtnCameraSetting_Click(null, null);
                            break;
                        case CameraBLLStatusKind.NoCameraGrabbing:
                            AntdUI.Notification.warn(form, LocalizeHelper.PROMPT, LocalizeHelper.NO_CAMERA_GRABBING, AntdUI.TAlignFrom.BR, Font);
                            // 延迟1秒
                            await Task.Delay(1000).ConfigureAwait(true);
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
                finally
                {
                    config.OK(LocalizeHelper.PAGE_LOADED_SUCCESS);
                    tcs.SetResult(true); // 标记整个异步操作完成
                }
            }, Font);
            await tcs.Task.ConfigureAwait(true); // 这里才是真正的等待点
            Console.WriteLine($"InitializeAsync took {sw.ElapsedMilliseconds}ms");
        }

        private void ScaleList_Changed(object sender, ObservableDictionary<string, CalculateScale>.ChangedEventArgs<string, CalculateScale> e)
        {
            selectScale.Items.Clear();
            selectScale.Items.AddRange(scaleList.Select(t => $"{FormatScaleItemName(t)}").ToArray());
            if (e.Action == ObservableDictionary<string, CalculateScale>.ChangedAction.Add)
            {
                Console.WriteLine($"Added: Key={e.Key}, Value={e.NewValue}");
                var text = FormatScaleItemName(new KeyValuePair<string, CalculateScale>(e.Key, e.NewValue));
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
                var text = FormatScaleItemName(new KeyValuePair<string, CalculateScale>(e.Key, e.NewValue));
                selectScale.SelectedValue = text;
                AntdUI.Message.success(form, $"{LocalizeHelper.SCALE_LOAD_SUCCESSED}{text}");
            }
        }

        private string FormatScaleItemName(KeyValuePair<string, CalculateScale> t)
        {
            var text = string.Empty;
            var (_, lengthScale, _, mpe, accuracy) = gaugeBlockMethodBLL.GetScaleParts(t.Value);
            switch (t.Key)
            {
                case "current":
                    text = $"{LocalizeHelper.CURRENT_SCALE_TITLE}[{lengthScale};{mpe};{accuracy}]";
                    break;
                case "atThatTime":
                    text = $"{LocalizeHelper.SCALE_TITLE_AT_THAT_TIME}[{lengthScale};{mpe};{accuracy}]";
                    break;
                default:
                    break;
            }
            return text;
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
                // selectTestNo.Items come less slowly than originalBlacknessResult.TestNo;
                selectTestNo.Text = originalBlacknessResult.TestNo;
            }
            else if (e.PropertyName == "CoilNumber")
            {
                inputCoilNumber.Text = originalBlacknessResult.CoilNumber;
            }
            else if (e.PropertyName == "Nth")
            {
                labelTestNo.Badge = originalBlacknessResult.Nth?.ToString();
                tempBlacknessResult.Nth = originalBlacknessResult.Nth;
            }
            else if (e.PropertyName == "Size")
            {
                inputSize.Text = originalBlacknessResult.Size;
            }
            else if (e.PropertyName == "Analyst")
            {
                inputAnalyst.Text = originalBlacknessResult.Analyst;
            }
            else if (e.PropertyName == "CalculateScale")
            {
                if (originalBlacknessResult.CalculateScale == null)
                {
                    selectScale.SelectedValue = null;
                    AntdUI.Notification.warn(form, LocalizeHelper.PROMPT, LocalizeHelper.LOST_SCALE, AntdUI.TAlignFrom.BR, Font);
                }
                else
                {
                    // different scale
                    if (!originalBlacknessResult.CalculateScale.Equals(scaleList.FirstOrDefault(t => "current".Equals(t.Key)).Value))
                    {
                        scaleList["atThatTime"] = originalBlacknessResult.CalculateScale;
                    }
                }
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
            if (selectScale.SelectedValue == null)
            {
                throw new Exception(LocalizeHelper.PLEASE_SELECT_SCALE);
            }
            if (tempBlacknessResult == null || tempBlacknessResult.Items.Count == 0)
            {
                throw new Exception(LocalizeHelper.PLEASE_PREDICT_BEFORE_SAVING);
            }
            // 比例尺不一致
            if (!tempBlacknessResult.CalculateScale.Equals(tempBlacknessResult.Items.FirstOrDefault()?.CalculateScale))
            {
                throw new Exception(LocalizeHelper.PLEASE_PREDICT_WITH_NEW_SCALE_BEFORE_SAVING);
            }
            if (selectWorkGroup.SelectedValue == null)
            {
                throw new Exception(LocalizeHelper.PLEASE_SELECT_WORKBENCH);
            }
            if (string.IsNullOrEmpty(selectTestNo.Text))
            {
                throw new Exception(LocalizeHelper.PLEASE_SELECT_OR_INPUT_TESTNO);
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
                    btnSetScale.Enabled = true;
                    checkboxRedefine.Enabled = true;
                    // for add scale firstly
                    if (scaleList.Count == 0)
                    {
                        BtnSetScale_Click(null, null);
                        return;
                    }
                    // call when edit
                    if (!originalBlacknessResult.OriginImagePath.Equals(tempBlacknessResult.OriginImagePath))
                    {
                        AntdUI.Message.success(form, LocalizeHelper.PREDICTED_SUCCESSFULLY);
                    }
                }
                else
                {
                    checkboxRedefine.Checked = false;
                    checkboxRedefine.Enabled = false;
                    ClearTexts();
                    if (!originalBlacknessResult.OriginImagePath.Equals(tempBlacknessResult.OriginImagePath))
                    {
                        AntdUI.Notification.warn(form, LocalizeHelper.PROMPT, LocalizeHelper.PLEASE_USE_CORRECT_BLACKNESS_IMAGE, AntdUI.TAlignFrom.BR, Font);
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
                    // cant predict automatically under the status of editing
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
            EDIT_METHOD_ID = string.Empty;
            await Task.Delay(50);
            cameraBLL.CloseDevice();
            CameraHelper.CAMERA_DEVICES.Remove(cameraBLL);
        }

        private bool InitializeSelectScale()
        {
            scaleList.Clear();
            var currentScaleFromDB = gaugeBlockMethodBLL.GetCurrentScale();
            if (currentScaleFromDB == null)
            {
                AntdUI.Notification.warn(form, LocalizeHelper.PROMPT, LocalizeHelper.PLEASE_SET_BLACKNESS_SCALE, AntdUI.TAlignFrom.BR, Font);
                return false;
            }
            scaleList["current"] = currentScaleFromDB;
            return true;
        }

        private async Task InitializeSelectTestNoAsync()
        {
            try
            {
                selectTestNo.Items.Clear();
                testNoList = await blacknessMethodBLL.GetTestNoListAsync();
                // select testNo
                var result = testNoList?.Select(t => t.TestNo).Distinct().OrderDescending().ToList();
                if (result != null && result.Count != 0)
                {
                    selectTestNo.Items.AddRange([.. result]);
                    AntdUI.Message.success(form, $"{LocalizeHelper.TESTNO_LIST_LOADED_SUCCESS(testNoList)}");
                }
                else
                {
                    selectTestNo.Items.AddRange(["test"]);
                    AntdUI.Notification.warn(form, LocalizeHelper.PROMPT, LocalizeHelper.ONLY_TEST_NO, AntdUI.TAlignFrom.BR, Font);
                }
                if (!string.IsNullOrEmpty(selectTestNo.Text))
                {
                    var target = testNoList?.FirstOrDefault(t => selectTestNo.Text.Equals(t.TestNo));
                    if (target == null)
                    {
                        inputCoilNumber.ReadOnly = false;
                        AntdUI.Notification.warn(form, LocalizeHelper.PROMPT, LocalizeHelper.TESTNO_NOT_IN_THE_LIST(selectTestNo.Text), AntdUI.TAlignFrom.BR, Font);
                    }
                    else
                    {
                        inputCoilNumber.ReadOnly = true;
                    }
                    selectTestNo.SelectedValue = target?.TestNo;
                }
            }
            catch (Exception error)
            {
                AntdUI.Notification.error(form, LocalizeHelper.ERROR, error.Message, AntdUI.TAlignFrom.BR, Font);
            }
        }

        private void AvatarOriginImage_Click(object sender, System.EventArgs e)
        {
            AntdUI.Preview.open(new AntdUI.Preview.Config(form, avatarOriginImage.Image)
            {
                Btns = [new AntdUI.Preview.Btn("download", Properties.Resources.btn_download)],
                OnBtns = (id, config) =>
                {
                    switch (id)
                    {
                        case "download":
                            var testNo = string.IsNullOrEmpty(tempBlacknessResult.TestNo) ? string.Empty : $"{tempBlacknessResult.TestNo}_";
                            // 弹出文件保存对话框
                            SaveFileDialog saveFileDialog = new()
                            {
                                //Filter = "JPEG Image Files|*.jpg;*.jpeg|All Files|*.*",
                                //DefaultExt = "jpg",
                                FileName = $"{testNo}黑度检测_原图_{DateTime.Now:yyyyMMddHHmmssfff}.jpg",
                                Title = LocalizeHelper.CHOOSE_THE_LOCATION
                            };
                            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                            {
                                string directoryPath = saveFileDialog.FileName;
                                avatarOriginImage.Image.Save(directoryPath, ImageFormat.Jpeg);
                                AntdUI.Message.success(form, LocalizeHelper.FILE_SAVED_LOCATION + directoryPath);
                            }
                            break;
                    }
                }
            });
        }

        private void AvatarRenderImage_Click(object sender, EventArgs e)
        {
            AntdUI.Preview.open(new AntdUI.Preview.Config(form, avatarRenderImage.Image)
            {
                Btns = [new AntdUI.Preview.Btn("download", Properties.Resources.btn_download)],
                OnBtns = (id, config) =>
                {
                    switch (id)
                    {
                        case "download":
                            var testNo = string.IsNullOrEmpty(tempBlacknessResult.TestNo) ? string.Empty : $"{tempBlacknessResult.TestNo}_";
                            // 弹出文件保存对话框
                            SaveFileDialog saveFileDialog = new()
                            {
                                //Filter = "JPEG Image Files|*.jpg;*.jpeg|All Files|*.*",
                                //DefaultExt = "jpg",
                                FileName = $"{testNo}黑度检测_渲染图_{DateTime.Now:yyyyMMddHHmmssfff}.jpg",
                                Title = LocalizeHelper.CHOOSE_THE_LOCATION
                            };
                            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                            {
                                string directoryPath = saveFileDialog.FileName;
                                avatarRenderImage.Image.Save(directoryPath, ImageFormat.Jpeg);
                                AntdUI.Message.success(form, LocalizeHelper.FILE_SAVED_LOCATION + directoryPath);
                            }
                            break;
                    }
                }
            });
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
                blacknessPredict.Predict(CurrentScale);
            }, () =>
            {
                if (btn.IsDisposed) return;
                btn.Loading = false;
            });
        }

        private CalculateScale CurrentScale
        {
            get => checkboxRedefine.Checked || selectScale.SelectedValue == null ? null : scaleList.FirstOrDefault(t => selectScale.SelectedValue.Equals(FormatScaleItemName(t))).Value;
        }

        private void OutputTexts()
        {
            tempBlacknessResult.Items = [new(BlacknessLocationKind.SURFACE_OP, blacknessPredict.Predictions[0], CurrentScale),
                            new(BlacknessLocationKind.SURFACE_CE, blacknessPredict.Predictions[1], CurrentScale),
                            new(BlacknessLocationKind.SURFACE_DR, blacknessPredict.Predictions[2],CurrentScale),
                            new(BlacknessLocationKind.INSIDE_OP, blacknessPredict.Predictions[3],CurrentScale),
                            new(BlacknessLocationKind.INSIDE_CE, blacknessPredict.Predictions[4], CurrentScale),
                            new(BlacknessLocationKind.INSIDE_DR, blacknessPredict.Predictions[5], CurrentScale)];
            inputSurfaceOP.Text = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_OP))?.Description;
            inputSurfaceCE.Text = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_CE))?.Description;
            inputSurfaceDR.Text = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_DR))?.Description;
            inputInsideOP.Text = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_OP))?.Description;
            inputInsideCE.Text = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_CE))?.Description;
            inputInsideDR.Text = tempBlacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_DR))?.Description;
            radioResultOK.Checked = tempBlacknessResult.Items.All(t => new List<string> { "3", "4", "5" }.Contains(t.Level)); // [1，2] not exit，otherwise，NG is checked
            radioResultNG.Checked = !radioResultOK.Checked;
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
            if (AntdUI.Modal.open(form, LocalizeHelper.CONFIRM, originalBlacknessResult.IsUploaded ?
                LocalizeHelper.WOULD_RESAVE_BLACKNESS_RESULT_AFTER_UPLOADED :
                LocalizeHelper.WOULD_SAVE_BLACKNESS_RESULT) == DialogResult.OK)
            {
                AntdUI.Button btn = (AntdUI.Button)sender;
                btn.LoadingWaveValue = 0;
                btn.Loading = true;
                AntdUI.ITask.Run(async () =>
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
                            AntdUI.Message.success(form, LocalizeHelper.SAVE_SUCCESSFULLY);
                            EDIT_METHOD_ID = result.ToString();
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
            tempBlacknessResult.WorkGroup = selectWorkGroup.SelectedValue?.ToString();
        }

        private void SelectTestNo_SelectedIndexChanged(object sender, AntdUI.IntEventArgs e)
        {
            tempBlacknessResult.TestNo = selectTestNo.SelectedValue?.ToString();
            UpdateCoilNumberInput();
        }

        private void SelectTestNo_TextChanged(object sender, EventArgs e)
        {
            tempBlacknessResult.TestNo = selectTestNo.Text;
            UpdateCoilNumberInput();
            Invoke(async () =>
            {
                inputSize.Text = await blacknessMethodBLL.GetSizeOfCoilAsync(tempBlacknessResult.TestNo);
            });
        }

        private void SelectTestNo_Leave(object sender, EventArgs e)
        {
            UpdateNth();
        }

        /// <summary>
        /// 统一更新钢卷号输入框的状态
        /// </summary>
        private void UpdateCoilNumberInput()
        {
            var hasValidTestNo = !string.IsNullOrEmpty(tempBlacknessResult.TestNo);
            var targetItem = hasValidTestNo && testNoList != null
                ? testNoList.FirstOrDefault(t => t.TestNo == tempBlacknessResult.TestNo)
                : null;

            var coilNumber = targetItem?.CoilNumber;

            inputCoilNumber.Text = !string.IsNullOrEmpty(coilNumber)
                ? coilNumber
                : tempBlacknessResult.CoilNumber;

            // 设置只读状态：当找到有效数据且自动填充内容时锁定输入
            inputCoilNumber.ReadOnly = targetItem != null && !string.IsNullOrEmpty(coilNumber);
        }

        private void InputCoilNumber_TextChanged(object sender, EventArgs e)
        {
            tempBlacknessResult.CoilNumber = inputCoilNumber.Text;
        }

        private void InputCoilNumber_Leave(object sender, EventArgs e)
        {
            UpdateNth();
        }

        private void UpdateNth()
        {
            if (!string.IsNullOrEmpty(tempBlacknessResult.TestNo) && !string.IsNullOrEmpty(tempBlacknessResult.CoilNumber))
            {
                var result = blacknessMethodBLL.GetNthOfMethod(tempBlacknessResult.TestNo, tempBlacknessResult.CoilNumber, originalBlacknessResult.Id);
                tempBlacknessResult.Nth = result;
                labelTestNo.Badge = result.ToString();
                AntdUI.Message.success(form, LocalizeHelper.BLACKNESS_EDITING_NTH(tempBlacknessResult));
            }
            else
            {
                tempBlacknessResult.Nth = null;
                labelTestNo.Badge = null;
            }
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
            ConfirmUnderUnsaved(async () =>
            {
                btnClear.Loading = true;
                EDIT_METHOD_ID = string.Empty;
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
                }
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
            ConfirmUnderUnsaved(async () =>
            {
                btnPre.Loading = true;
                var index = sortedIDs.FindIndex(t => t.ToString().Equals(EDIT_METHOD_ID));
                if (index != 0)
                {
                    EDIT_METHOD_ID = (sortedIDs[index - 1]).ToString();
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
                var index = sortedIDs.FindIndex(t => t.ToString().Equals(EDIT_METHOD_ID));
                if (index != sortedIDs.Count)
                {
                    EDIT_METHOD_ID = (sortedIDs[index + 1]).ToString();
                    await InitializeAsync();
                }
                btnNext.Loading = false;
            }, LocalizeHelper.NEXT_RECORD_CONFIRM_WHEN_SOMETHING_IS_UNDONE);

        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                AntdUI.Drawer.open(form, new BlacknessReport(form, EDIT_METHOD_ID, () => { })
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
            // for the first time or want to redefine the scale.
            if (scaleList.Count == 0 || checkboxRedefine.Checked)
            {
                // go to the gauge block method page
                if (AntdUI.Modal.open(form, LocalizeHelper.CONFIRM, scaleList.Count == 0 ? $"{LocalizeHelper.SETTING_SCALE_FIRSTLY_BEFORE_PREDICTING}{LocalizeHelper.JUMP_TO_GAUGE_BLOCK_METHOD}" : $"{LocalizeHelper.RESETTING_SCALE_BEFORE_PREDICTING}{LocalizeHelper.JUMP_TO_GAUGE_BLOCK_METHOD}") == DialogResult.OK)
                {
                    try
                    {
                        BeginInvoke(new Action(() =>
                        {
                            form.OpenPage("Scale Setting");
                        }));
                    }
                    catch (Exception error)
                    {
                        AntdUI.Notification.error(form, LocalizeHelper.ERROR, error.Message, AntdUI.TAlignFrom.BR, Font);
                    }
                }
                else
                {
                    checkboxRedefine.Checked = false;
                }
                return;
            }
            var setting = new GaugeScaleSetting(form);
            setting.SetScaleDetails(null, checkboxRedefine.Checked, scaleList.FirstOrDefault(t => "atThatTime".Equals(t.Key)).Value);
            var result = AntdUI.Modal.open(new AntdUI.Modal.Config(form, LocalizeHelper.BLACKNESS_SCALE_MODAL_TITLE, setting)
            {
                OnButtonStyle = (id, btn) =>
                {
                    btn.BackExtend = "135, #6253E1, #04BEFE";
                },
                CancelText = null,
                OkText = LocalizeHelper.CONFIRM
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
                tempBlacknessResult.CalculateScale = scaleList.FirstOrDefault(t => selectScale.SelectedValue.Equals(FormatScaleItemName(t))).Value;
            }
            else
            {
                tempBlacknessResult.CalculateScale = null;
            }
        }

        private void CheckboxRedefine_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            if (checkboxRedefine.Checked && !string.IsNullOrEmpty(tempBlacknessResult.RenderImagePath) && tempBlacknessResult.Items != null && tempBlacknessResult.Items.Count != 0)
            {
                BtnSetScale_Click(null, null);
            }
        }
    }
}