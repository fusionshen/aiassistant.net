using AI_Assistant_Win.Business;
using AI_Assistant_Win.Models;
using AI_Assistant_Win.Models.Enums;
using AI_Assistant_Win.Models.Middle;
using AI_Assistant_Win.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AI_Assistant_Win
{
    public partial class BlacknessScaleSetting : UserControl
    {
        Form form;
        private readonly BlacknessMethodBLL blacknessMethodBLL;

        private readonly ApiBLL apiBLL = ApiHandler.Instance.GetApiBLL();

        public BlacknessScaleSetting(Form _form)
        {
            form = _form;
            blacknessMethodBLL = new BlacknessMethodBLL();
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
        public void SetCurrentScaleDetails(BlacknessResult blacknessResult, bool reDefined, CalculateScale scaleAtThatTime)
        {
            #region current
            var currentScale = blacknessMethodBLL.GetCurrentScale();
            if (reDefined || currentScale == null)
            {
                imagePath = blacknessResult.RenderImagePath;
                avatarCurrent.Image = Image.FromFile(imagePath);
                textSurfaceOPPixels.Text = blacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_OP))?.Width.ToString("F2");
                textSurfaceCEPixels.Text = blacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_CE))?.Width.ToString("F2");
                textSurfaceDRPixels.Text = blacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_DR))?.Width.ToString("F2");
                textInsideOPPixels.Text = blacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_OP))?.Width.ToString("F2");
                textInsideCEPixels.Text = blacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_CE))?.Width.ToString("F2");
                textInsideDRPixels.Text = blacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_DR))?.Width.ToString("F2");
            }
            else
            {
                imagePath = currentScale.ImagePath;
                avatarCurrent.Image = Image.FromFile(imagePath);
                var settings = JsonConvert.DeserializeObject<List<BlacknessScaleItem>>(currentScale.Settings);
                textSurfaceOPPixels.Text = settings.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_OP))?.Pixels;
                inputSurfaceOPCurrent.Text = settings.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_OP))?.MeasuredValue;
                textSurfaceCEPixels.Text = settings.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_CE))?.Pixels;
                inputSurfaceCECurrent.Text = settings.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_CE))?.MeasuredValue;
                textSurfaceDRPixels.Text = settings.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_DR))?.Pixels;
                inputSurfaceDRCurrent.Text = settings.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_DR))?.MeasuredValue;
                textInsideOPPixels.Text = settings.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_OP))?.Pixels;
                inputInsideOPCurrent.Text = settings.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_OP))?.MeasuredValue;
                textInsideCEPixels.Text = settings.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_CE))?.Pixels;
                inputInsideCECurrent.Text = settings.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_CE))?.MeasuredValue;
                textInsideDRPixels.Text = settings.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_DR))?.Pixels;
                inputInsideDRCurrent.Text = settings.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_DR))?.MeasuredValue;
                labelRatioCurrent.Text = $"{LocalizeHelper.SCALE_CACULATED_RESULT_TITLE}{currentScale.Value}{LocalizeHelper.LENGTH_SCALE_CACULATED_RATIO_UNIT}";
            }
            #endregion
            #region at that time
            if (scaleAtThatTime != null)
            {
                panelAtThatTime.Visible = true;
                avatarThatTime.Image = Image.FromFile(scaleAtThatTime.ImagePath);
                var settings = JsonConvert.DeserializeObject<List<BlacknessScaleItem>>(scaleAtThatTime.Settings);
                textSurfaceOPPixelsThatTime.Text = settings.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_OP))?.Pixels;
                inputSurfaceOPThatTime.Text = settings.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_OP))?.MeasuredValue;
                textSurfaceCEPixelsThatTime.Text = settings.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_CE))?.Pixels;
                inputSurfaceCEThatTime.Text = settings.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_CE))?.MeasuredValue;
                textSurfaceDRPixelsThatTime.Text = settings.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_DR))?.Pixels;
                inputSurfaceDRThatTime.Text = settings.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_DR))?.MeasuredValue;
                textInsideOPPixelsThatTime.Text = settings.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_OP))?.Pixels;
                inputInsideOPThatTime.Text = settings.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_OP))?.MeasuredValue;
                textInsideCEPixelsThatTime.Text = settings.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_CE))?.Pixels;
                inputInsideCEThatTime.Text = settings.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_CE))?.MeasuredValue;
                textInsideDRPixelsThatTime.Text = settings.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_DR))?.Pixels;
                inputInsideDRThatTime.Text = settings.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_DR))?.MeasuredValue;
                labelRatioAtThatTime.Text = $"{LocalizeHelper.SCALE_CACULATED_RESULT_TITLE}{scaleAtThatTime.Value}{LocalizeHelper.LENGTH_SCALE_CACULATED_RATIO_UNIT}";
            }
            #endregion
            // 调整表单大小以适应内容面板
            AdjustFormSizeToContent();
        }

        private void InputSurfaceOPCurrent_TextChanged(object sender, System.EventArgs e)
        {
            DisplayCaculateRatio();
        }

        private void DisplayCaculateRatio()
        {
            try
            {
                var result = CaculateRatio();
                labelRatioCurrent.Text = $"{LocalizeHelper.SCALE_CACULATED_RESULT_TITLE}{result:F2}{LocalizeHelper.LENGTH_SCALE_CACULATED_RATIO_UNIT}";
            }
            catch (Exception)
            {
                labelRatioCurrent.Text = LocalizeHelper.SCALE_INPUT_ERROR;
            }
        }

        private float CaculateRatio()
        {
            var dics = new Dictionary<string, string>();
            #region 构建算子
            if (!string.IsNullOrEmpty(textSurfaceOPPixels.Text) && !string.IsNullOrEmpty(inputSurfaceOPCurrent.Text))
            {
                dics.Add(textSurfaceOPPixels.Text, inputSurfaceOPCurrent.Text);
            }
            if (!string.IsNullOrEmpty(textSurfaceCEPixels.Text) && !string.IsNullOrEmpty(inputSurfaceCECurrent.Text))
            {
                dics.Add(textSurfaceCEPixels.Text, inputSurfaceCECurrent.Text);
            }
            if (!string.IsNullOrEmpty(textSurfaceDRPixels.Text) && !string.IsNullOrEmpty(inputSurfaceDRCurrent.Text))
            {
                dics.Add(textSurfaceDRPixels.Text, inputSurfaceDRCurrent.Text);
            }
            if (!string.IsNullOrEmpty(textInsideOPPixels.Text) && !string.IsNullOrEmpty(inputInsideOPCurrent.Text))
            {
                dics.Add(textInsideOPPixels.Text, inputInsideOPCurrent.Text);
            }
            if (!string.IsNullOrEmpty(textInsideCEPixels.Text) && !string.IsNullOrEmpty(inputInsideCECurrent.Text))
            {
                dics.Add(textInsideCEPixels.Text, inputInsideCECurrent.Text);
            }
            if (!string.IsNullOrEmpty(textInsideDRPixels.Text) && !string.IsNullOrEmpty(inputInsideDRCurrent.Text))
            {
                dics.Add(textInsideDRPixels.Text, inputInsideDRCurrent.Text);
            }
            #endregion
            if (dics.Count == 0)
            {
                return 1f;
            }
            var result = dics.Select(t => float.Parse(t.Value) / float.Parse(t.Key)).Average();
            return result;
        }

        private void InputSurfaceCECurrent_TextChanged(object sender, System.EventArgs e)
        {
            DisplayCaculateRatio();
        }

        private void InputSurfaceDRCurrent_TextChanged(object sender, System.EventArgs e)
        {
            DisplayCaculateRatio();
        }

        private void InputInsideOPCurrent_TextChanged(object sender, System.EventArgs e)
        {
            DisplayCaculateRatio();
        }

        private void InputInsideCECurrent_TextChanged(object sender, System.EventArgs e)
        {
            DisplayCaculateRatio();
        }

        private void InputInsideDRCurrent_TextChanged(object sender, System.EventArgs e)
        {
            DisplayCaculateRatio();
        }

        public CalculateScale SaveSettings()
        {
            try
            {
                var items = new List<BlacknessScaleItem> {
                    new() { Location = BlacknessLocationKind.SURFACE_OP, Pixels = textSurfaceOPPixels.Text, MeasuredValue = inputSurfaceOPCurrent.Text },
                    new() { Location = BlacknessLocationKind.SURFACE_CE, Pixels = textSurfaceCEPixels.Text, MeasuredValue = inputSurfaceCECurrent.Text },
                    new() { Location = BlacknessLocationKind.SURFACE_DR, Pixels = textSurfaceDRPixels.Text, MeasuredValue = inputSurfaceDRCurrent.Text },
                    new() { Location = BlacknessLocationKind.INSIDE_OP, Pixels = textInsideOPPixels.Text, MeasuredValue = inputInsideOPCurrent.Text },
                    new() { Location = BlacknessLocationKind.INSIDE_CE, Pixels = textInsideCEPixels.Text, MeasuredValue = inputInsideCECurrent.Text },
                    new() { Location = BlacknessLocationKind.INSIDE_DR, Pixels = textInsideDRPixels.Text, MeasuredValue = inputInsideDRCurrent.Text },
                };
                var add = new CalculateScale
                {
                    Key = "Blackness",
                    Value = CaculateRatio(),
                    Unit = LocalizeHelper.LENGTH_SCALE_CACULATED_RATIO_UNIT,
                    ImagePath = imagePath,
                    Settings = JsonConvert.SerializeObject(items),
                    Creator = $"{apiBLL.LoginUserInfo.Username}-{apiBLL.LoginUserInfo.Nickname}",
                    CreateTime = DateTime.Now
                };
                var currentScale = blacknessMethodBLL.GetCurrentScale();
                if (currentScale != null && add.Equals(currentScale))
                {
                    throw new Exception(LocalizeHelper.NO_NEED_TO_SAVE_THE_SAME_SCALE);
                }
                blacknessMethodBLL.SaveScaleSetting(add);
                return add;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
