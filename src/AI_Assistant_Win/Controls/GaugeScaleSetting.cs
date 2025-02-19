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

        private readonly GaugeBlockMethodBLL gaugeBlockMethodBLL;

        private readonly ApiBLL apiBLL = ApiHandler.Instance.GetApiBLL();

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
        public void SetCurrentScaleDetails(GaugeBlockResult circularAreaResult, bool reDefined, CalculateScale scaleAtThatTime)
        {
            #region current
            var currentScale = gaugeBlockMethodBLL.GetCurrentScale();
            if (reDefined || currentScale == null)
            {
                imagePath = circularAreaResult.RenderImagePath;
                avatarCurrent.Image = Image.FromFile(imagePath);
                inputPixels.Text = circularAreaResult.Item.AreaOfPixels.ToString();
            }
            else
            {
                imagePath = currentScale.ImagePath;
                avatarCurrent.Image = Image.FromFile(imagePath);
                var settings = JsonConvert.DeserializeObject<CircularAreaScaleItem>(currentScale.Settings);
                inputPixels.Text = settings.Pixels;
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
                labelRatioThatTime.Text = $"{LocalizeHelper.SCALE_CACULATED_RATIO_TITLE}{scaleAtThatTime.Value}{LocalizeHelper.CIRCULAR_SCALE_CACULATED_RATIO_UNIT}";
            }
            #endregion
            // 调整表单大小以适应内容面板
            AdjustFormSizeToContent();
        }

        private void InputArea_TextChanged(object sender, EventArgs e)
        {
            //if (!string.IsNullOrEmpty(inputArea.Text))
            //{
            //    DisplayCaculateRatio();
            //}
            //else
            //{
            //    labelRatioCurrent.Text = LocalizeHelper.AUTO_CALCULATE;
            //}
        }

        private void DisplayCaculateRatio()
        {
            //try
            //{
            //    var result = CaculateRatio();
            //    labelRatioCurrent.Text = $"{LocalizeHelper.SCALE_CACULATED_RATIO_TITLE}{result:F2}{LocalizeHelper.CIRCULAR_SCALE_CACULATED_RATIO_UNIT}";
            //}
            //catch (Exception)
            //{
            //    labelRatioCurrent.Text = LocalizeHelper.SCALE_INPUT_ERROR;
            //}
        }

        private float CaculateRatio()
        {
            //var result = 100 * float.Parse(inputArea.Text) / float.Parse(inputPixels.Text);
            //return result;
            return 0;
        }


        public CalculateScale SaveSettings()
        {
            try
            {
                if (string.IsNullOrEmpty(inputTop.Text))
                {
                    throw new Exception(LocalizeHelper.NO_TOP_GRADUATIONS);
                }
                var add = new CalculateScale
                {
                    Key = "CircularArea",
                    Value = CaculateRatio(),
                    Unit = LocalizeHelper.CIRCULAR_SCALE_CACULATED_RATIO_UNIT,
                    ImagePath = imagePath,
                    //Settings = JsonConvert.SerializeObject(new CircularAreaScaleItem { Pixels = inputPixels.Text, MeasuredValue = inputArea.Text, TopGraduations = inputTop.Text }),
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
