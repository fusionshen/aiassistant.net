using AI_Assistant_Win.Business;
using AI_Assistant_Win.Models;
using AI_Assistant_Win.Models.Middle;
using AI_Assistant_Win.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AI_Assistant_Win
{
    public partial class CircularScaleSetting : UserControl
    {
        Form form;

        private readonly CircularAreaMethodBLL circularAreaMethodBLL;

        private readonly ApiBLL apiBLL = ApiHandler.Instance.GetApiBLL();

        public CircularScaleSetting(Form _form)
        {
            form = _form;
            circularAreaMethodBLL = new CircularAreaMethodBLL();
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
        public void SetCurrentScaleDetails(CircularAreaResult circularAreaResult, bool reDefined, CalculateScale scaleAtThatTime)
        {
            #region current
            var currentScale = circularAreaMethodBLL.GetCurrentScale();
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
                var settings = JsonConvert.DeserializeObject<List<BlacknessScaleItem>>(currentScale.Settings);
                //textSurfaceOPPixels.Text = settings.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_OP))?.Pixels;
                //inputSurfaceOPCurrent.Text = settings.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_OP))?.MeasuredValue;
                //textSurfaceCEPixels.Text = settings.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_CE))?.Pixels;
                //inputSurfaceCECurrent.Text = settings.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_CE))?.MeasuredValue;
                //textSurfaceDRPixels.Text = settings.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_DR))?.Pixels;
                //inputSurfaceDRCurrent.Text = settings.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_DR))?.MeasuredValue;
                //textInsideOPPixels.Text = settings.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_OP))?.Pixels;
                //inputInsideOPCurrent.Text = settings.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_OP))?.MeasuredValue;
                //textInsideCEPixels.Text = settings.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_CE))?.Pixels;
                //inputInsideCECurrent.Text = settings.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_CE))?.MeasuredValue;
                //textInsideDRPixels.Text = settings.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_DR))?.Pixels;
                //inputInsideDRCurrent.Text = settings.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_DR))?.MeasuredValue;
                labelRatioCurrent.Text = $"{LocalizeHelper.SCALE_CACULATED_RATIO_TITLE}{currentScale.Value}{LocalizeHelper.BLACKNESS_SCALE_CACULATED_RATIO_UNIT}";
            }
            #endregion
            #region at that time
            if (scaleAtThatTime != null)
            {
                panelAtThatTime.Visible = true;
                avatarThatTime.Image = Image.FromFile(scaleAtThatTime.ImagePath);
                var settings = JsonConvert.DeserializeObject<List<BlacknessScaleItem>>(scaleAtThatTime.Settings);
                //textSurfaceOPPixelsThatTime.Text = settings.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_OP))?.Pixels;
                //inputSurfaceOPThatTime.Text = settings.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_OP))?.MeasuredValue;
                //textSurfaceCEPixelsThatTime.Text = settings.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_CE))?.Pixels;
                //inputSurfaceCEThatTime.Text = settings.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_CE))?.MeasuredValue;
                //textSurfaceDRPixelsThatTime.Text = settings.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_DR))?.Pixels;
                //inputSurfaceDRThatTime.Text = settings.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_DR))?.MeasuredValue;
                //textInsideOPPixelsThatTime.Text = settings.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_OP))?.Pixels;
                //inputInsideOPThatTime.Text = settings.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_OP))?.MeasuredValue;
                //textInsideCEPixelsThatTime.Text = settings.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_CE))?.Pixels;
                //inputInsideCEThatTime.Text = settings.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_CE))?.MeasuredValue;
                //textInsideDRPixelsThatTime.Text = settings.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_DR))?.Pixels;
                //inputInsideDRThatTime.Text = settings.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_DR))?.MeasuredValue;
                //labelRatioAtThatTime.Text = $"{LocalizeHelper.BLACKNESS_SCALE_CACULATED_RATIO_TITLE}{scaleAtThatTime.Value}{LocalizeHelper.BLACKNESS_SCALE_CACULATED_RATIO_UNIT}";
            }
            #endregion
            // 调整表单大小以适应内容面板
            AdjustFormSizeToContent();
        }

        private void InputArea_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(inputArea.Text))
            {
                DisplayCaculateRatio();
            }
            else
            {
                labelRatioCurrent.Text = LocalizeHelper.AUTO_CALCULATE;
            }
        }

        private void DisplayCaculateRatio()
        {
            try
            {
                var result = CaculateRatio();
                labelRatioCurrent.Text = $"{LocalizeHelper.SCALE_CACULATED_RATIO_TITLE}{result:F2}{LocalizeHelper.CIRCULAR_SCALE_CACULATED_RATIO_UNIT}";
            }
            catch (Exception)
            {
                labelRatioCurrent.Text = LocalizeHelper.SCALE_INPUT_ERROR;
            }
        }

        private float CaculateRatio()
        {
            var result = 100 * float.Parse(inputArea.Text) / float.Parse(inputPixels.Text);
            return result;
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
                    Settings = JsonConvert.SerializeObject(new CircularAreaScaleItem { Pixels = inputPixels.Text, MeasuredValue = inputArea.Text, TopGraduations = inputTop.Text }),
                    Creator = $"{apiBLL.LoginUserInfo.Username}-{apiBLL.LoginUserInfo.Nickname}",
                    CreateTime = DateTime.Now
                };
                var currentScale = circularAreaMethodBLL.GetCurrentScale();
                if (currentScale != null && add.Equals(currentScale))
                {
                    throw new Exception(LocalizeHelper.NO_NEED_TO_SAVE_THE_SAME_SCALE);
                }
                circularAreaMethodBLL.SaveScaleSetting(add);
                return add;
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
