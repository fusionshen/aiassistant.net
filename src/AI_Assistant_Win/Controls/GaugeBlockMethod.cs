﻿using AI_Assistant_Win.Business;
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
    public partial class GaugeBlockMethod : UserControl, IAsyncInit
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
            // this.Load += async (s, e) => await InitializeAsync();
            this.HandleDestroyed += async (s, e) => await DestoryAsync();
        }

        public async Task InitializeAsync()
        {
            var sw = Stopwatch.StartNew();
            // 使用TaskCompletionSource创建可等待的Task
            var tcs = new TaskCompletionSource<bool>();
            AntdUI.Message.loading(form, LocalizeHelper.LOADING_PAGE, async (config) =>
            {
                // Scale List
                if (!InitializeSelectScale())
                {
                    // tell client how to do when any scale does not exit.
                    BtnSetScale_Click(null, null);
                }
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
                        AntdUI.Message.success(form, LocalizeHelper.GAUGE_BLOCK_EDIT_MODE(originalGaugeBlockResult));
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
            var (topGraduations, lengthScale, _, mpe, accuracy) = gaugeBlockMethodBLL.GetScaleParts(t.Value);
            switch (t.Key)
            {
                case "current":
                    text = $"{LocalizeHelper.CURRENT_SCALE_TITLE}[{topGraduations};{lengthScale};{mpe};{accuracy}]";
                    break;
                case "atThatTime":
                    text = $"{LocalizeHelper.SCALE_TITLE_AT_THAT_TIME}[{topGraduations};{lengthScale};{mpe};{accuracy}]";
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
            else if (e.PropertyName == "InputEdge")
            {
                selectEdge.SelectedValue = originalGaugeBlockResult.InputEdge;
            }
            else if (e.PropertyName == "InputEdgeLength")
            {
                inputEdgeLength.Text = originalGaugeBlockResult.InputEdgeLength;
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
            if (selectEdge.SelectedValue == null)
            {
                throw new Exception(LocalizeHelper.PLEASE_SELECT_EDGE);
            }
            if (string.IsNullOrEmpty(inputEdgeLength.Text))
            {
                throw new Exception(LocalizeHelper.PLEASE_INPUT_CORRECT_EDGE_LENGTH);
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
                        AntdUI.Notification.warn(form, LocalizeHelper.PROMPT, LocalizeHelper.PLEASE_USE_CORRECT_GAUGE_IMAGE, AntdUI.TAlignFrom.BR, Font);
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
                        BtnPredict_ClickAsync(btnPredict, null);  // auto predict
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
                            // 弹出文件保存对话框
                            SaveFileDialog saveFileDialog = new()
                            {
                                //Filter = "JPEG Image Files|*.jpg;*.jpeg|All Files|*.*",
                                //DefaultExt = "jpg",
                                FileName = $"比例尺设置_原图_{DateTime.Now:yyyyMMddHHmmssfff}.jpg",
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
                            // 弹出文件保存对话框
                            SaveFileDialog saveFileDialog = new()
                            {
                                //Filter = "JPEG Image Files|*.jpg;*.jpeg|All Files|*.*",
                                //DefaultExt = "jpg",
                                FileName = $"比例尺设置_渲染图_{DateTime.Now:yyyyMMddHHmmssfff}.jpg",
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
            if (gaugeMethod_OpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                AntdUI.Button btn = (AntdUI.Button)sender;
                btn.LoadingWaveValue = 0;
                btn.Loading = true;
                AntdUI.ITask.Run(() =>
                {
                    imageProcessBLL.SaveOriginImage(gaugeMethod_OpenFileDialog.FileName);
                    // stop realtime image render
                    cameraBLL.StopGrabbing();
                    AntdUI.Message.success(form, LocalizeHelper.UPLOAD_ORIGINAL_IMAGE_SUCCESSFULLY);
                    if (btn.IsDisposed) return;
                    btn.Loading = false;
                });
            }
        }

        private async void BtnPredict_ClickAsync(object sender, EventArgs e)
        {
            await ExecutePredictAsync();
        }

        private async Task ExecutePredictAsync()
        {
            AntdUI.Button btn = btnPredict;
            btn.LoadingWaveValue = 0;
            btn.Loading = true;

            // 创建 TaskCompletionSource 用于异步等待
            var tcs = new TaskCompletionSource<bool>();

            await AntdUI.ITask.Run(() =>
            {
                gaugeBlockPredict.Predict(CurrentScale);
            }, () =>
            {
                if (btn.IsDisposed)
                {
                    tcs.TrySetCanceled();
                    return;
                }
                btn.Loading = false;
                tcs.TrySetResult(true); // 标记任务完成
            });
            try
            {
                await tcs.Task;
            }
            catch (TaskCanceledException)
            {
                // 页面关闭时任务被取消，忽略异常
            }
        }

        private CalculateScale CurrentScale
        {
            get => checkboxRedefine.Checked || selectScale.SelectedValue == null ? null : scaleList.FirstOrDefault(t => selectScale.SelectedValue.Equals(FormatScaleItemName(t))).Value;
        }

        private void OutputTexts()
        {
            tempGaugeBlockResult.Item = new GaugeBlock(gaugeBlockPredict.Prediction, CurrentScale);
            inputAreaOfPixels.Text = $"{tempGaugeBlockResult.Item.AreaOfPixels}";
            inputConfidence.Text = $"{tempGaugeBlockResult.Item.Confidence:P2}";
            OutputScaleTexts(tempGaugeBlockResult.Item.CalculateScale);
            inputCalculatedArea.Text = $"{tempGaugeBlockResult.Item.CalculatedArea:F4}{tempGaugeBlockResult.Item.AreaUnit}";
            inputVertexPositions.Text = tempGaugeBlockResult.Item.VertexPositonsText;
            inputEdgePixels.Text = string.Join(" ", tempGaugeBlockResult.Item.EdgePixels.Select(t => $"{t.Key}={t.Value:F2}"));
            inputCalculatdEdges.Text = string.Join(" ", tempGaugeBlockResult.Item.CalculatedEdgeLengths.Select(t => $"{t.Key}={t.Value:F2}{tempGaugeBlockResult.Item.LengthUnit}"));
        }

        private void OutputScaleTexts(CalculateScale calculateScale)
        {
            if (calculateScale != null)
            {
                var (topGraduations, lengthScale, areaScale, mpe, accuracy) = gaugeBlockMethodBLL.GetScaleParts(calculateScale);
                inputLengthScale.Text = $"{lengthScale};{mpe};{accuracy}";
                inputAreaScale.Text = $"{areaScale}";
                inputGrade.Text = topGraduations.Split(":")[1];
            }
            else
            {
                inputLengthScale.Text = string.Empty;
                inputAreaScale.Text = string.Empty;
                inputGrade.Text = string.Empty;
            }
        }

        private void ClearTexts()
        {
            tempGaugeBlockResult.Item = null;
            inputGrade.Text = string.Empty;
            inputConfidence.Text = string.Empty;
            inputAreaOfPixels.Text = string.Empty;
            inputAreaScale.Text = string.Empty;
            inputCalculatedArea.Text = string.Empty;
            inputVertexPositions.Text = string.Empty;
            inputEdgePixels.Text = string.Empty;
            inputLengthScale.Text = string.Empty;
            inputCalculatdEdges.Text = string.Empty;
            selectEdge.SelectedValue = null;
            inputEdgeLength.Text = string.Empty;
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
                LocalizeHelper.WOULD_RESAVE_SCALE_PRECISION_RESULT_AFTER_UPLOADING :
                gaugeBlockMethodBLL.GetResultExitsInDB(tempGaugeBlockResult) != null ?
                  LocalizeHelper.WOULD_RESAVE_SCALE_PRECISION_RESULT_ON_THIS_GRADE(inputGrade.Text) :
                LocalizeHelper.WOULD_SAVE_SCALE_PRECISION_RESULT) == DialogResult.OK)
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
            tempGaugeBlockResult.WorkGroup = selectWorkGroup.SelectedValue?.ToString();
        }

        private void InputAnalyst_TextChanged(object sender, EventArgs e)
        {
            tempGaugeBlockResult.Analyst = inputAnalyst.Text;
        }

        private void SelectEdge_SelectedIndexChanged(object sender, AntdUI.IntEventArgs e)
        {

            tempGaugeBlockResult.InputEdge = selectEdge.SelectedValue?.ToString();
            DisplayAccuracy();
        }

        private void InputEdgeLength_TextChanged(object sender, EventArgs e)
        {
            tempGaugeBlockResult.InputEdgeLength = inputEdgeLength.Text;
            DisplayAccuracy();
        }

        private void DisplayAccuracy()
        {
            if (tempGaugeBlockResult.Item != null &&
                tempGaugeBlockResult.Item.CalculateScale != null &&
                !string.IsNullOrEmpty(tempGaugeBlockResult.InputEdge) &&
                !string.IsNullOrEmpty(tempGaugeBlockResult.InputEdgeLength))
            {
                try
                {
                    // length Accuracy
                    var calculatedLength = tempGaugeBlockResult.Item.CalculatedEdgeLengths[tempGaugeBlockResult.InputEdge];
                    var realLenght = float.Parse(tempGaugeBlockResult.InputEdgeLength);
                    if (realLenght <= 0)
                    {
                        inputLenghtAccuracy.Text = string.Empty;
                        inputAreaAccuracy.Text = string.Empty;
                        return;
                    }
                    inputLenghtAccuracy.Text = $"{1 - Math.Abs(calculatedLength - realLenght) / Math.Abs(realLenght):P2}";
                    // area Accuracy
                    var calculatedArea = tempGaugeBlockResult.Item.CalculatedArea;
                    var realArea = Math.Pow(realLenght, 2) * calculatedArea / Math.Pow(calculatedLength, 2);
                    inputAreaAccuracy.Text = $"{realArea:F4}{LocalizeHelper.SQUARE_MILLIMETER} {1 - Math.Abs(calculatedArea - realArea) / Math.Abs(realArea):P2}";
                }
                catch (Exception)
                {
                    inputLenghtAccuracy.Text = string.Empty;
                    inputAreaAccuracy.Text = string.Empty;
                }
            }
            else
            {
                inputLenghtAccuracy.Text = string.Empty;
                inputAreaAccuracy.Text = string.Empty;
            }
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
            if (AntdUI.Modal.open(form, LocalizeHelper.CONFIRM, LocalizeHelper.JUMP_TO_ACCURACY_TRACER) == DialogResult.OK)
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
                var history = gaugeBlockMethodBLL.GetTracerHistoryByMethodId(originalGaugeBlockResult.Id.ToString());
                AntdUI.Drawer.open(form, new ScaleAccuracyReport(form, history.Tracer.Id.ToString(), () => { })
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
            setting.SetScaleDetails(tempGaugeBlockResult, checkboxRedefine.Checked, scaleList.FirstOrDefault(t => "atThatTime".Equals(t.Key)).Value);
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
            checkboxRedefine.Checked = false;
        }

        private void SelectScale_SelectedIndexChanged(object sender, AntdUI.IntEventArgs e)
        {
            if (selectScale.SelectedValue != null)
            {
                var selected = scaleList.FirstOrDefault(t => selectScale.SelectedValue.Equals(FormatScaleItemName(t))).Value;
                tempGaugeBlockResult.CalculateScale = selected;
            }
            else
            {
                tempGaugeBlockResult.CalculateScale = null;
            }
        }

        private async void CheckboxRedefine_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            // 取消比例尺重新识别一次，避免使用已经依靠比例尺识别过一次的图片作为比例尺图片
            if (checkboxRedefine.Checked && !string.IsNullOrEmpty(tempGaugeBlockResult.OriginImagePath))
            {
                await ExecutePredictAsync();  // auto predict
                BtnSetScale_Click(null, null);
            }
        }
    }
}