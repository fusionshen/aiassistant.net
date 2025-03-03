using AI_Assistant_Win.Business;
using AI_Assistant_Win.Models;
using AI_Assistant_Win.Models.Enums;
using AI_Assistant_Win.Models.Middle;
using AI_Assistant_Win.Models.Response;
using AI_Assistant_Win.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using YoloDotNet.Extensions;

namespace AI_Assistant_Win.Controls
{
    public partial class CircularAreaMethod : UserControl, IAsyncInit
    {
        public static string EDIT_ITEM_ID = string.Empty;

        private readonly MainWindow form;

        private readonly CircularAreaMethodBLL circularAreaMethodBLL;

        private readonly GaugeBlockMethodBLL gaugeBlockMethodBLL;

        private readonly ImageProcessBLL imageProcessBLL;

        private readonly CameraBLL cameraBLL;

        private readonly CircularAreaPredict circularAreaPredict;

        private readonly CircularAreaResult originalCircularAreaResult;

        private readonly CircularAreaResult tempCircularAreaResult;

        private List<int> sortedIDs = [];

        private readonly ObservableDictionary<string, CalculateScale> scaleList = [];

        private List<GetTestNoListResponse> testNoList;

        public CircularAreaMethod(MainWindow _form)
        {
            form = _form;
            InitializeComponent();
            circularAreaMethodBLL = new CircularAreaMethodBLL();
            gaugeBlockMethodBLL = new GaugeBlockMethodBLL();
            imageProcessBLL = new ImageProcessBLL("CircularArea");
            imageProcessBLL.PropertyChanged += ImageProcessBLL_PropertyChanged;
            circularAreaPredict = new CircularAreaPredict(imageProcessBLL);
            circularAreaPredict.PropertyChanged += CircularAreaPredict_PropertyChanged;
            cameraBLL = new CameraBLL("CircularArea", avatarOriginImage.Handle);
            cameraBLL.PropertyChanged += CameraBLL_PropertyChanged;
            CameraHelper.CAMERA_DEVICES.Add(cameraBLL);
            originalCircularAreaResult = new CircularAreaResult();
            originalCircularAreaResult.PropertyChanged += OriginalCircularAreaResult_PropertyChanged;
            tempCircularAreaResult = new CircularAreaResult();
            tempCircularAreaResult.PropertyChanged += TempCircularAreaResult_PropertyChanged;
            scaleList.Changed += ScaleList_Changed;
            this.HandleDestroyed += async (s, e) => await DestoryAsync();
        }

        public async Task InitializeAsync()
        {
            var sw = Stopwatch.StartNew();
            // 使用TaskCompletionSource创建可等待的Task
            var tcs = new TaskCompletionSource<bool>();
            // 用户提供的`InitializeAsync`方法内部使用了`AntdUI.Message.loading`，并传入了一个异步委托。
            // 这里可能存在一个问题：`AntdUI.Message.loading`方法可能不会返回一个可以等待的任务，或者用户传入的异步委托没有被正确等待。
            // 此外，整个`Message.loading`的调用后面跟着一个`await Task.Delay(50);`，这可能不足以等待所有内部操作完成。
            // 问题可能出在`AntdUI.Message.loading`方法是否支持异步操作。
            // 如果这个方法启动了一个异步操作但没有返回一个`Task`，那么`await InitializeAsync()`实际上并不会等待这个异步操作完成，因为`Message.loading`可能没有正确关联到返回的`Task`。
            // 此外，`Message.loading`内部的异步委托可能没有被正确等待，导致整个方法提前完成。
            // 可能的解决方案包括：
            // 1. * *确保`Message.loading`返回一个可以等待的`Task`**：需要检查`AntdUI.Message.loading`的返回值是否是`Task`或类似的可等待对象。如果该方法没有返回`Task`，那么内部的异步操作不会被`await`捕获，导致方法提前完成。
            // 2. * *使用`TaskCompletionSource`来手动控制完成信号 * *：如果`Message.loading`没有提供返回`Task`的方式，可以创建一个`TaskCompletionSource`，在异步委托完成时设置结果，然后在`InitializeAsync`中等待这个任务。
            // 3. * *调整`Message.loading`的调用方式 * *：可能需要将内部的异步操作移到外部，或者确保所有异步操作都被正确等待。
            // 此外，用户代码中在`Message.loading`的回调中使用了`config.OK`，这可能在设置完成状态，但不确定这是否会影响异步流程。
            // 另一个需要注意的地方是，`Message.loading`可能是在UI线程上运行，而其中的异步操作如果没有正确配置上下文，可能导致某些代码在错误的线程上执行，从而引发问题。不过这可能不是当前问题的直接原因，但也是需要考虑的因素。
            // 现在，我需要将这些思考整理成一个解决方案，并给出具体的代码修改建议，确保`InitializeAsync`方法正确等待所有内部异步操作完成，从而使得调用`await InitializeAsync()`后的代码在正确的时间执行。
            AntdUI.Message.loading(form, LocalizeHelper.LOADING_PAGE, async (config) =>
            {
                try
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
                    // Position List
                    InitializeSelectPostion();
                    // 通过观察者模式实现界面数据效果
                    sortedIDs = circularAreaMethodBLL.LoadOriginalResultFromDB(originalCircularAreaResult, EDIT_ITEM_ID);
                    if (!string.IsNullOrEmpty(EDIT_ITEM_ID))
                    {
                        // The order is very important.:)
                        btnNext.Visible = true;
                        btnNext.Enabled = sortedIDs.Count > 0 && sortedIDs.FindIndex(t => t.ToString().Equals(EDIT_ITEM_ID)) != sortedIDs.Count - 1;
                        btnPrint.Visible = true;
                        btnPre.Visible = true;
                        btnPre.Enabled = sortedIDs.Count > 0 && sortedIDs.FindIndex(t => t.ToString().Equals(EDIT_ITEM_ID)) != 0;
                        AntdUI.Message.success(form, LocalizeHelper.CIRCULAR_AREA_EDIT_MODE(originalCircularAreaResult));
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
            var (topGraduations, _, areaScale, _, _) = gaugeBlockMethodBLL.GetScaleParts(t.Value);
            switch (t.Key)
            {
                case "current":
                    text = $"{LocalizeHelper.CURRENT_SCALE_TITLE}[{topGraduations};{areaScale}]";
                    break;
                case "atThatTime":
                    text = $"{LocalizeHelper.SCALE_TITLE_AT_THAT_TIME}[{topGraduations};{areaScale}]";
                    break;
                default:
                    break;
            }
            return text;
        }

        private void OriginalCircularAreaResult_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Id")
            {
                tempCircularAreaResult.Id = originalCircularAreaResult.Id;
            }
            else if (e.PropertyName == "TestNo")
            {
                // selectTestNo.Items come slowerly than originalCircularAreaResult.TestNo;
                selectTestNo.Text = originalCircularAreaResult.TestNo;
            }
            else if (e.PropertyName == "CoilNumber")
            {
                inputCoilNumber.Text = originalCircularAreaResult.CoilNumber;
            }
            else if (e.PropertyName == "Position")
            {
                selectPosition.SelectedValue = originalCircularAreaResult.Position;
            }
            else if (e.PropertyName == "OriginImagePath")
            {
                imageProcessBLL.OriginImagePath = originalCircularAreaResult.OriginImagePath;
            }
            else if (e.PropertyName == "RenderImagePath")
            {
                imageProcessBLL.RenderImagePath = originalCircularAreaResult.RenderImagePath;
            }
            else if (e.PropertyName == "WorkGroup")
            {
                selectWorkGroup.SelectedValue = originalCircularAreaResult.WorkGroup;
            }
            else if (e.PropertyName == "Analyst")
            {
                inputAnalyst.Text = originalCircularAreaResult.Analyst;
            }
            else if (e.PropertyName == "CalculateScale")
            {
                if (originalCircularAreaResult.CalculateScale == null)
                {
                    selectScale.SelectedValue = null;
                    AntdUI.Notification.warn(form, LocalizeHelper.PROMPT, LocalizeHelper.LOST_SCALE, AntdUI.TAlignFrom.BR, Font);
                }
                else
                {
                    // different scale
                    if (!originalCircularAreaResult.CalculateScale.Equals(scaleList.FirstOrDefault(t => "current".Equals(t.Key)).Value))
                    {
                        scaleList["atThatTime"] = originalCircularAreaResult.CalculateScale;
                    }
                }
            }
            else if (e.PropertyName == "Item")
            {
                // 观察者模式虽好，但是跳来跳去，不好好考虑，真的容易多写代码
                circularAreaPredict.Prediction = originalCircularAreaResult.Item?.Prediction;
            }
        }

        private void TempCircularAreaResult_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!originalCircularAreaResult.Equals(tempCircularAreaResult))
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
            if (tempCircularAreaResult == null || tempCircularAreaResult.Item == null)
            {
                throw new Exception(LocalizeHelper.PLEASE_PREDICT_BEFORE_SAVING);
            }
            // 比例尺不一致
            if (!tempCircularAreaResult.CalculateScale.Equals(tempCircularAreaResult.Item.CalculateScale))
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
            if (selectPosition.SelectedValue == null)
            {
                throw new Exception(LocalizeHelper.PLEASE_SELECT_POSITION);
            }
            if (string.IsNullOrEmpty(inputAnalyst.Text))
            {
                throw new Exception(LocalizeHelper.PLEASE_INPUT_ANALYST);
            }
        }

        private void CircularAreaPredict_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Prediction")
            {
                if (circularAreaPredict.Prediction != null)
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
                    if (!originalCircularAreaResult.OriginImagePath.Equals(tempCircularAreaResult.OriginImagePath))
                    {
                        AntdUI.Message.success(form, LocalizeHelper.PREDICTED_SUCCESSFULLY);
                    }
                }
                else
                {
                    checkboxRedefine.Checked = false;
                    checkboxRedefine.Enabled = false;
                    ClearTexts();
                    if (!originalCircularAreaResult.OriginImagePath.Equals(tempCircularAreaResult.OriginImagePath))
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
                tempCircularAreaResult.OriginImagePath = imageProcessBLL.OriginImagePath;
                btnPredict.Enabled = !string.IsNullOrEmpty(imageProcessBLL.OriginImagePath);
                if (!string.IsNullOrEmpty(imageProcessBLL.OriginImagePath))
                {
                    avatarOriginImage.Image = Image.FromFile(imageProcessBLL.OriginImagePath);
                    // cant predict automatically under the status of editing
                    if (!originalCircularAreaResult.OriginImagePath.Equals(tempCircularAreaResult.OriginImagePath))
                    {
                        BtnPredict_Click(btnPredict, null);  // auto predict
                    }
                }
                else
                {
                    avatarOriginImage.Image = Properties.Resources.area_template;
                }
            }
            else if (e.PropertyName == "RenderImagePath")
            {
                tempCircularAreaResult.RenderImagePath = imageProcessBLL.RenderImagePath;
                if (!string.IsNullOrEmpty(imageProcessBLL.RenderImagePath))
                {
                    avatarRenderImage.Image = Image.FromFile(imageProcessBLL.RenderImagePath);
                }
                else
                {
                    avatarRenderImage.Image = Properties.Resources.area_template;
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

        private void InitializeSelectPostion()
        {

            selectPosition.Items.Clear();
            // select position
            var result = circularAreaMethodBLL.PositionList.Select(t => t.Value).ToList();
            selectPosition.Items.AddRange([.. result]);
        }

        private bool InitializeSelectScale()
        {
            scaleList.Clear();
            var currentScaleFromDB = gaugeBlockMethodBLL.GetCurrentScale();
            if (currentScaleFromDB == null)
            {
                AntdUI.Notification.warn(form, LocalizeHelper.PROMPT, LocalizeHelper.PLEASE_SET_CIRCULAR_AREA_SCALE, AntdUI.TAlignFrom.BR, Font);
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
                testNoList = await circularAreaMethodBLL.GetTestNoListAsync();
                // select testNo
                var result = testNoList?.Select(t => t.TestNo).Distinct().OrderDescending().ToList();
                if (result != null && result.Count != 0)
                {
                    selectTestNo.Items.AddRange([.. result]);
                    AntdUI.Message.success(form, $"{LocalizeHelper.TESTNO_LIST_LOADED_SUCCESS(testNoList.Count)}");
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
                    selectTestNo.SelectedValue = target;
                }
            }
            catch (Exception error)
            {
                AntdUI.Notification.error(form, LocalizeHelper.ERROR, error.Message, AntdUI.TAlignFrom.BR, Font);
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
                circularAreaPredict.Predict(CurrentScale);
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
            tempCircularAreaResult.Item = new(circularAreaPredict.Prediction, CurrentScale);
            inputAreaOfPixels.Text = $"{tempCircularAreaResult.Item.AreaOfPixels}{LocalizeHelper.AREA_OF_PIXELS}";
            inputConfidence.Text = $"{tempCircularAreaResult.Item.Confidence.ToPercent()}%";
            OutputScaleTexts(tempCircularAreaResult.Item.CalculateScale);
            inputCalculatedArea.Text = $"{tempCircularAreaResult.Item.CalculatedArea:F2}{tempCircularAreaResult.Item.Unit}";
            inputDiameter.Text = $"{tempCircularAreaResult.Item.Diameter:F2}{tempCircularAreaResult.Item.DiameterUnit}";
        }

        private void OutputScaleTexts(CalculateScale calculateScale)
        {
            if (calculateScale != null)
            {
                var (topGraduations, _, areaScale, _, _) = gaugeBlockMethodBLL.GetScaleParts(calculateScale);
                inputScale.Text = $"{areaScale}";
                inputGrade.Text = topGraduations.Split(":")[1];
            }
            else
            {
                inputScale.Text = string.Empty;
                inputGrade.Text = string.Empty;
            }
        }

        private void ClearTexts()
        {
            tempCircularAreaResult.Item = null;
            inputAreaOfPixels.Text = string.Empty;
            inputConfidence.Text = string.Empty;
            inputScale.Text = string.Empty;
            inputGrade.Text = string.Empty;
            inputCalculatedArea.Text = string.Empty;
            inputDiameter.Text = string.Empty;
        }

        /// <summary>
        /// 首先，我需要回忆一下C#中async/await的工作机制。当使用await关键字时，当前方法会异步等待任务完成，然后继续执行后续代码。
        /// 因此，如果在await InitializeAsync();之后直接写那两个语句，理论上是应该等待InitializeAsync完成后再执行的。那为什么用户会遇到这个问题呢？
        /// 可能的原因有几个。首先，检查用户代码中的await InitializeAsync(); 是否正确。可能InitializeAsync方法没有正确返回Task，导致await无法正确等待。
        /// 例如，如果InitializeAsync是一个同步方法或者没有返回Task，那么await会立即继续执行后面的代码，而不会等待。
        /// 另一个可能性是，用户可能在InitializeAsync内部没有正确地实现异步操作，比如没有使用await关键字来等待内部的异步方法，导致整个InitializeAsync实际上同步执行。
        /// 这时候，虽然方法标记为async，但实际并没有异步操作，所以await会立即返回。
        /// 此外，还要检查AntdUI.ITask.Run的使用是否正确。AntdUI可能封装了任务的运行方式，可能存在特定的线程或上下文处理问题。
        /// 比如，AntdUI的任务调度是否在主线程执行，而InitializeAsync可能在后台线程执行，导致UI更新出现问题，但这里的问题不是UI更新，而是执行顺序的问题，所以可能需要更多的信息。
        /// 总结用户需要确保两点：
        /// 1. InitializeAsync方法内部所有的异步操作都正确使用await，从而保证整个方法异步完成。
        /// 2. InitializeAsync方法的返回类型是Task，而不是void。
        /// 如果这两个条件都满足，那么await InitializeAsync(); 之后的代码应该会在方法完成后执行。
        /// 如果问题仍然存在，可能需要进一步检查InitializeAsync的实现，或者查看是否有其他并发的代码干扰。
        /// 此外，还可以尝试在InitializeAsync内部添加日志或断点，确认其实际执行时间，以及是否真的异步执行。比如，在方法的开始和结束处输出日志，观察执行顺序。
        /// 另外，考虑AntdUI.ITask.Run是否可能影响执行上下文。例如，是否在某个特定的同步上下文下运行，导致await之后的代码在错误的线程上执行，从而引发问题。
        /// 不过，btn.Enabled和静态变量的赋值通常不依赖线程上下文，所以可能性较低。
        /// 综上，用户需要检查InitializeAsync的实现是否正确异步，并正确返回Task，以及所有内部操作都被正确等待。如果这些都没问题，那么可能是其他地方的代码干扰，需要进一步排查。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            if (AntdUI.Modal.open(form, LocalizeHelper.CONFIRM, originalCircularAreaResult.IsUploaded ?
                LocalizeHelper.WOULD_RESAVE_CIRCULAR_AREA_RESULT_AFTER_UPLOADING :
                circularAreaMethodBLL.GetResultExitsInDB(tempCircularAreaResult) != null ?
                  LocalizeHelper.WOULD_RESAVE_CIRCULAR_AREA_RESULT_ON_THIS_POSITION(tempCircularAreaResult.Position) :
                LocalizeHelper.WOULD_SAVE_CIRCULAR_AREA_RESULT) == DialogResult.OK)
            {
                AntdUI.Button btn = (AntdUI.Button)sender;
                btn.LoadingWaveValue = 0;
                btn.Loading = true;
                AntdUI.ITask.Run(async () =>
                {
                    try
                    {
                        var result = circularAreaMethodBLL.SaveResult(tempCircularAreaResult);
                        if (result == 0)
                        {
                            AntdUI.Notification.error(form, LocalizeHelper.ERROR, LocalizeHelper.SAVE_FAILED, AntdUI.TAlignFrom.BR, Font);
                            return;
                        }
                        else
                        {
                            AntdUI.Message.success(form, LocalizeHelper.SAVE_SUCCESSFULLY);
                            EDIT_ITEM_ID = result.ToString();
                            // 这里现在会正确等待所有初始化操作
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
            tempCircularAreaResult.WorkGroup = selectWorkGroup.SelectedValue?.ToString();
        }


        private void SelectTestNo_SelectedIndexChanged(object sender, AntdUI.IntEventArgs e)
        {
            tempCircularAreaResult.TestNo = selectTestNo.SelectedValue?.ToString();
            UpdateCoilNumberInput();
        }

        private void SelectTestNo_TextChanged(object sender, EventArgs e)
        {
            tempCircularAreaResult.TestNo = selectTestNo.Text;
            UpdateCoilNumberInput();
        }

        /// <summary>
        /// 统一更新钢卷号输入框的状态
        /// </summary>
        private void UpdateCoilNumberInput()
        {
            var hasValidTestNo = !string.IsNullOrEmpty(tempCircularAreaResult.TestNo);
            var targetItem = hasValidTestNo && testNoList != null
                ? testNoList.FirstOrDefault(t => t.TestNo == tempCircularAreaResult.TestNo)
                : null;

            var coilNumber = targetItem?.CoilNumber;
            var otherCoilNumber = targetItem?.OtherCoilNumber;

            // 优先级：CoilNumber > OtherCoilNumber > 空
            inputCoilNumber.Text = !string.IsNullOrEmpty(coilNumber)
                ? coilNumber
                : otherCoilNumber ?? string.Empty;

            // 设置只读状态：当找到有效数据且自动填充内容时锁定输入
            inputCoilNumber.ReadOnly = targetItem != null &&
                (!string.IsNullOrEmpty(coilNumber) || !string.IsNullOrEmpty(otherCoilNumber));
        }

        private void InputCoilNumber_TextChanged(object sender, EventArgs e)
        {
            tempCircularAreaResult.CoilNumber = inputCoilNumber.Text;
        }

        private void SelectPosition_SelectedIndexChanged(object sender, AntdUI.IntEventArgs e)
        {
            tempCircularAreaResult.Position = selectPosition.SelectedValue?.ToString();
        }

        private void InputAnalyst_TextChanged(object sender, EventArgs e)
        {
            tempCircularAreaResult.Analyst = inputAnalyst.Text;
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
                AntdUI.Drawer.open(form, new CircularAreaReport(form, originalCircularAreaResult.TestNo, () => { })
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
            var result = AntdUI.Modal.open(new AntdUI.Modal.Config(form, LocalizeHelper.CIRCULAR_SCALE_MODAL_TITLE, setting)
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
                var selected = scaleList.FirstOrDefault(t => selectScale.SelectedValue.Equals(FormatScaleItemName(t))).Value;
                tempCircularAreaResult.CalculateScale = selected;
            }
            else
            {
                tempCircularAreaResult.CalculateScale = null;
            }
        }

        private void CheckboxRedefine_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            if (checkboxRedefine.Checked && !string.IsNullOrEmpty(tempCircularAreaResult.RenderImagePath) && tempCircularAreaResult.Item != null)
            {
                BtnSetScale_Click(null, null);
            }
        }
    }
}