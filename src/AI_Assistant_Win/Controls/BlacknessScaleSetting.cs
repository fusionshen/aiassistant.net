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
            // 调整表单大小以适应内容面板
            AdjustFormSizeToContent();
        }
        private void AdjustFormSizeToContent()
        {
            if (!panelAtThatTime.Visible)
            {
                this.ClientSize = new Size(this.Width / 2, this.Height);
            }
        }

        private string imagePath;
        public void SetCurrentScaleDetails(BlacknessResult blacknessResult)
        {
            var currentScale = blacknessMethodBLL.GetCurrentScale();
            if (currentScale == null)
            {
                imagePath = blacknessResult.RenderImagePath;
                avatarCurrent.Image = Image.FromFile(blacknessResult.RenderImagePath);
                textSurfaceOPPixels.Text = blacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_OP))?.Width.ToString("F2");
                textSurfaceCEPixels.Text = blacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_CE))?.Width.ToString("F2");
                textSurfaceDRPixels.Text = blacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.SURFACE_DR))?.Width.ToString("F2");
                textInsideOPPixels.Text = blacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_OP))?.Width.ToString("F2");
                textInsideCEPixels.Text = blacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_CE))?.Width.ToString("F2");
                textInsideDRPixels.Text = blacknessResult.Items.FirstOrDefault(t => t.Location.Equals(BlacknessLocationKind.INSIDE_DR))?.Width.ToString("F2");
            }
        }

        private void InputSurfaceOPCurrent_TextChanged(object sender, System.EventArgs e)
        {
            try
            {
                var result = CaculateRatio();
                labelRatio.Text = $"{LocalizeHelper.BLACKNESS_SCALE_CACULATED_RATIO_TITLE}{result:F2}{LocalizeHelper.BLACKNESS_SCALE_CACULATED_RATIO_UNIT}";
            }
            catch (Exception)
            {
                labelRatio.Text = LocalizeHelper.BLACKNESS_SCALE_INPUT_ERROR;
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
            var result = dics.Select(t => 100 * float.Parse(t.Value) / float.Parse(t.Key)).Average();
            return result;
        }

        private void InputSurfaceCECurrent_TextChanged(object sender, System.EventArgs e)
        {
            CaculateRatio();
        }

        private void InputSurfaceDRCurrent_TextChanged(object sender, System.EventArgs e)
        {
            CaculateRatio();
        }

        private void InputInsideOPCurrent_TextChanged(object sender, System.EventArgs e)
        {
            CaculateRatio();
        }

        private void InputInsideCECurrent_TextChanged(object sender, System.EventArgs e)
        {
            CaculateRatio();
        }

        private void InputInsideDRCurrent_TextChanged(object sender, System.EventArgs e)
        {
            CaculateRatio();
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
                    Unit = LocalizeHelper.BLACKNESS_SCALE_CACULATED_RATIO_UNIT,
                    ImagePath = imagePath,
                    Settings = JsonConvert.SerializeObject(items),
                    Creator = $"{apiBLL.LoginUserInfo.Username}-{apiBLL.LoginUserInfo.Nickname}",
                    CreateTime = DateTime.Now
                };
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
