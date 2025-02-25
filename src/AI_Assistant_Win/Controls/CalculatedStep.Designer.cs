namespace AI_Assistant_Win
{
    partial class CalculatedStep
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            AntdUI.StepsItem stepsItem9 = new AntdUI.StepsItem();
            AntdUI.StepsItem stepsItem10 = new AntdUI.StepsItem();
            AntdUI.StepsItem stepsItem11 = new AntdUI.StepsItem();
            AntdUI.StepsItem stepsItem12 = new AntdUI.StepsItem();
            AntdUI.StepsItem stepsItem13 = new AntdUI.StepsItem();
            AntdUI.StepsItem stepsItem14 = new AntdUI.StepsItem();
            AntdUI.StepsItem stepsItem15 = new AntdUI.StepsItem();
            AntdUI.StepsItem stepsItem16 = new AntdUI.StepsItem();
            stepsCalculate = new AntdUI.Steps();
            avatarCurrent = new AntdUI.Avatar();
            avatar1 = new AntdUI.Avatar();
            avatar2 = new AntdUI.Avatar();
            avatar3 = new AntdUI.Avatar();
            avatar4 = new AntdUI.Avatar();
            avatar5 = new AntdUI.Avatar();
            avatar6 = new AntdUI.Avatar();
            avatar7 = new AntdUI.Avatar();
            avatar8 = new AntdUI.Avatar();
            panelCalculateStepResults = new AntdUI.Panel();
            labelConfidence = new AntdUI.Label();
            label14 = new AntdUI.Label();
            labelDistribution = new AntdUI.Label();
            label12 = new AntdUI.Label();
            labelUncertainty = new AntdUI.Label();
            label10 = new AntdUI.Label();
            labelStandardError = new AntdUI.Label();
            label8 = new AntdUI.Label();
            labelStandardDiviation = new AntdUI.Label();
            label6 = new AntdUI.Label();
            labelAverage = new AntdUI.Label();
            label4 = new AntdUI.Label();
            labelSame = new AntdUI.Label();
            label2 = new AntdUI.Label();
            labelMPE = new AntdUI.Label();
            label20 = new AntdUI.Label();
            panelCalculateStepResults.SuspendLayout();
            SuspendLayout();
            // 
            // stepsCalculate
            // 
            stepsCalculate.Current = -1;
            stepsCalculate.Dock = System.Windows.Forms.DockStyle.Left;
            stepsItem9.Title = "统计所有测量数据计算MPE";
            stepsItem10.Title = "重复测量n次，记录数据​";
            stepsItem11.Title = "计算平均值";
            stepsItem12.Title = "计算标准差";
            stepsItem13.Title = "计算标准误差";
            stepsItem14.Title = "合成不确定度";
            stepsItem15.Title = "计算置信概率";
            stepsItem16.Title = "最终结果表示";
            stepsCalculate.Items.Add(stepsItem9);
            stepsCalculate.Items.Add(stepsItem10);
            stepsCalculate.Items.Add(stepsItem11);
            stepsCalculate.Items.Add(stepsItem12);
            stepsCalculate.Items.Add(stepsItem13);
            stepsCalculate.Items.Add(stepsItem14);
            stepsCalculate.Items.Add(stepsItem15);
            stepsCalculate.Items.Add(stepsItem16);
            stepsCalculate.Location = new System.Drawing.Point(0, 0);
            stepsCalculate.Name = "stepsCalculate";
            stepsCalculate.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            stepsCalculate.Size = new System.Drawing.Size(300, 543);
            stepsCalculate.Status = AntdUI.TStepState.Finish;
            stepsCalculate.TabIndex = 33;
            stepsCalculate.Text = "steps4";
            stepsCalculate.Vertical = true;
            // 
            // avatarCurrent
            // 
            avatarCurrent.Image = Properties.Resources.average_formula;
            avatarCurrent.Location = new System.Drawing.Point(56, 179);
            avatarCurrent.Name = "avatarCurrent";
            avatarCurrent.Size = new System.Drawing.Size(100, 40);
            avatarCurrent.TabIndex = 34;
            // 
            // avatar1
            // 
            avatar1.Image = Properties.Resources.number_range;
            avatar1.Location = new System.Drawing.Point(63, 118);
            avatar1.Name = "avatar1";
            avatar1.Size = new System.Drawing.Size(116, 28);
            avatar1.TabIndex = 35;
            // 
            // avatar2
            // 
            avatar2.Image = Properties.Resources.standard_diviation_formula;
            avatar2.Location = new System.Drawing.Point(64, 243);
            avatar2.Name = "avatar2";
            avatar2.Size = new System.Drawing.Size(158, 45);
            avatar2.TabIndex = 36;
            // 
            // avatar3
            // 
            avatar3.Image = Properties.Resources.measure_error;
            avatar3.Location = new System.Drawing.Point(62, 46);
            avatar3.Name = "avatar3";
            avatar3.Size = new System.Drawing.Size(116, 18);
            avatar3.TabIndex = 37;
            // 
            // avatar4
            // 
            avatar4.Image = Properties.Resources.mpe;
            avatar4.Location = new System.Drawing.Point(65, 69);
            avatar4.Name = "avatar4";
            avatar4.Size = new System.Drawing.Size(196, 22);
            avatar4.TabIndex = 38;
            // 
            // avatar5
            // 
            avatar5.Image = Properties.Resources.standard_error_formula;
            avatar5.Location = new System.Drawing.Point(64, 310);
            avatar5.Name = "avatar5";
            avatar5.Size = new System.Drawing.Size(100, 45);
            avatar5.TabIndex = 39;
            // 
            // avatar6
            // 
            avatar6.Image = Properties.Resources.uncertanty_formula;
            avatar6.Location = new System.Drawing.Point(61, 378);
            avatar6.Name = "avatar6";
            avatar6.Size = new System.Drawing.Size(173, 45);
            avatar6.TabIndex = 40;
            // 
            // avatar7
            // 
            avatar7.Image = Properties.Resources.whole;
            avatar7.Location = new System.Drawing.Point(60, 512);
            avatar7.Name = "avatar7";
            avatar7.Size = new System.Drawing.Size(230, 20);
            avatar7.TabIndex = 41;
            // 
            // avatar8
            // 
            avatar8.Image = Properties.Resources.normal_distribution;
            avatar8.Location = new System.Drawing.Point(125, 445);
            avatar8.Name = "avatar8";
            avatar8.Size = new System.Drawing.Size(76, 45);
            avatar8.TabIndex = 42;
            // 
            // panelCalculateStepResults
            // 
            panelCalculateStepResults.ArrowSize = 10;
            panelCalculateStepResults.Controls.Add(labelConfidence);
            panelCalculateStepResults.Controls.Add(label14);
            panelCalculateStepResults.Controls.Add(labelDistribution);
            panelCalculateStepResults.Controls.Add(label12);
            panelCalculateStepResults.Controls.Add(labelUncertainty);
            panelCalculateStepResults.Controls.Add(label10);
            panelCalculateStepResults.Controls.Add(labelStandardError);
            panelCalculateStepResults.Controls.Add(label8);
            panelCalculateStepResults.Controls.Add(labelStandardDiviation);
            panelCalculateStepResults.Controls.Add(label6);
            panelCalculateStepResults.Controls.Add(labelAverage);
            panelCalculateStepResults.Controls.Add(label4);
            panelCalculateStepResults.Controls.Add(labelSame);
            panelCalculateStepResults.Controls.Add(label2);
            panelCalculateStepResults.Controls.Add(labelMPE);
            panelCalculateStepResults.Controls.Add(label20);
            panelCalculateStepResults.Dock = System.Windows.Forms.DockStyle.Right;
            panelCalculateStepResults.Location = new System.Drawing.Point(306, 0);
            panelCalculateStepResults.Name = "panelCalculateStepResults";
            panelCalculateStepResults.Padding = new System.Windows.Forms.Padding(7);
            panelCalculateStepResults.Radius = 0;
            panelCalculateStepResults.Shadow = 2;
            panelCalculateStepResults.ShadowOpacityAnimation = true;
            panelCalculateStepResults.Size = new System.Drawing.Size(344, 543);
            panelCalculateStepResults.TabIndex = 43;
            // 
            // labelConfidence
            // 
            labelConfidence.BackColor = System.Drawing.Color.Transparent;
            labelConfidence.Dock = System.Windows.Forms.DockStyle.Fill;
            labelConfidence.Enabled = false;
            labelConfidence.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            labelConfidence.ForeColor = System.Drawing.Color.Black;
            labelConfidence.LocalizationText = "";
            labelConfidence.Location = new System.Drawing.Point(9, 510);
            labelConfidence.Name = "labelConfidence";
            labelConfidence.Size = new System.Drawing.Size(326, 24);
            labelConfidence.TabIndex = 26;
            labelConfidence.Text = "±0.6052mm(k=1,68.27%)(实际88.89%)";
            labelConfidence.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label14
            // 
            label14.BackColor = System.Drawing.Color.Transparent;
            label14.Dock = System.Windows.Forms.DockStyle.Top;
            label14.Enabled = false;
            label14.Font = new System.Drawing.Font("Microsoft YaHei UI", 15F, System.Drawing.FontStyle.Bold);
            label14.ForeColor = System.Drawing.Color.DarkGray;
            label14.LocalizationText = "";
            label14.Location = new System.Drawing.Point(9, 493);
            label14.Name = "label14";
            label14.Size = new System.Drawing.Size(326, 17);
            label14.TabIndex = 25;
            label14.Text = "";
            label14.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // labelDistribution
            // 
            labelDistribution.BackColor = System.Drawing.Color.Transparent;
            labelDistribution.Dock = System.Windows.Forms.DockStyle.Top;
            labelDistribution.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            labelDistribution.ForeColor = System.Drawing.Color.Black;
            labelDistribution.LocalizationText = "";
            labelDistribution.Location = new System.Drawing.Point(9, 443);
            labelDistribution.Name = "labelDistribution";
            labelDistribution.Size = new System.Drawing.Size(326, 50);
            labelDistribution.TabIndex = 24;
            labelDistribution.Text = "数据分布比例：μ±1σ 内: 70.00% (理论68.27%)μ±2σ 内: 95.00% (理论95.45%)μ±3σ 内: 100.00% (理论99.73%)";
            labelDistribution.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label12
            // 
            label12.BackColor = System.Drawing.Color.Transparent;
            label12.Dock = System.Windows.Forms.DockStyle.Top;
            label12.Enabled = false;
            label12.Font = new System.Drawing.Font("Microsoft YaHei UI", 15F, System.Drawing.FontStyle.Bold);
            label12.ForeColor = System.Drawing.Color.DarkGray;
            label12.LocalizationText = "";
            label12.Location = new System.Drawing.Point(9, 426);
            label12.Name = "label12";
            label12.Size = new System.Drawing.Size(326, 17);
            label12.TabIndex = 23;
            label12.Text = "";
            label12.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // labelUncertainty
            // 
            labelUncertainty.BackColor = System.Drawing.Color.Transparent;
            labelUncertainty.Dock = System.Windows.Forms.DockStyle.Top;
            labelUncertainty.Font = new System.Drawing.Font("Microsoft YaHei UI", 15F);
            labelUncertainty.ForeColor = System.Drawing.Color.Black;
            labelUncertainty.LocalizationText = "";
            labelUncertainty.Location = new System.Drawing.Point(9, 376);
            labelUncertainty.Name = "labelUncertainty";
            labelUncertainty.Size = new System.Drawing.Size(326, 50);
            labelUncertainty.TabIndex = 22;
            labelUncertainty.Text = "0.003mm";
            labelUncertainty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            label10.BackColor = System.Drawing.Color.Transparent;
            label10.Dock = System.Windows.Forms.DockStyle.Top;
            label10.Enabled = false;
            label10.Font = new System.Drawing.Font("Microsoft YaHei UI", 15F, System.Drawing.FontStyle.Bold);
            label10.ForeColor = System.Drawing.Color.DarkGray;
            label10.LocalizationText = "";
            label10.Location = new System.Drawing.Point(9, 359);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(326, 17);
            label10.TabIndex = 21;
            label10.Text = "";
            label10.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // labelStandardError
            // 
            labelStandardError.BackColor = System.Drawing.Color.Transparent;
            labelStandardError.Dock = System.Windows.Forms.DockStyle.Top;
            labelStandardError.Font = new System.Drawing.Font("Microsoft YaHei UI", 15F);
            labelStandardError.ForeColor = System.Drawing.Color.Black;
            labelStandardError.LocalizationText = "";
            labelStandardError.Location = new System.Drawing.Point(9, 309);
            labelStandardError.Name = "labelStandardError";
            labelStandardError.Size = new System.Drawing.Size(326, 50);
            labelStandardError.TabIndex = 20;
            labelStandardError.Text = "0.0005mm";
            labelStandardError.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            label8.BackColor = System.Drawing.Color.Transparent;
            label8.Dock = System.Windows.Forms.DockStyle.Top;
            label8.Enabled = false;
            label8.Font = new System.Drawing.Font("Microsoft YaHei UI", 15F, System.Drawing.FontStyle.Bold);
            label8.ForeColor = System.Drawing.Color.DarkGray;
            label8.LocalizationText = "";
            label8.Location = new System.Drawing.Point(9, 292);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(326, 17);
            label8.TabIndex = 19;
            label8.Text = "";
            label8.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // labelStandardDiviation
            // 
            labelStandardDiviation.BackColor = System.Drawing.Color.Transparent;
            labelStandardDiviation.Dock = System.Windows.Forms.DockStyle.Top;
            labelStandardDiviation.Font = new System.Drawing.Font("Microsoft YaHei UI", 15F);
            labelStandardDiviation.ForeColor = System.Drawing.Color.Black;
            labelStandardDiviation.LocalizationText = "";
            labelStandardDiviation.Location = new System.Drawing.Point(9, 242);
            labelStandardDiviation.Name = "labelStandardDiviation";
            labelStandardDiviation.Size = new System.Drawing.Size(326, 50);
            labelStandardDiviation.TabIndex = 18;
            labelStandardDiviation.Text = "随机误差（标准差）：σ=0.0011mm";
            labelStandardDiviation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            label6.BackColor = System.Drawing.Color.Transparent;
            label6.Dock = System.Windows.Forms.DockStyle.Top;
            label6.Enabled = false;
            label6.Font = new System.Drawing.Font("Microsoft YaHei UI", 15F, System.Drawing.FontStyle.Bold);
            label6.ForeColor = System.Drawing.Color.DarkGray;
            label6.LocalizationText = "";
            label6.Location = new System.Drawing.Point(9, 225);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(326, 17);
            label6.TabIndex = 17;
            label6.Text = "";
            label6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // labelAverage
            // 
            labelAverage.BackColor = System.Drawing.Color.Transparent;
            labelAverage.Dock = System.Windows.Forms.DockStyle.Top;
            labelAverage.Font = new System.Drawing.Font("Microsoft YaHei UI", 15F);
            labelAverage.ForeColor = System.Drawing.Color.Black;
            labelAverage.LocalizationText = "";
            labelAverage.Location = new System.Drawing.Point(9, 175);
            labelAverage.Name = "labelAverage";
            labelAverage.Size = new System.Drawing.Size(326, 50);
            labelAverage.TabIndex = 16;
            labelAverage.Text = "10.0016mm";
            labelAverage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            label4.BackColor = System.Drawing.Color.Transparent;
            label4.Dock = System.Windows.Forms.DockStyle.Top;
            label4.Enabled = false;
            label4.Font = new System.Drawing.Font("Microsoft YaHei UI", 15F, System.Drawing.FontStyle.Bold);
            label4.ForeColor = System.Drawing.Color.DarkGray;
            label4.LocalizationText = "";
            label4.Location = new System.Drawing.Point(9, 158);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(326, 17);
            label4.TabIndex = 15;
            label4.Text = "";
            label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // labelSame
            // 
            labelSame.BackColor = System.Drawing.Color.Transparent;
            labelSame.Dock = System.Windows.Forms.DockStyle.Top;
            labelSame.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            labelSame.ForeColor = System.Drawing.Color.Black;
            labelSame.LocalizationText = "";
            labelSame.Location = new System.Drawing.Point(9, 108);
            labelSame.Name = "labelSame";
            labelSame.Size = new System.Drawing.Size(326, 50);
            labelSame.TabIndex = 14;
            labelSame.Text = "测量5次数据为：10.002mm, 10.001mm, 10.003mm, 10.000mm, 10.002mm。";
            // 
            // label2
            // 
            label2.BackColor = System.Drawing.Color.Transparent;
            label2.Dock = System.Windows.Forms.DockStyle.Top;
            label2.Enabled = false;
            label2.Font = new System.Drawing.Font("Microsoft YaHei UI", 15F, System.Drawing.FontStyle.Bold);
            label2.ForeColor = System.Drawing.Color.DarkGray;
            label2.LocalizationText = "";
            label2.Location = new System.Drawing.Point(9, 91);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(326, 17);
            label2.TabIndex = 13;
            label2.Text = "";
            label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // labelMPE
            // 
            labelMPE.BackColor = System.Drawing.Color.Transparent;
            labelMPE.Dock = System.Windows.Forms.DockStyle.Top;
            labelMPE.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            labelMPE.ForeColor = System.Drawing.Color.Black;
            labelMPE.LocalizationText = "";
            labelMPE.Location = new System.Drawing.Point(9, 39);
            labelMPE.Name = "labelMPE";
            labelMPE.Size = new System.Drawing.Size(326, 52);
            labelMPE.TabIndex = 12;
            labelMPE.Text = "最大允许误差(MPE, Maximum Permissible Error)是仪器或测量系统在特定条件下允许的最大误差值.则其MPE为 0.003mm。";
            // 
            // label20
            // 
            label20.BackColor = System.Drawing.Color.Transparent;
            label20.Dock = System.Windows.Forms.DockStyle.Top;
            label20.Font = new System.Drawing.Font("Microsoft YaHei UI", 15F, System.Drawing.FontStyle.Bold);
            label20.ForeColor = System.Drawing.Color.DarkGray;
            label20.LocalizationText = "";
            label20.Location = new System.Drawing.Point(9, 9);
            label20.Name = "label20";
            label20.Size = new System.Drawing.Size(326, 30);
            label20.TabIndex = 11;
            label20.Text = "";
            label20.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // CalculatedStep
            // 
            Controls.Add(panelCalculateStepResults);
            Controls.Add(avatar8);
            Controls.Add(avatar7);
            Controls.Add(avatar6);
            Controls.Add(avatar5);
            Controls.Add(avatar4);
            Controls.Add(avatar3);
            Controls.Add(avatar2);
            Controls.Add(avatar1);
            Controls.Add(avatarCurrent);
            Controls.Add(stepsCalculate);
            Name = "CalculatedStep";
            Size = new System.Drawing.Size(650, 543);
            panelCalculateStepResults.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.Steps stepsCalculate;
        private AntdUI.Avatar avatarCurrent;
        private AntdUI.Avatar avatar1;
        private AntdUI.Avatar avatar2;
        private AntdUI.Avatar avatar3;
        private AntdUI.Avatar avatar4;
        private AntdUI.Avatar avatar5;
        private AntdUI.Avatar avatar6;
        private AntdUI.Avatar avatar7;
        private AntdUI.Avatar avatar8;
        private AntdUI.Panel panelCalculateStepResults;
        private AntdUI.Label label20;
        private AntdUI.Label labelMPE;
        private AntdUI.Label label2;
        private AntdUI.Label label4;
        private AntdUI.Label labelSame;
        private AntdUI.Label label14;
        private AntdUI.Label labelDistribution;
        private AntdUI.Label label12;
        private AntdUI.Label labelUncertainty;
        private AntdUI.Label label10;
        private AntdUI.Label labelStandardError;
        private AntdUI.Label label8;
        private AntdUI.Label labelStandardDiviation;
        private AntdUI.Label label6;
        private AntdUI.Label labelAverage;
        private AntdUI.Label labelConfidence;
    }
}