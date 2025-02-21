using AI_Assistant_Win.Business;
using AI_Assistant_Win.Models;
using AI_Assistant_Win.Models.Middle;
using AI_Assistant_Win.Utils;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AI_Assistant_Win
{
    public partial class GaugeScaleSetting : UserControl
    {
        Form form;

        private bool isUpdating = false;  // 标记是否正在更新其他输入框

        private readonly GaugeBlockMethodBLL gaugeBlockMethodBLL;

        private readonly ApiBLL apiBLL = ApiHandler.Instance.GetApiBLL();

        private GaugeBlockResult gaugeBlockDetection;
        public GaugeScaleSetting(Form _form)
        {
            form = _form;
            gaugeBlockMethodBLL = new GaugeBlockMethodBLL();
            InitializeComponent();
        }
        private void AdjustFormSizeToContent()
        {
            if (!panelAtThatTime.Visible)
            {
                this.ClientSize = new Size(this.Width / 2, this.Height);
            }
        }

        private string imagePath;
        public void SetCurrentScaleDetails(GaugeBlockResult gaugeBlockResult, bool reDefined, CalculateScale scaleAtThatTime)
        {
            gaugeBlockDetection = gaugeBlockResult;
            #region current
            var currentScale = gaugeBlockMethodBLL.GetCurrentScale();
            if (reDefined || currentScale == null)
            {
                imagePath = gaugeBlockDetection.RenderImagePath;
                avatarCurrent.Image = Image.FromFile(imagePath);
                inputAreaOfPixels.Text = gaugeBlockDetection.Item.AreaOfPixels.ToString();
                inputExtractedArea.Text = $"{gaugeBlockDetection.Item.ExtractedAreaOfPixels:F2}";
                inputAreaLoss.Text = $"{(gaugeBlockDetection.Item.AreaOfPixels - gaugeBlockDetection.Item.ExtractedAreaOfPixels) / gaugeBlockDetection.Item.AreaOfPixels:P2}";
                textSideABPixels.Text = $"{gaugeBlockDetection.Item.SidePixels["AB"]:F2}";
                textSideBCPixels.Text = $"{gaugeBlockDetection.Item.SidePixels["BC"]:F2}";
                textSideCDPixels.Text = $"{gaugeBlockDetection.Item.SidePixels["CD"]:F2}";
                textSideDAPixels.Text = $"{gaugeBlockDetection.Item.SidePixels["DA"]:F2}";
            }
            else
            {
                imagePath = currentScale.ImagePath;
                avatarCurrent.Image = Image.FromFile(imagePath);
                var settings = JsonConvert.DeserializeObject<CircularAreaScaleItem>(currentScale.Settings);
                inputAreaOfPixels.Text = settings.Pixels;
                //inputArea.Text = settings.MeasuredValue;
                //inputTop.Text = settings.TopGraduations;
                //labelRatioCurrent.Text = $"{LocalizeHelper.SCALE_CACULATED_RATIO_TITLE}{currentScale.Value}{LocalizeHelper.CIRCULAR_SCALE_CACULATED_RATIO_UNIT}";
            }
            #endregion
            #region at that time
            if (scaleAtThatTime != null)
            {
                panelAtThatTime.Visible = true;
                avatarThatTime.Image = Image.FromFile(scaleAtThatTime.ImagePath);
                var settings = JsonConvert.DeserializeObject<CircularAreaScaleItem>(scaleAtThatTime.Settings);
                inputPixelsThatTime.Text = settings.Pixels;
                inputAreaThatTime.Text = settings.MeasuredValue;
                inputTopThatTime.Text = settings.TopGraduations;
                labelRatioThatTime.Text = $"{LocalizeHelper.SCALE_CACULATED_RESULT_TITLE}{scaleAtThatTime.Value}{LocalizeHelper.CIRCULAR_SCALE_CACULATED_RATIO_UNIT}";
            }
            #endregion
            // 调整表单大小以适应内容面板
            AdjustFormSizeToContent();
        }

        private void InputSideABLength_TextChanged(object sender, EventArgs e)
        {
            if (isUpdating) return;  // 防止递归调用

            isUpdating = true;

            if (!string.IsNullOrEmpty(inputSideABLength.Text))
            {
                DisplayCaculatedResult("AB");
            }
            else
            {
                ClearSideResult("AB");
                label1ResultDescription.Text = LocalizeHelper.AUTO_CALCULATE;
            }

            isUpdating = false;
        }

        private void InputSideBCLength_TextChanged(object sender, EventArgs e)
        {
            if (isUpdating) return;  // 防止递归调用

            isUpdating = true;

            if (!string.IsNullOrEmpty(inputSideBCLength.Text))
            {
                DisplayCaculatedResult("BC");
            }
            else
            {
                ClearSideResult("BC");
                label1ResultDescription.Text = LocalizeHelper.AUTO_CALCULATE;
            }

            isUpdating = false;
        }

        private void InputSideCDLength_TextChanged(object sender, EventArgs e)
        {
            if (isUpdating) return;  // 防止递归调用

            isUpdating = true;

            if (!string.IsNullOrEmpty(inputSideCDLength.Text))
            {
                DisplayCaculatedResult("CD");
            }
            else
            {
                ClearSideResult("CD");
                label1ResultDescription.Text = LocalizeHelper.AUTO_CALCULATE;
            }

            isUpdating = false;
        }

        private void InputSideDALength_TextChanged(object sender, EventArgs e)
        {
            if (isUpdating) return;  // 防止递归调用

            isUpdating = true;

            if (!string.IsNullOrEmpty(inputSideDALength.Text))
            {
                DisplayCaculatedResult("DA");
            }
            else
            {
                ClearSideResult("DA");
                label1ResultDescription.Text = LocalizeHelper.AUTO_CALCULATE;
            }

            isUpdating = false;
        }

        private void DisplayCaculatedResult(string side)
        {
            try
            {
                switch (side)
                {
                    case "AB":
                        inputSideBCLength.Text = $"{float.Parse(textSideBCPixels.Text) * float.Parse(inputSideABLength.Text) / float.Parse(textSideABPixels.Text):F2}";
                        inputSideCDLength.Text = $"{float.Parse(textSideCDPixels.Text) * float.Parse(inputSideABLength.Text) / float.Parse(textSideABPixels.Text):F2}";
                        inputSideDALength.Text = $"{float.Parse(textSideDAPixels.Text) * float.Parse(inputSideABLength.Text) / float.Parse(textSideABPixels.Text):F2}";
                        break;
                    case "BC":
                        inputSideABLength.Text = $"{float.Parse(textSideABPixels.Text) * float.Parse(inputSideBCLength.Text) / float.Parse(textSideBCPixels.Text):F2}";
                        inputSideCDLength.Text = $"{float.Parse(textSideCDPixels.Text) * float.Parse(inputSideBCLength.Text) / float.Parse(textSideBCPixels.Text):F2}";
                        inputSideDALength.Text = $"{float.Parse(textSideDAPixels.Text) * float.Parse(inputSideBCLength.Text) / float.Parse(textSideBCPixels.Text):F2}";
                        break;
                    case "CD":
                        inputSideABLength.Text = $"{float.Parse(textSideABPixels.Text) * float.Parse(inputSideCDLength.Text) / float.Parse(textSideCDPixels.Text):F2}";
                        inputSideBCLength.Text = $"{float.Parse(textSideBCPixels.Text) * float.Parse(inputSideCDLength.Text) / float.Parse(textSideCDPixels.Text):F2}";
                        inputSideDALength.Text = $"{float.Parse(textSideDAPixels.Text) * float.Parse(inputSideCDLength.Text) / float.Parse(textSideCDPixels.Text):F2}";
                        break;
                    case "DA":
                        inputSideABLength.Text = $"{float.Parse(textSideABPixels.Text) * float.Parse(inputSideDALength.Text) / float.Parse(textSideDAPixels.Text):F2}";
                        inputSideBCLength.Text = $"{float.Parse(textSideBCPixels.Text) * float.Parse(inputSideDALength.Text) / float.Parse(textSideDAPixels.Text):F2}";
                        inputSideCDLength.Text = $"{float.Parse(textSideCDPixels.Text) * float.Parse(inputSideDALength.Text) / float.Parse(textSideDAPixels.Text):F2}";
                        break;
                    default:
                        break;
                }

                label1ResultDescription.Text = $"{LocalizeHelper.LENGTH_SCALE_RESULT_TITLE}{float.Parse(inputSideABLength.Text) / float.Parse(textSideABPixels.Text):F2}{LocalizeHelper.LENGTH_SCALE_CACULATED_RATIO_UNIT}\n" +
                    $"{LocalizeHelper.AREA_SCALE_RESULT_TITLE}{Math.Pow(float.Parse(inputSideABLength.Text) / float.Parse(textSideABPixels.Text), 2):F4}{LocalizeHelper.AREA_SCALE_CACULATED_RATIO_UNIT}\n" +
                    $"{LocalizeHelper.CALCULATED_AREA_TITLE}{float.Parse(inputAreaOfPixels.Text) * Math.Pow(float.Parse(inputSideABLength.Text) / float.Parse(textSideABPixels.Text), 2):F4}±{(float.Parse(inputAreaOfPixels.Text) - float.Parse(inputExtractedArea.Text)) * Math.Pow(float.Parse(inputSideABLength.Text) / float.Parse(textSideABPixels.Text), 2):F4}{LocalizeHelper.SQUARE_MILLIMETER}";
            }
            catch (Exception)
            {
                ClearSideResult(side);
                label1ResultDescription.Text = LocalizeHelper.SCALE_INPUT_ERROR;
            }
        }

        private void ClearSideResult(string side)
        {
            switch (side)
            {
                case "AB":
                    inputSideBCLength.Text = string.Empty;
                    inputSideCDLength.Text = string.Empty;
                    inputSideDALength.Text = string.Empty;
                    break;
                case "BC":
                    inputSideABLength.Text = string.Empty;
                    inputSideCDLength.Text = string.Empty;
                    inputSideDALength.Text = string.Empty;
                    break;
                case "CD":
                    inputSideABLength.Text = string.Empty;
                    inputSideBCLength.Text = string.Empty;
                    inputSideDALength.Text = string.Empty;
                    break;
                case "DA":
                    inputSideABLength.Text = string.Empty;
                    inputSideBCLength.Text = string.Empty;
                    inputSideCDLength.Text = string.Empty;
                    break;
                default:
                    break;
            }
        }

        public CalculateScale SaveSettings()
        {
            try
            {
                if (string.IsNullOrEmpty(inputTopGrade.Text))
                {
                    throw new Exception(LocalizeHelper.NO_TOP_GRADUATIONS);
                }
                var add = new CalculateScale
                {
                    Key = "GaugeBlock",
                    Value = float.Parse(inputSideABLength.Text) / float.Parse(textSideABPixels.Text),
                    Unit = LocalizeHelper.CIRCULAR_SCALE_CACULATED_RATIO_UNIT,
                    ImagePath = imagePath,
                    Settings = JsonConvert.SerializeObject(new GaugeBlockScaleItem
                    {
                        TopGraduations = inputTopGrade.Text,
                        Pixels = inputAreaOfPixels.Text,
                        ExtractedPixels = inputExtractedArea.Text,
                        AreaLoss = inputAreaLoss.Text,
                        Prediction = gaugeBlockDetection.Item.Prediction, // 顶点位置
                        Sides = [
                                new QuadrilateralSide("AB", textSideABPixels.Text , inputSideABLength.Text),
                                new QuadrilateralSide("BC", textSideBCPixels.Text , inputSideBCLength.Text),
                                new QuadrilateralSide("CD", textSideCDPixels.Text , inputSideCDLength.Text),
                                new QuadrilateralSide("DA", textSideDAPixels.Text , inputSideDALength.Text)
                            ],
                        DisplayText = label1ResultDescription.Text
                    }, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }),
                    Creator = $"{apiBLL.LoginUserInfo.Username}-{apiBLL.LoginUserInfo.Nickname}",
                    CreateTime = DateTime.Now
                };
                var currentScale = gaugeBlockMethodBLL.GetCurrentScale();
                if (currentScale != null && add.Equals(currentScale))
                {
                    throw new Exception(LocalizeHelper.NO_NEED_TO_SAVE_THE_SAME_SCALE);
                }
                gaugeBlockMethodBLL.SaveScaleSetting(add);
                return add;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
