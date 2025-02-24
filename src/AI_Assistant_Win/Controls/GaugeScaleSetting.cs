using AI_Assistant_Win.Business;
using AI_Assistant_Win.Models;
using AI_Assistant_Win.Models.Middle;
using AI_Assistant_Win.Utils;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.Linq;
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
                textEdgeABPixels.Text = $"{gaugeBlockDetection.Item.EdgePixels["AB"]:F2}";
                textEdgeBCPixels.Text = $"{gaugeBlockDetection.Item.EdgePixels["BC"]:F2}";
                textEdgeCDPixels.Text = $"{gaugeBlockDetection.Item.EdgePixels["CD"]:F2}";
                textEdgeDAPixels.Text = $"{gaugeBlockDetection.Item.EdgePixels["DA"]:F2}";
            }
            else
            {
                imagePath = currentScale.ImagePath;
                avatarCurrent.Image = Image.FromFile(imagePath);
                var settings = JsonConvert.DeserializeObject<GaugeBlockScaleItem>(currentScale.Settings);
                inputAreaOfPixels.Text = settings.Pixels;
                inputExtractedArea.Text = settings.ExtractedPixels;
                inputAreaLoss.Text = settings.AreaLoss;
                textEdgeABPixels.Text = settings.Edges.FirstOrDefault(t => "AB".Equals(t.Edge))?.PixelLength;
                textEdgeBCPixels.Text = settings.Edges.FirstOrDefault(t => "BC".Equals(t.Edge))?.PixelLength;
                textEdgeCDPixels.Text = settings.Edges.FirstOrDefault(t => "CD".Equals(t.Edge))?.PixelLength;
                textEdgeDAPixels.Text = settings.Edges.FirstOrDefault(t => "DA".Equals(t.Edge))?.PixelLength;
                inputEdgeABLength.Text = settings.Edges.FirstOrDefault(t => "AB".Equals(t.Edge))?.RealLength;
                inputEdgeBCLength.Text = settings.Edges.FirstOrDefault(t => "BC".Equals(t.Edge))?.RealLength;
                inputEdgeCDLength.Text = settings.Edges.FirstOrDefault(t => "CD".Equals(t.Edge))?.RealLength;
                inputEdgeDALength.Text = settings.Edges.FirstOrDefault(t => "DA".Equals(t.Edge))?.RealLength;
                inputTopGrade.Text = settings.TopGraduations;
                labelResultDescription.Text = settings.DisplayText;
            }
            #endregion
            #region at that time
            if (scaleAtThatTime != null)
            {
                panelAtThatTime.Visible = true;
                avatarThatTime.Image = Image.FromFile(scaleAtThatTime.ImagePath);
                var settings = JsonConvert.DeserializeObject<GaugeBlockScaleItem>(scaleAtThatTime.Settings);
                inputAreaOfPixelsThatTime.Text = settings.Pixels;
                inputExtractedAreaThatTime.Text = settings.ExtractedPixels;
                inputAreaLossThatTime.Text = settings.AreaLoss;
                textEdgeABPixelsThatTime.Text = settings.Edges.FirstOrDefault(t => "AB".Equals(t.Edge))?.PixelLength;
                textEdgeBCPixelsThatTime.Text = settings.Edges.FirstOrDefault(t => "BC".Equals(t.Edge))?.PixelLength;
                textEdgeCDPixelsThatTime.Text = settings.Edges.FirstOrDefault(t => "CD".Equals(t.Edge))?.PixelLength;
                textEdgeDAPixelsThatTime.Text = settings.Edges.FirstOrDefault(t => "DA".Equals(t.Edge))?.PixelLength;
                inputEdgeABLengthThatTime.Text = settings.Edges.FirstOrDefault(t => "AB".Equals(t.Edge))?.RealLength;
                inputEdgeBCLengthThatTime.Text = settings.Edges.FirstOrDefault(t => "BC".Equals(t.Edge))?.RealLength;
                inputEdgeCDLengthThatTime.Text = settings.Edges.FirstOrDefault(t => "CD".Equals(t.Edge))?.RealLength;
                inputEdgeDALengthThatTime.Text = settings.Edges.FirstOrDefault(t => "DA".Equals(t.Edge))?.RealLength;
                inputTopGradeThatTime.Text = settings.TopGraduations;
                labelResultDescriptionThatTime.Text = settings.DisplayText;
            }
            #endregion
            // 调整表单大小以适应内容面板
            AdjustFormSizeToContent();
        }

        private void InputEdgeABLength_TextChanged(object sender, EventArgs e)
        {
            if (isUpdating) return;  // 防止递归调用

            isUpdating = true;

            if (!string.IsNullOrEmpty(inputEdgeABLength.Text))
            {
                DisplayCaculatedResult("AB");
            }
            else
            {
                ClearEdgeResult("AB");
                labelResultDescription.Text = LocalizeHelper.AUTO_CALCULATE;
            }

            isUpdating = false;
        }

        private void InputEdgeBCLength_TextChanged(object sender, EventArgs e)
        {
            if (isUpdating) return;  // 防止递归调用

            isUpdating = true;

            if (!string.IsNullOrEmpty(inputEdgeBCLength.Text))
            {
                DisplayCaculatedResult("BC");
            }
            else
            {
                ClearEdgeResult("BC");
                labelResultDescription.Text = LocalizeHelper.AUTO_CALCULATE;
            }

            isUpdating = false;
        }

        private void InputEdgeCDLength_TextChanged(object sender, EventArgs e)
        {
            if (isUpdating) return;  // 防止递归调用

            isUpdating = true;

            if (!string.IsNullOrEmpty(inputEdgeCDLength.Text))
            {
                DisplayCaculatedResult("CD");
            }
            else
            {
                ClearEdgeResult("CD");
                labelResultDescription.Text = LocalizeHelper.AUTO_CALCULATE;
            }

            isUpdating = false;
        }

        private void InputEdgeDALength_TextChanged(object sender, EventArgs e)
        {
            if (isUpdating) return;  // 防止递归调用

            isUpdating = true;

            if (!string.IsNullOrEmpty(inputEdgeDALength.Text))
            {
                DisplayCaculatedResult("DA");
            }
            else
            {
                ClearEdgeResult("DA");
                labelResultDescription.Text = LocalizeHelper.AUTO_CALCULATE;
            }

            isUpdating = false;
        }

        private void DisplayCaculatedResult(string edge)
        {
            try
            {
                switch (edge)
                {
                    case "AB":
                        inputEdgeBCLength.Text = $"{float.Parse(textEdgeBCPixels.Text) * float.Parse(inputEdgeABLength.Text) / float.Parse(textEdgeABPixels.Text):F2}";
                        inputEdgeCDLength.Text = $"{float.Parse(textEdgeCDPixels.Text) * float.Parse(inputEdgeABLength.Text) / float.Parse(textEdgeABPixels.Text):F2}";
                        inputEdgeDALength.Text = $"{float.Parse(textEdgeDAPixels.Text) * float.Parse(inputEdgeABLength.Text) / float.Parse(textEdgeABPixels.Text):F2}";
                        break;
                    case "BC":
                        inputEdgeABLength.Text = $"{float.Parse(textEdgeABPixels.Text) * float.Parse(inputEdgeBCLength.Text) / float.Parse(textEdgeBCPixels.Text):F2}";
                        inputEdgeCDLength.Text = $"{float.Parse(textEdgeCDPixels.Text) * float.Parse(inputEdgeBCLength.Text) / float.Parse(textEdgeBCPixels.Text):F2}";
                        inputEdgeDALength.Text = $"{float.Parse(textEdgeDAPixels.Text) * float.Parse(inputEdgeBCLength.Text) / float.Parse(textEdgeBCPixels.Text):F2}";
                        break;
                    case "CD":
                        inputEdgeABLength.Text = $"{float.Parse(textEdgeABPixels.Text) * float.Parse(inputEdgeCDLength.Text) / float.Parse(textEdgeCDPixels.Text):F2}";
                        inputEdgeBCLength.Text = $"{float.Parse(textEdgeBCPixels.Text) * float.Parse(inputEdgeCDLength.Text) / float.Parse(textEdgeCDPixels.Text):F2}";
                        inputEdgeDALength.Text = $"{float.Parse(textEdgeDAPixels.Text) * float.Parse(inputEdgeCDLength.Text) / float.Parse(textEdgeCDPixels.Text):F2}";
                        break;
                    case "DA":
                        inputEdgeABLength.Text = $"{float.Parse(textEdgeABPixels.Text) * float.Parse(inputEdgeDALength.Text) / float.Parse(textEdgeDAPixels.Text):F2}";
                        inputEdgeBCLength.Text = $"{float.Parse(textEdgeBCPixels.Text) * float.Parse(inputEdgeDALength.Text) / float.Parse(textEdgeDAPixels.Text):F2}";
                        inputEdgeCDLength.Text = $"{float.Parse(textEdgeCDPixels.Text) * float.Parse(inputEdgeDALength.Text) / float.Parse(textEdgeDAPixels.Text):F2}";
                        break;
                    default:
                        break;
                }

                labelResultDescription.Text = $"{LocalizeHelper.LENGTH_SCALE_RESULT_TITLE}{float.Parse(inputEdgeABLength.Text) / float.Parse(textEdgeABPixels.Text):F2}{LocalizeHelper.LENGTH_SCALE_CACULATED_RATIO_UNIT}\n" +
                    $"{LocalizeHelper.AREA_SCALE_RESULT_TITLE}{Math.Pow(float.Parse(inputEdgeABLength.Text) / float.Parse(textEdgeABPixels.Text), 2):F4}{LocalizeHelper.AREA_SCALE_CACULATED_RATIO_UNIT}\n" +
                    $"{LocalizeHelper.CALCULATED_AREA_TITLE}{float.Parse(inputAreaOfPixels.Text) * Math.Pow(float.Parse(inputEdgeABLength.Text) / float.Parse(textEdgeABPixels.Text), 2):F4}±{(float.Parse(inputAreaOfPixels.Text) - float.Parse(inputExtractedArea.Text)) * Math.Pow(float.Parse(inputEdgeABLength.Text) / float.Parse(textEdgeABPixels.Text), 2):F4}{LocalizeHelper.SQUARE_MILLIMETER}";
            }
            catch (Exception)
            {
                ClearEdgeResult(edge);
                labelResultDescription.Text = LocalizeHelper.SCALE_INPUT_ERROR;
            }
        }

        private void ClearEdgeResult(string edge)
        {
            switch (edge)
            {
                case "AB":
                    inputEdgeBCLength.Text = string.Empty;
                    inputEdgeCDLength.Text = string.Empty;
                    inputEdgeDALength.Text = string.Empty;
                    break;
                case "BC":
                    inputEdgeABLength.Text = string.Empty;
                    inputEdgeCDLength.Text = string.Empty;
                    inputEdgeDALength.Text = string.Empty;
                    break;
                case "CD":
                    inputEdgeABLength.Text = string.Empty;
                    inputEdgeBCLength.Text = string.Empty;
                    inputEdgeDALength.Text = string.Empty;
                    break;
                case "DA":
                    inputEdgeABLength.Text = string.Empty;
                    inputEdgeBCLength.Text = string.Empty;
                    inputEdgeCDLength.Text = string.Empty;
                    break;
                default:
                    break;
            }
        }

        public CalculateScale SaveSettings()
        {

            if (string.IsNullOrEmpty(inputTopGrade.Text))
            {
                throw new Exception(LocalizeHelper.NO_TOP_GRADUATIONS);
            }
            try
            {
                var add = new CalculateScale
                {
                    Key = "GaugeBlock",
                    Value = float.Parse(inputEdgeABLength.Text) / float.Parse(textEdgeABPixels.Text),
                    Unit = LocalizeHelper.LENGTH_SCALE_CACULATED_RATIO_UNIT,
                    ImagePath = imagePath,
                    Settings = JsonConvert.SerializeObject(new GaugeBlockScaleItem
                    {
                        TopGraduations = inputTopGrade.Text,
                        Pixels = inputAreaOfPixels.Text,
                        ExtractedPixels = inputExtractedArea.Text,
                        AreaLoss = inputAreaLoss.Text,
                        Prediction = gaugeBlockDetection.Item.Prediction, // 顶点位置
                        Edges = [
                                new QuadrilateralEdge("AB", textEdgeABPixels.Text , inputEdgeABLength.Text),
                                new QuadrilateralEdge("BC", textEdgeBCPixels.Text , inputEdgeBCLength.Text),
                                new QuadrilateralEdge("CD", textEdgeCDPixels.Text , inputEdgeCDLength.Text),
                                new QuadrilateralEdge("DA", textEdgeDAPixels.Text , inputEdgeDALength.Text)
                            ],
                        DisplayText = labelResultDescription.Text
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
