using System.Drawing;
using System.Windows.Forms;

namespace AI_Assistant_Win.Controls
{
    partial class ScaleAccuracyReport
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScaleAccuracyReport));
            panel1 = new System.Windows.Forms.Panel();
            panel3 = new System.Windows.Forms.Panel();
            avatarErrorFormula = new AntdUI.Avatar();
            labelConfidence = new AntdUI.Label();
            label13 = new AntdUI.Label();
            avatarWholeFormula = new AntdUI.Avatar();
            avatarDistribution = new AntdUI.Avatar();
            avatarUncertaincy = new AntdUI.Avatar();
            avatarStandardError = new AntdUI.Avatar();
            avatarStandardDiviation = new AntdUI.Avatar();
            avatarAverage = new AntdUI.Avatar();
            avatarNumbers = new AntdUI.Avatar();
            avatarMPE = new AntdUI.Avatar();
            labelDistribution = new AntdUI.Label();
            label12 = new AntdUI.Label();
            labelUncertainty = new AntdUI.Label();
            label11 = new AntdUI.Label();
            labelStandardError = new AntdUI.Label();
            label10 = new AntdUI.Label();
            labelStandardDiviation = new AntdUI.Label();
            label7 = new AntdUI.Label();
            labelAverage = new AntdUI.Label();
            label6 = new AntdUI.Label();
            labelSame = new AntdUI.Label();
            label5 = new AntdUI.Label();
            labelMPE = new AntdUI.Label();
            stepsCalculate = new AntdUI.Steps();
            panel8 = new AntdUI.Panel();
            labelAreaScale = new AntdUI.Label();
            labelLengthScale = new AntdUI.Label();
            labelTopGrade = new AntdUI.Label();
            panel2 = new AntdUI.Panel();
            tablePanel = new TableLayoutPanel();
            textEdgeDAPixels = new AntdUI.Input();
            textEdgeCDPixels = new AntdUI.Input();
            textEdgeBCPixels = new AntdUI.Input();
            textEdgeABPixels = new AntdUI.Input();
            inputEdgeDALength = new AntdUI.Input();
            inputEdgeCDLength = new AntdUI.Input();
            inputEdgeBCLength = new AntdUI.Input();
            inputEdgeABLength = new AntdUI.Input();
            labelInsideOP = new AntdUI.Label();
            labelSurfaceDR = new AntdUI.Label();
            labelSurfaceCE = new AntdUI.Label();
            labelAB = new AntdUI.Label();
            avatarScaleRenderImage = new AntdUI.Avatar();
            panel7 = new AntdUI.Panel();
            label_Analyst = new Label();
            label4 = new Label();
            panel16 = new AntdUI.Panel();
            checkbox_Ding_Night = new AntdUI.Checkbox();
            button13 = new AntdUI.Button();
            panel15 = new AntdUI.Panel();
            checkbox_Ding_Day = new AntdUI.Checkbox();
            button12 = new AntdUI.Button();
            panel14 = new AntdUI.Panel();
            checkbox_Bing_Night = new AntdUI.Checkbox();
            button11 = new AntdUI.Button();
            panel13 = new AntdUI.Panel();
            checkbox_Bing_Day = new AntdUI.Checkbox();
            button10 = new AntdUI.Button();
            panel12 = new AntdUI.Panel();
            checkbox_Yi_Night = new AntdUI.Checkbox();
            button9 = new AntdUI.Button();
            panel11 = new AntdUI.Panel();
            checkbox_Yi_Day = new AntdUI.Checkbox();
            button8 = new AntdUI.Button();
            panel10 = new AntdUI.Panel();
            checkbox_Jia_Night = new AntdUI.Checkbox();
            button7 = new AntdUI.Button();
            panel9 = new AntdUI.Panel();
            checkbox_Jia_Day = new AntdUI.Checkbox();
            button6 = new AntdUI.Button();
            button4 = new AntdUI.Button();
            panel6 = new AntdUI.Panel();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            blacknessMethod_OriginImage_Zone = new Label();
            button3 = new AntdUI.Button();
            panel4 = new AntdUI.Panel();
            button2 = new AntdUI.Button();
            panel5 = new AntdUI.Panel();
            labelDate = new Label();
            labelTitle = new Label();
            printDocument1 = new System.Drawing.Printing.PrintDocument();
            printPreviewDialog1 = new PrintPreviewDialog();
            pageSetupDialog1 = new PageSetupDialog();
            panel1.SuspendLayout();
            panel3.SuspendLayout();
            panel8.SuspendLayout();
            panel2.SuspendLayout();
            tablePanel.SuspendLayout();
            panel7.SuspendLayout();
            panel16.SuspendLayout();
            panel15.SuspendLayout();
            panel14.SuspendLayout();
            panel13.SuspendLayout();
            panel12.SuspendLayout();
            panel11.SuspendLayout();
            panel10.SuspendLayout();
            panel9.SuspendLayout();
            panel6.SuspendLayout();
            panel4.SuspendLayout();
            panel5.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.AutoScroll = true;
            panel1.Controls.Add(panel3);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(420, 622);
            panel1.TabIndex = 6;
            // 
            // panel3
            // 
            panel3.AutoSize = true;
            panel3.Controls.Add(avatarErrorFormula);
            panel3.Controls.Add(labelConfidence);
            panel3.Controls.Add(label13);
            panel3.Controls.Add(avatarWholeFormula);
            panel3.Controls.Add(avatarDistribution);
            panel3.Controls.Add(avatarUncertaincy);
            panel3.Controls.Add(avatarStandardError);
            panel3.Controls.Add(avatarStandardDiviation);
            panel3.Controls.Add(avatarAverage);
            panel3.Controls.Add(avatarNumbers);
            panel3.Controls.Add(avatarMPE);
            panel3.Controls.Add(labelDistribution);
            panel3.Controls.Add(label12);
            panel3.Controls.Add(labelUncertainty);
            panel3.Controls.Add(label11);
            panel3.Controls.Add(labelStandardError);
            panel3.Controls.Add(label10);
            panel3.Controls.Add(labelStandardDiviation);
            panel3.Controls.Add(label7);
            panel3.Controls.Add(labelAverage);
            panel3.Controls.Add(label6);
            panel3.Controls.Add(labelSame);
            panel3.Controls.Add(label5);
            panel3.Controls.Add(labelMPE);
            panel3.Controls.Add(stepsCalculate);
            panel3.Controls.Add(panel8);
            panel3.Controls.Add(panel2);
            panel3.Controls.Add(panel7);
            panel3.Controls.Add(panel6);
            panel3.Controls.Add(panel4);
            panel3.Controls.Add(panel5);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(0, 0);
            panel3.Name = "panel3";
            panel3.Size = new Size(420, 622);
            panel3.TabIndex = 1;
            // 
            // avatarErrorFormula
            // 
            avatarErrorFormula.Image = Properties.Resources.measure_error;
            avatarErrorFormula.Location = new Point(56, 266);
            avatarErrorFormula.Name = "avatarErrorFormula";
            avatarErrorFormula.Size = new Size(102, 15);
            avatarErrorFormula.TabIndex = 34;
            // 
            // labelConfidence
            // 
            labelConfidence.BackColor = Color.Transparent;
            labelConfidence.Dock = DockStyle.Fill;
            labelConfidence.Enabled = false;
            labelConfidence.Font = new Font("Microsoft YaHei UI", 8F);
            labelConfidence.ForeColor = Color.Black;
            labelConfidence.LocalizationText = "";
            labelConfidence.Location = new Point(201, 618);
            labelConfidence.Name = "labelConfidence";
            labelConfidence.Size = new Size(219, 4);
            labelConfidence.TabIndex = 60;
            labelConfidence.Text = "±0.6052mm(k=1,68.27%)(实际88.89%)";
            labelConfidence.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label13
            // 
            label13.BackColor = Color.Transparent;
            label13.Dock = DockStyle.Top;
            label13.Font = new Font("Microsoft YaHei UI", 15F, FontStyle.Bold);
            label13.ForeColor = Color.DarkGray;
            label13.LocalizationText = "";
            label13.Location = new Point(201, 608);
            label13.Name = "label13";
            label13.Size = new Size(219, 10);
            label13.TabIndex = 59;
            label13.Text = "";
            label13.TextAlign = ContentAlignment.TopCenter;
            // 
            // avatarWholeFormula
            // 
            avatarWholeFormula.Image = Properties.Resources.whole;
            avatarWholeFormula.Location = new Point(47, 601);
            avatarWholeFormula.Name = "avatarWholeFormula";
            avatarWholeFormula.Size = new Size(142, 17);
            avatarWholeFormula.TabIndex = 34;
            // 
            // avatarDistribution
            // 
            avatarDistribution.Image = Properties.Resources.normal_distribution;
            avatarDistribution.Location = new Point(123, 548);
            avatarDistribution.Name = "avatarDistribution";
            avatarDistribution.Size = new Size(48, 39);
            avatarDistribution.TabIndex = 34;
            // 
            // avatarUncertaincy
            // 
            avatarUncertaincy.Image = Properties.Resources.uncertanty_formula;
            avatarUncertaincy.Location = new Point(106, 509);
            avatarUncertaincy.Name = "avatarUncertaincy";
            avatarUncertaincy.Size = new Size(95, 22);
            avatarUncertaincy.TabIndex = 34;
            // 
            // avatarStandardError
            // 
            avatarStandardError.Image = Properties.Resources.standard_error_formula;
            avatarStandardError.Location = new Point(92, 462);
            avatarStandardError.Name = "avatarStandardError";
            avatarStandardError.Size = new Size(63, 21);
            avatarStandardError.TabIndex = 34;
            // 
            // avatarStandardDiviation
            // 
            avatarStandardDiviation.Image = Properties.Resources.standard_diviation_formula;
            avatarStandardDiviation.Location = new Point(91, 414);
            avatarStandardDiviation.Name = "avatarStandardDiviation";
            avatarStandardDiviation.Size = new Size(87, 21);
            avatarStandardDiviation.TabIndex = 34;
            // 
            // avatarAverage
            // 
            avatarAverage.Image = Properties.Resources.average_formula;
            avatarAverage.Location = new Point(101, 358);
            avatarAverage.Name = "avatarAverage";
            avatarAverage.Size = new Size(62, 37);
            avatarAverage.TabIndex = 34;
            // 
            // avatarNumbers
            // 
            avatarNumbers.Image = Properties.Resources.number_range;
            avatarNumbers.Location = new Point(90, 318);
            avatarNumbers.Name = "avatarNumbers";
            avatarNumbers.Size = new Size(90, 20);
            avatarNumbers.TabIndex = 34;
            // 
            // avatarMPE
            // 
            avatarMPE.Image = Properties.Resources.mpe;
            avatarMPE.Location = new Point(33, 280);
            avatarMPE.Name = "avatarMPE";
            avatarMPE.Size = new Size(160, 15);
            avatarMPE.TabIndex = 34;
            // 
            // labelDistribution
            // 
            labelDistribution.BackColor = Color.Transparent;
            labelDistribution.Dock = DockStyle.Top;
            labelDistribution.Font = new Font("Microsoft YaHei UI", 8F);
            labelDistribution.ForeColor = Color.Black;
            labelDistribution.LocalizationText = "";
            labelDistribution.Location = new Point(201, 568);
            labelDistribution.Name = "labelDistribution";
            labelDistribution.Size = new Size(219, 40);
            labelDistribution.TabIndex = 48;
            labelDistribution.Text = "数据分布比例：μ±1σ 内: 70.00% (理论68.27%)μ±2σ 内: 95.00% (理论95.45%)μ±3σ 内: 100.00% (理论99.73%)";
            labelDistribution.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label12
            // 
            label12.BackColor = Color.Transparent;
            label12.Dock = DockStyle.Top;
            label12.Font = new Font("Microsoft YaHei UI", 15F, FontStyle.Bold);
            label12.ForeColor = Color.DarkGray;
            label12.LocalizationText = "";
            label12.Location = new Point(201, 558);
            label12.Name = "label12";
            label12.Size = new Size(219, 10);
            label12.TabIndex = 47;
            label12.Text = "";
            label12.TextAlign = ContentAlignment.TopCenter;
            // 
            // labelUncertainty
            // 
            labelUncertainty.BackColor = Color.Transparent;
            labelUncertainty.Dock = DockStyle.Top;
            labelUncertainty.Font = new Font("Microsoft YaHei UI", 10F);
            labelUncertainty.ForeColor = Color.Black;
            labelUncertainty.LocalizationText = "";
            labelUncertainty.Location = new Point(201, 518);
            labelUncertainty.Name = "labelUncertainty";
            labelUncertainty.Size = new Size(219, 40);
            labelUncertainty.TabIndex = 46;
            labelUncertainty.Text = "0.003mm";
            labelUncertainty.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label11
            // 
            label11.BackColor = Color.Transparent;
            label11.Dock = DockStyle.Top;
            label11.Font = new Font("Microsoft YaHei UI", 15F, FontStyle.Bold);
            label11.ForeColor = Color.DarkGray;
            label11.LocalizationText = "";
            label11.Location = new Point(201, 508);
            label11.Name = "label11";
            label11.Size = new Size(219, 10);
            label11.TabIndex = 45;
            label11.Text = "";
            label11.TextAlign = ContentAlignment.TopCenter;
            // 
            // labelStandardError
            // 
            labelStandardError.BackColor = Color.Transparent;
            labelStandardError.Dock = DockStyle.Top;
            labelStandardError.Font = new Font("Microsoft YaHei UI", 10F);
            labelStandardError.ForeColor = Color.Black;
            labelStandardError.LocalizationText = "";
            labelStandardError.Location = new Point(201, 468);
            labelStandardError.Name = "labelStandardError";
            labelStandardError.Size = new Size(219, 40);
            labelStandardError.TabIndex = 44;
            labelStandardError.Text = "0.0005mm";
            labelStandardError.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            label10.BackColor = Color.Transparent;
            label10.Dock = DockStyle.Top;
            label10.Font = new Font("Microsoft YaHei UI", 15F, FontStyle.Bold);
            label10.ForeColor = Color.DarkGray;
            label10.LocalizationText = "";
            label10.Location = new Point(201, 458);
            label10.Name = "label10";
            label10.Size = new Size(219, 10);
            label10.TabIndex = 43;
            label10.Text = "";
            label10.TextAlign = ContentAlignment.TopCenter;
            // 
            // labelStandardDiviation
            // 
            labelStandardDiviation.BackColor = Color.Transparent;
            labelStandardDiviation.Dock = DockStyle.Top;
            labelStandardDiviation.Font = new Font("Microsoft YaHei UI", 10F);
            labelStandardDiviation.ForeColor = Color.Black;
            labelStandardDiviation.LocalizationText = "";
            labelStandardDiviation.Location = new Point(201, 418);
            labelStandardDiviation.Name = "labelStandardDiviation";
            labelStandardDiviation.Size = new Size(219, 40);
            labelStandardDiviation.TabIndex = 42;
            labelStandardDiviation.Text = "随机误差（标准差）：σ=0.0011mm";
            labelStandardDiviation.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            label7.BackColor = Color.Transparent;
            label7.Dock = DockStyle.Top;
            label7.Font = new Font("Microsoft YaHei UI", 15F, FontStyle.Bold);
            label7.ForeColor = Color.DarkGray;
            label7.LocalizationText = "";
            label7.Location = new Point(201, 408);
            label7.Name = "label7";
            label7.Size = new Size(219, 10);
            label7.TabIndex = 41;
            label7.Text = "";
            label7.TextAlign = ContentAlignment.TopCenter;
            // 
            // labelAverage
            // 
            labelAverage.BackColor = Color.Transparent;
            labelAverage.Dock = DockStyle.Top;
            labelAverage.Font = new Font("Microsoft YaHei UI", 10F);
            labelAverage.ForeColor = Color.Black;
            labelAverage.LocalizationText = "";
            labelAverage.Location = new Point(201, 368);
            labelAverage.Name = "labelAverage";
            labelAverage.Size = new Size(219, 40);
            labelAverage.TabIndex = 40;
            labelAverage.Text = "10.0016mm";
            labelAverage.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            label6.BackColor = Color.Transparent;
            label6.Dock = DockStyle.Top;
            label6.Font = new Font("Microsoft YaHei UI", 15F, FontStyle.Bold);
            label6.ForeColor = Color.DarkGray;
            label6.LocalizationText = "";
            label6.Location = new Point(201, 358);
            label6.Name = "label6";
            label6.Size = new Size(219, 10);
            label6.TabIndex = 39;
            label6.Text = "";
            label6.TextAlign = ContentAlignment.TopCenter;
            // 
            // labelSame
            // 
            labelSame.BackColor = Color.Transparent;
            labelSame.Dock = DockStyle.Top;
            labelSame.Font = new Font("Microsoft YaHei UI", 8F);
            labelSame.ForeColor = Color.Black;
            labelSame.LocalizationText = "";
            labelSame.Location = new Point(201, 318);
            labelSame.Name = "labelSame";
            labelSame.Size = new Size(219, 40);
            labelSame.TabIndex = 38;
            labelSame.Text = "测量5次数据为：10.002mm, 10.001mm, 10.003mm, 10.000mm, 10.002mm。";
            // 
            // label5
            // 
            label5.BackColor = Color.Transparent;
            label5.Dock = DockStyle.Top;
            label5.Font = new Font("Microsoft YaHei UI", 15F, FontStyle.Bold);
            label5.ForeColor = Color.DarkGray;
            label5.LocalizationText = "";
            label5.Location = new Point(201, 308);
            label5.Name = "label5";
            label5.Size = new Size(219, 10);
            label5.TabIndex = 37;
            label5.Text = "";
            label5.TextAlign = ContentAlignment.TopCenter;
            // 
            // labelMPE
            // 
            labelMPE.BackColor = Color.Transparent;
            labelMPE.Dock = DockStyle.Top;
            labelMPE.Font = new Font("Microsoft YaHei UI", 8F);
            labelMPE.ForeColor = Color.Black;
            labelMPE.LocalizationText = "";
            labelMPE.Location = new Point(201, 233);
            labelMPE.Name = "labelMPE";
            labelMPE.Size = new Size(219, 75);
            labelMPE.TabIndex = 36;
            labelMPE.Text = "最大允许误差(MPE, Maximum Permissible Error)是仪器或测量系统在特定条件下允许的最大误差值.则其MPE为 0.003mm。";
            // 
            // stepsCalculate
            // 
            stepsCalculate.Current = -1;
            stepsCalculate.Dock = DockStyle.Left;
            stepsCalculate.Font = new Font("Microsoft YaHei UI", 8F);
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
            stepsCalculate.Location = new Point(0, 233);
            stepsCalculate.Name = "stepsCalculate";
            stepsCalculate.Padding = new Padding(10, 0, 0, 0);
            stepsCalculate.Size = new Size(201, 389);
            stepsCalculate.Status = AntdUI.TStepState.Finish;
            stepsCalculate.TabIndex = 34;
            stepsCalculate.Text = "steps4";
            stepsCalculate.Vertical = true;
            // 
            // panel8
            // 
            panel8.Back = Color.Transparent;
            panel8.BackColor = Color.Transparent;
            panel8.Controls.Add(labelAreaScale);
            panel8.Controls.Add(labelLengthScale);
            panel8.Controls.Add(labelTopGrade);
            panel8.Dock = DockStyle.Top;
            panel8.Location = new Point(0, 200);
            panel8.Name = "panel8";
            panel8.Radius = 0;
            panel8.Size = new Size(420, 33);
            panel8.TabIndex = 17;
            // 
            // labelAreaScale
            // 
            labelAreaScale.BackColor = Color.Transparent;
            labelAreaScale.Dock = DockStyle.Fill;
            labelAreaScale.Font = new Font("Microsoft YaHei UI", 10F);
            labelAreaScale.LocalizationText = "";
            labelAreaScale.Location = new Point(253, 0);
            labelAreaScale.Name = "labelAreaScale";
            labelAreaScale.Size = new Size(167, 33);
            labelAreaScale.TabIndex = 18;
            labelAreaScale.Text = "0.2244平方毫米/像素面积";
            labelAreaScale.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // labelLengthScale
            // 
            labelLengthScale.BackColor = Color.Transparent;
            labelLengthScale.Dock = DockStyle.Left;
            labelLengthScale.Font = new Font("Microsoft YaHei UI", 10F);
            labelLengthScale.LocalizationText = "";
            labelLengthScale.Location = new Point(100, 0);
            labelLengthScale.Name = "labelLengthScale";
            labelLengthScale.Size = new Size(153, 33);
            labelLengthScale.TabIndex = 17;
            labelLengthScale.Text = "0.15毫米/像素边长";
            labelLengthScale.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // labelTopGrade
            // 
            labelTopGrade.BackColor = Color.Transparent;
            labelTopGrade.Dock = DockStyle.Left;
            labelTopGrade.Font = new Font("Microsoft YaHei UI", 10F);
            labelTopGrade.LocalizationText = "";
            labelTopGrade.Location = new Point(0, 0);
            labelTopGrade.Name = "labelTopGrade";
            labelTopGrade.Size = new Size(100, 33);
            labelTopGrade.TabIndex = 16;
            labelTopGrade.Text = "上表面刻度：0";
            labelTopGrade.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            panel2.Back = Color.Transparent;
            panel2.BackColor = Color.Transparent;
            panel2.Controls.Add(tablePanel);
            panel2.Controls.Add(avatarScaleRenderImage);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 71);
            panel2.Name = "panel2";
            panel2.Radius = 0;
            panel2.Size = new Size(420, 129);
            panel2.TabIndex = 16;
            // 
            // tablePanel
            // 
            tablePanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tablePanel.ColumnCount = 3;
            tablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tablePanel.Controls.Add(textEdgeDAPixels, 1, 3);
            tablePanel.Controls.Add(textEdgeCDPixels, 1, 2);
            tablePanel.Controls.Add(textEdgeBCPixels, 1, 1);
            tablePanel.Controls.Add(textEdgeABPixels, 1, 0);
            tablePanel.Controls.Add(inputEdgeDALength, 2, 3);
            tablePanel.Controls.Add(inputEdgeCDLength, 2, 2);
            tablePanel.Controls.Add(inputEdgeBCLength, 2, 1);
            tablePanel.Controls.Add(inputEdgeABLength, 2, 0);
            tablePanel.Controls.Add(labelInsideOP, 0, 3);
            tablePanel.Controls.Add(labelSurfaceDR, 0, 2);
            tablePanel.Controls.Add(labelSurfaceCE, 0, 1);
            tablePanel.Controls.Add(labelAB, 0, 0);
            tablePanel.Dock = DockStyle.Fill;
            tablePanel.Location = new Point(200, 0);
            tablePanel.Name = "tablePanel";
            tablePanel.RowCount = 4;
            tablePanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tablePanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tablePanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tablePanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tablePanel.Size = new Size(220, 129);
            tablePanel.TabIndex = 19;
            // 
            // textEdgeDAPixels
            // 
            textEdgeDAPixels.Dock = DockStyle.Fill;
            textEdgeDAPixels.Font = new Font("Microsoft YaHei UI", 10F);
            textEdgeDAPixels.LocalizationPlaceholderText = "";
            textEdgeDAPixels.Location = new Point(45, 94);
            textEdgeDAPixels.Margin = new Padding(0);
            textEdgeDAPixels.Name = "textEdgeDAPixels";
            textEdgeDAPixels.PlaceholderText = "";
            textEdgeDAPixels.Radius = 0;
            textEdgeDAPixels.ReadOnly = true;
            textEdgeDAPixels.Size = new Size(86, 34);
            textEdgeDAPixels.TabIndex = 41;
            // 
            // textEdgeCDPixels
            // 
            textEdgeCDPixels.Dock = DockStyle.Fill;
            textEdgeCDPixels.Font = new Font("Microsoft YaHei UI", 10F);
            textEdgeCDPixels.LocalizationPlaceholderText = "";
            textEdgeCDPixels.Location = new Point(45, 63);
            textEdgeCDPixels.Margin = new Padding(0);
            textEdgeCDPixels.Name = "textEdgeCDPixels";
            textEdgeCDPixels.PlaceholderText = "";
            textEdgeCDPixels.Radius = 0;
            textEdgeCDPixels.ReadOnly = true;
            textEdgeCDPixels.Size = new Size(86, 30);
            textEdgeCDPixels.TabIndex = 40;
            // 
            // textEdgeBCPixels
            // 
            textEdgeBCPixels.Dock = DockStyle.Fill;
            textEdgeBCPixels.Font = new Font("Microsoft YaHei UI", 10F);
            textEdgeBCPixels.LocalizationPlaceholderText = "";
            textEdgeBCPixels.Location = new Point(45, 32);
            textEdgeBCPixels.Margin = new Padding(0);
            textEdgeBCPixels.Name = "textEdgeBCPixels";
            textEdgeBCPixels.PlaceholderText = "";
            textEdgeBCPixels.Radius = 0;
            textEdgeBCPixels.ReadOnly = true;
            textEdgeBCPixels.Size = new Size(86, 30);
            textEdgeBCPixels.TabIndex = 39;
            // 
            // textEdgeABPixels
            // 
            textEdgeABPixels.Dock = DockStyle.Fill;
            textEdgeABPixels.Font = new Font("Microsoft YaHei UI", 10F);
            textEdgeABPixels.LocalizationPlaceholderText = "";
            textEdgeABPixels.Location = new Point(45, 1);
            textEdgeABPixels.Margin = new Padding(0);
            textEdgeABPixels.Name = "textEdgeABPixels";
            textEdgeABPixels.PlaceholderText = "";
            textEdgeABPixels.Radius = 0;
            textEdgeABPixels.ReadOnly = true;
            textEdgeABPixels.Size = new Size(86, 30);
            textEdgeABPixels.TabIndex = 38;
            // 
            // inputEdgeDALength
            // 
            inputEdgeDALength.Dock = DockStyle.Fill;
            inputEdgeDALength.Font = new Font("Microsoft YaHei UI", 10F);
            inputEdgeDALength.LocalizationPlaceholderText = "Measured value";
            inputEdgeDALength.Location = new Point(132, 94);
            inputEdgeDALength.Margin = new Padding(0);
            inputEdgeDALength.Name = "inputEdgeDALength";
            inputEdgeDALength.PlaceholderText = "请输入实际值";
            inputEdgeDALength.Radius = 0;
            inputEdgeDALength.ReadOnly = true;
            inputEdgeDALength.Size = new Size(87, 34);
            inputEdgeDALength.TabIndex = 37;
            inputEdgeDALength.Text = "0";
            // 
            // inputEdgeCDLength
            // 
            inputEdgeCDLength.Dock = DockStyle.Fill;
            inputEdgeCDLength.Font = new Font("Microsoft YaHei UI", 10F);
            inputEdgeCDLength.LocalizationPlaceholderText = "Measured value";
            inputEdgeCDLength.Location = new Point(132, 63);
            inputEdgeCDLength.Margin = new Padding(0);
            inputEdgeCDLength.Name = "inputEdgeCDLength";
            inputEdgeCDLength.PlaceholderText = "请输入实际值";
            inputEdgeCDLength.Radius = 0;
            inputEdgeCDLength.ReadOnly = true;
            inputEdgeCDLength.Size = new Size(87, 30);
            inputEdgeCDLength.TabIndex = 36;
            inputEdgeCDLength.Text = "0";
            // 
            // inputEdgeBCLength
            // 
            inputEdgeBCLength.Dock = DockStyle.Fill;
            inputEdgeBCLength.Font = new Font("Microsoft YaHei UI", 10F);
            inputEdgeBCLength.LocalizationPlaceholderText = "Measured value";
            inputEdgeBCLength.Location = new Point(132, 32);
            inputEdgeBCLength.Margin = new Padding(0);
            inputEdgeBCLength.Name = "inputEdgeBCLength";
            inputEdgeBCLength.PlaceholderText = "请输入实际值";
            inputEdgeBCLength.Radius = 0;
            inputEdgeBCLength.ReadOnly = true;
            inputEdgeBCLength.Size = new Size(87, 30);
            inputEdgeBCLength.TabIndex = 35;
            inputEdgeBCLength.Text = "0";
            // 
            // inputEdgeABLength
            // 
            inputEdgeABLength.Dock = DockStyle.Fill;
            inputEdgeABLength.Font = new Font("Microsoft YaHei UI", 10F);
            inputEdgeABLength.LocalizationPlaceholderText = "Measured value";
            inputEdgeABLength.Location = new Point(132, 1);
            inputEdgeABLength.Margin = new Padding(0);
            inputEdgeABLength.Name = "inputEdgeABLength";
            inputEdgeABLength.PlaceholderText = "请输入实际值";
            inputEdgeABLength.Radius = 0;
            inputEdgeABLength.ReadOnly = true;
            inputEdgeABLength.Size = new Size(87, 30);
            inputEdgeABLength.TabIndex = 34;
            inputEdgeABLength.Text = "0";
            // 
            // labelInsideOP
            // 
            labelInsideOP.BackColor = Color.Transparent;
            labelInsideOP.Dock = DockStyle.Fill;
            labelInsideOP.Font = new Font("Microsoft YaHei UI", 10F);
            labelInsideOP.LocalizationText = "DA";
            labelInsideOP.Location = new Point(4, 97);
            labelInsideOP.Name = "labelInsideOP";
            labelInsideOP.Size = new Size(37, 28);
            labelInsideOP.TabIndex = 26;
            labelInsideOP.Text = "DA";
            labelInsideOP.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // labelSurfaceDR
            // 
            labelSurfaceDR.BackColor = Color.Transparent;
            labelSurfaceDR.Dock = DockStyle.Fill;
            labelSurfaceDR.Font = new Font("Microsoft YaHei UI", 10F);
            labelSurfaceDR.LocalizationText = "CD";
            labelSurfaceDR.Location = new Point(4, 66);
            labelSurfaceDR.Name = "labelSurfaceDR";
            labelSurfaceDR.Size = new Size(37, 24);
            labelSurfaceDR.TabIndex = 23;
            labelSurfaceDR.Text = "CD";
            labelSurfaceDR.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // labelSurfaceCE
            // 
            labelSurfaceCE.BackColor = Color.Transparent;
            labelSurfaceCE.Dock = DockStyle.Fill;
            labelSurfaceCE.Font = new Font("Microsoft YaHei UI", 10F);
            labelSurfaceCE.LocalizationText = "BC";
            labelSurfaceCE.Location = new Point(4, 35);
            labelSurfaceCE.Name = "labelSurfaceCE";
            labelSurfaceCE.Size = new Size(37, 24);
            labelSurfaceCE.TabIndex = 20;
            labelSurfaceCE.Text = "BC";
            labelSurfaceCE.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // labelAB
            // 
            labelAB.BackColor = Color.Transparent;
            labelAB.Dock = DockStyle.Fill;
            labelAB.Font = new Font("Microsoft YaHei UI", 10F);
            labelAB.LocalizationText = "AB";
            labelAB.Location = new Point(4, 4);
            labelAB.Name = "labelAB";
            labelAB.Size = new Size(37, 24);
            labelAB.TabIndex = 17;
            labelAB.Text = "AB";
            labelAB.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // avatarScaleRenderImage
            // 
            avatarScaleRenderImage.Dock = DockStyle.Left;
            avatarScaleRenderImage.Image = Properties.Resources.gauge_template;
            avatarScaleRenderImage.Location = new Point(0, 0);
            avatarScaleRenderImage.Name = "avatarScaleRenderImage";
            avatarScaleRenderImage.Radius = 6;
            avatarScaleRenderImage.Size = new Size(200, 129);
            avatarScaleRenderImage.TabIndex = 14;
            // 
            // panel7
            // 
            panel7.Controls.Add(label_Analyst);
            panel7.Controls.Add(label4);
            panel7.Controls.Add(panel16);
            panel7.Controls.Add(panel15);
            panel7.Controls.Add(panel14);
            panel7.Controls.Add(panel13);
            panel7.Controls.Add(panel12);
            panel7.Controls.Add(panel11);
            panel7.Controls.Add(panel10);
            panel7.Controls.Add(panel9);
            panel7.Controls.Add(button4);
            panel7.Dock = DockStyle.Top;
            panel7.Location = new Point(0, 53);
            panel7.Name = "panel7";
            panel7.Radius = 0;
            panel7.Size = new Size(420, 18);
            panel7.TabIndex = 7;
            panel7.Text = "panel7";
            // 
            // label_Analyst
            // 
            label_Analyst.BackColor = Color.Transparent;
            label_Analyst.Dock = DockStyle.Left;
            label_Analyst.Font = new Font("Microsoft YaHei UI", 7F, FontStyle.Underline);
            label_Analyst.Location = new Point(367, 0);
            label_Analyst.Margin = new Padding(0);
            label_Analyst.Name = "label_Analyst";
            label_Analyst.Size = new Size(50, 18);
            label_Analyst.TabIndex = 18;
            label_Analyst.Text = " 张  三   ";
            label_Analyst.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            label4.BackColor = Color.Transparent;
            label4.Dock = DockStyle.Left;
            label4.Font = new Font("Microsoft YaHei UI", 7F);
            label4.Location = new Point(320, 0);
            label4.Margin = new Padding(0);
            label4.Name = "label4";
            label4.Size = new Size(47, 18);
            label4.TabIndex = 17;
            label4.Text = "分析人：";
            label4.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel16
            // 
            panel16.BorderColor = Color.Black;
            panel16.BorderWidth = 1F;
            panel16.Controls.Add(checkbox_Ding_Night);
            panel16.Controls.Add(button13);
            panel16.Dock = DockStyle.Left;
            panel16.Location = new Point(280, 0);
            panel16.Name = "panel16";
            panel16.Radius = 0;
            panel16.Size = new Size(40, 18);
            panel16.TabIndex = 16;
            panel16.Text = "panel16";
            // 
            // checkbox_Ding_Night
            // 
            checkbox_Ding_Night.AutoCheck = true;
            checkbox_Ding_Night.BackColor = Color.Transparent;
            checkbox_Ding_Night.Dock = DockStyle.Left;
            checkbox_Ding_Night.Fill = Color.FromArgb(200, 0, 0);
            checkbox_Ding_Night.Font = new Font("Microsoft YaHei UI", 8F);
            checkbox_Ding_Night.Location = new Point(1, 1);
            checkbox_Ding_Night.Name = "checkbox_Ding_Night";
            checkbox_Ding_Night.RightToLeft = RightToLeft.Yes;
            checkbox_Ding_Night.Size = new Size(40, 16);
            checkbox_Ding_Night.TabIndex = 3;
            checkbox_Ding_Night.Text = "夜";
            checkbox_Ding_Night.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // button13
            // 
            button13.Location = new Point(0, 0);
            button13.Name = "button13";
            button13.Size = new Size(0, 0);
            button13.TabIndex = 0;
            // 
            // panel15
            // 
            panel15.BorderColor = Color.Black;
            panel15.BorderWidth = 1F;
            panel15.Controls.Add(checkbox_Ding_Day);
            panel15.Controls.Add(button12);
            panel15.Dock = DockStyle.Left;
            panel15.Location = new Point(240, 0);
            panel15.Name = "panel15";
            panel15.Radius = 0;
            panel15.Size = new Size(40, 18);
            panel15.TabIndex = 15;
            panel15.Text = "panel15";
            // 
            // checkbox_Ding_Day
            // 
            checkbox_Ding_Day.AutoCheck = true;
            checkbox_Ding_Day.BackColor = Color.Transparent;
            checkbox_Ding_Day.Dock = DockStyle.Left;
            checkbox_Ding_Day.Fill = Color.FromArgb(200, 0, 0);
            checkbox_Ding_Day.Font = new Font("Microsoft YaHei UI", 8F);
            checkbox_Ding_Day.Location = new Point(1, 1);
            checkbox_Ding_Day.Name = "checkbox_Ding_Day";
            checkbox_Ding_Day.RightToLeft = RightToLeft.Yes;
            checkbox_Ding_Day.Size = new Size(40, 16);
            checkbox_Ding_Day.TabIndex = 3;
            checkbox_Ding_Day.Text = "白   ";
            checkbox_Ding_Day.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // button12
            // 
            button12.Location = new Point(0, 0);
            button12.Name = "button12";
            button12.Size = new Size(0, 0);
            button12.TabIndex = 0;
            // 
            // panel14
            // 
            panel14.BorderColor = Color.Black;
            panel14.BorderWidth = 1F;
            panel14.Controls.Add(checkbox_Bing_Night);
            panel14.Controls.Add(button11);
            panel14.Dock = DockStyle.Left;
            panel14.Location = new Point(200, 0);
            panel14.Name = "panel14";
            panel14.Radius = 0;
            panel14.Size = new Size(40, 18);
            panel14.TabIndex = 14;
            panel14.Text = "panel14";
            // 
            // checkbox_Bing_Night
            // 
            checkbox_Bing_Night.AutoCheck = true;
            checkbox_Bing_Night.BackColor = Color.Transparent;
            checkbox_Bing_Night.Dock = DockStyle.Left;
            checkbox_Bing_Night.Fill = Color.FromArgb(200, 0, 0);
            checkbox_Bing_Night.Font = new Font("Microsoft YaHei UI", 8F);
            checkbox_Bing_Night.Location = new Point(1, 1);
            checkbox_Bing_Night.Name = "checkbox_Bing_Night";
            checkbox_Bing_Night.RightToLeft = RightToLeft.Yes;
            checkbox_Bing_Night.Size = new Size(40, 16);
            checkbox_Bing_Night.TabIndex = 3;
            checkbox_Bing_Night.Text = "夜";
            checkbox_Bing_Night.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // button11
            // 
            button11.Location = new Point(0, 0);
            button11.Name = "button11";
            button11.Size = new Size(0, 0);
            button11.TabIndex = 0;
            // 
            // panel13
            // 
            panel13.BorderColor = Color.Black;
            panel13.BorderWidth = 1F;
            panel13.Controls.Add(checkbox_Bing_Day);
            panel13.Controls.Add(button10);
            panel13.Dock = DockStyle.Left;
            panel13.Location = new Point(160, 0);
            panel13.Name = "panel13";
            panel13.Radius = 0;
            panel13.Size = new Size(40, 18);
            panel13.TabIndex = 13;
            panel13.Text = "panel13";
            // 
            // checkbox_Bing_Day
            // 
            checkbox_Bing_Day.AutoCheck = true;
            checkbox_Bing_Day.BackColor = Color.Transparent;
            checkbox_Bing_Day.Dock = DockStyle.Left;
            checkbox_Bing_Day.Fill = Color.FromArgb(200, 0, 0);
            checkbox_Bing_Day.Font = new Font("Microsoft YaHei UI", 8F);
            checkbox_Bing_Day.Location = new Point(1, 1);
            checkbox_Bing_Day.Name = "checkbox_Bing_Day";
            checkbox_Bing_Day.RightToLeft = RightToLeft.Yes;
            checkbox_Bing_Day.Size = new Size(40, 16);
            checkbox_Bing_Day.TabIndex = 3;
            checkbox_Bing_Day.Text = "白   ";
            checkbox_Bing_Day.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // button10
            // 
            button10.Location = new Point(0, 0);
            button10.Name = "button10";
            button10.Size = new Size(0, 0);
            button10.TabIndex = 0;
            // 
            // panel12
            // 
            panel12.BorderColor = Color.Black;
            panel12.BorderWidth = 1F;
            panel12.Controls.Add(checkbox_Yi_Night);
            panel12.Controls.Add(button9);
            panel12.Dock = DockStyle.Left;
            panel12.Location = new Point(120, 0);
            panel12.Name = "panel12";
            panel12.Radius = 0;
            panel12.Size = new Size(40, 18);
            panel12.TabIndex = 12;
            panel12.Text = "panel12";
            // 
            // checkbox_Yi_Night
            // 
            checkbox_Yi_Night.AutoCheck = true;
            checkbox_Yi_Night.BackColor = Color.Transparent;
            checkbox_Yi_Night.Checked = true;
            checkbox_Yi_Night.Dock = DockStyle.Left;
            checkbox_Yi_Night.Fill = Color.FromArgb(200, 0, 0);
            checkbox_Yi_Night.Font = new Font("Microsoft YaHei UI", 8F);
            checkbox_Yi_Night.Location = new Point(1, 1);
            checkbox_Yi_Night.Name = "checkbox_Yi_Night";
            checkbox_Yi_Night.RightToLeft = RightToLeft.Yes;
            checkbox_Yi_Night.Size = new Size(40, 16);
            checkbox_Yi_Night.TabIndex = 3;
            checkbox_Yi_Night.Text = "夜";
            checkbox_Yi_Night.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // button9
            // 
            button9.Location = new Point(0, 0);
            button9.Name = "button9";
            button9.Size = new Size(0, 0);
            button9.TabIndex = 0;
            // 
            // panel11
            // 
            panel11.BorderColor = Color.Black;
            panel11.BorderWidth = 1F;
            panel11.Controls.Add(checkbox_Yi_Day);
            panel11.Controls.Add(button8);
            panel11.Dock = DockStyle.Left;
            panel11.Location = new Point(80, 0);
            panel11.Name = "panel11";
            panel11.Radius = 0;
            panel11.Size = new Size(40, 18);
            panel11.TabIndex = 11;
            panel11.Text = "panel11";
            // 
            // checkbox_Yi_Day
            // 
            checkbox_Yi_Day.AutoCheck = true;
            checkbox_Yi_Day.BackColor = Color.Transparent;
            checkbox_Yi_Day.Dock = DockStyle.Left;
            checkbox_Yi_Day.Fill = Color.FromArgb(200, 0, 0);
            checkbox_Yi_Day.Font = new Font("Microsoft YaHei UI", 8F);
            checkbox_Yi_Day.Location = new Point(1, 1);
            checkbox_Yi_Day.Name = "checkbox_Yi_Day";
            checkbox_Yi_Day.RightToLeft = RightToLeft.Yes;
            checkbox_Yi_Day.Size = new Size(40, 16);
            checkbox_Yi_Day.TabIndex = 3;
            checkbox_Yi_Day.Text = "白   ";
            checkbox_Yi_Day.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // button8
            // 
            button8.Location = new Point(0, 0);
            button8.Name = "button8";
            button8.Size = new Size(0, 0);
            button8.TabIndex = 0;
            // 
            // panel10
            // 
            panel10.BorderColor = Color.Black;
            panel10.BorderWidth = 1F;
            panel10.Controls.Add(checkbox_Jia_Night);
            panel10.Controls.Add(button7);
            panel10.Dock = DockStyle.Left;
            panel10.Location = new Point(40, 0);
            panel10.Name = "panel10";
            panel10.Radius = 0;
            panel10.Size = new Size(40, 18);
            panel10.TabIndex = 10;
            panel10.Text = "panel10";
            // 
            // checkbox_Jia_Night
            // 
            checkbox_Jia_Night.AutoCheck = true;
            checkbox_Jia_Night.BackColor = Color.Transparent;
            checkbox_Jia_Night.Dock = DockStyle.Left;
            checkbox_Jia_Night.Fill = Color.FromArgb(200, 0, 0);
            checkbox_Jia_Night.Font = new Font("Microsoft YaHei UI", 8F);
            checkbox_Jia_Night.Location = new Point(1, 1);
            checkbox_Jia_Night.Name = "checkbox_Jia_Night";
            checkbox_Jia_Night.RightToLeft = RightToLeft.Yes;
            checkbox_Jia_Night.Size = new Size(40, 16);
            checkbox_Jia_Night.TabIndex = 3;
            checkbox_Jia_Night.Text = "夜";
            checkbox_Jia_Night.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // button7
            // 
            button7.Location = new Point(0, 0);
            button7.Name = "button7";
            button7.Size = new Size(0, 0);
            button7.TabIndex = 0;
            // 
            // panel9
            // 
            panel9.BorderColor = Color.Black;
            panel9.BorderWidth = 1F;
            panel9.Controls.Add(checkbox_Jia_Day);
            panel9.Controls.Add(button6);
            panel9.Dock = DockStyle.Left;
            panel9.Location = new Point(0, 0);
            panel9.Name = "panel9";
            panel9.Radius = 0;
            panel9.Size = new Size(40, 18);
            panel9.TabIndex = 9;
            panel9.Text = "panel9";
            // 
            // checkbox_Jia_Day
            // 
            checkbox_Jia_Day.AutoCheck = true;
            checkbox_Jia_Day.BackColor = Color.Transparent;
            checkbox_Jia_Day.Dock = DockStyle.Left;
            checkbox_Jia_Day.Fill = Color.FromArgb(200, 0, 0);
            checkbox_Jia_Day.Font = new Font("Microsoft YaHei UI", 8F);
            checkbox_Jia_Day.Location = new Point(1, 1);
            checkbox_Jia_Day.Name = "checkbox_Jia_Day";
            checkbox_Jia_Day.RightToLeft = RightToLeft.Yes;
            checkbox_Jia_Day.Size = new Size(40, 16);
            checkbox_Jia_Day.TabIndex = 3;
            checkbox_Jia_Day.Text = "白   ";
            checkbox_Jia_Day.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // button6
            // 
            button6.Location = new Point(0, 0);
            button6.Name = "button6";
            button6.Size = new Size(0, 0);
            button6.TabIndex = 0;
            // 
            // button4
            // 
            button4.Location = new Point(0, 0);
            button4.Name = "button4";
            button4.Size = new Size(0, 0);
            button4.TabIndex = 0;
            // 
            // panel6
            // 
            panel6.Controls.Add(label3);
            panel6.Controls.Add(label2);
            panel6.Controls.Add(label1);
            panel6.Controls.Add(blacknessMethod_OriginImage_Zone);
            panel6.Controls.Add(button3);
            panel6.Dock = DockStyle.Top;
            panel6.Location = new Point(0, 38);
            panel6.Name = "panel6";
            panel6.Radius = 0;
            panel6.Size = new Size(420, 15);
            panel6.TabIndex = 6;
            panel6.Text = "panel6";
            // 
            // label3
            // 
            label3.BackColor = Color.Transparent;
            label3.BorderStyle = BorderStyle.FixedSingle;
            label3.Dock = DockStyle.Left;
            label3.Font = new Font("Microsoft YaHei UI", 8F);
            label3.ForeColor = Color.Black;
            label3.Location = new Point(240, 0);
            label3.Name = "label3";
            label3.Size = new Size(80, 15);
            label3.TabIndex = 15;
            label3.Text = "丁";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            label2.BackColor = Color.Transparent;
            label2.BorderStyle = BorderStyle.FixedSingle;
            label2.Dock = DockStyle.Left;
            label2.Font = new Font("Microsoft YaHei UI", 8F);
            label2.ForeColor = Color.Black;
            label2.Location = new Point(160, 0);
            label2.Name = "label2";
            label2.Size = new Size(80, 15);
            label2.TabIndex = 14;
            label2.Text = "丙";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            label1.BackColor = Color.Transparent;
            label1.BorderStyle = BorderStyle.FixedSingle;
            label1.Dock = DockStyle.Left;
            label1.Font = new Font("Microsoft YaHei UI", 8F);
            label1.ForeColor = Color.Black;
            label1.Location = new Point(80, 0);
            label1.Name = "label1";
            label1.Size = new Size(80, 15);
            label1.TabIndex = 13;
            label1.Text = "乙";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // blacknessMethod_OriginImage_Zone
            // 
            blacknessMethod_OriginImage_Zone.BackColor = Color.Transparent;
            blacknessMethod_OriginImage_Zone.BorderStyle = BorderStyle.FixedSingle;
            blacknessMethod_OriginImage_Zone.Dock = DockStyle.Left;
            blacknessMethod_OriginImage_Zone.Font = new Font("Microsoft YaHei UI", 8F);
            blacknessMethod_OriginImage_Zone.ForeColor = Color.Black;
            blacknessMethod_OriginImage_Zone.Location = new Point(0, 0);
            blacknessMethod_OriginImage_Zone.Name = "blacknessMethod_OriginImage_Zone";
            blacknessMethod_OriginImage_Zone.Size = new Size(80, 15);
            blacknessMethod_OriginImage_Zone.TabIndex = 12;
            blacknessMethod_OriginImage_Zone.Text = "甲";
            blacknessMethod_OriginImage_Zone.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // button3
            // 
            button3.Location = new Point(0, 0);
            button3.Name = "button3";
            button3.Size = new Size(0, 0);
            button3.TabIndex = 0;
            // 
            // panel4
            // 
            panel4.BorderColor = Color.Gray;
            panel4.BorderStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
            panel4.BorderWidth = 2F;
            panel4.Controls.Add(button2);
            panel4.Dock = DockStyle.Top;
            panel4.Location = new Point(0, 34);
            panel4.Name = "panel4";
            panel4.Size = new Size(420, 4);
            panel4.TabIndex = 5;
            panel4.Text = "panel4";
            // 
            // button2
            // 
            button2.Location = new Point(0, 0);
            button2.Name = "button2";
            button2.Size = new Size(0, 0);
            button2.TabIndex = 0;
            // 
            // panel5
            // 
            panel5.BorderColor = SystemColors.WindowFrame;
            panel5.Controls.Add(labelDate);
            panel5.Controls.Add(labelTitle);
            panel5.Dock = DockStyle.Top;
            panel5.Location = new Point(0, 0);
            panel5.Name = "panel5";
            panel5.Radius = 0;
            panel5.Size = new Size(420, 34);
            panel5.TabIndex = 1;
            panel5.Text = "panel4";
            // 
            // labelDate
            // 
            labelDate.BackColor = Color.Transparent;
            labelDate.Dock = DockStyle.Left;
            labelDate.Font = new Font("Microsoft YaHei UI", 8F);
            labelDate.Location = new Point(280, 0);
            labelDate.Margin = new Padding(0);
            labelDate.Name = "labelDate";
            labelDate.Size = new Size(120, 34);
            labelDate.TabIndex = 14;
            labelDate.Text = "2024 年 11 月 28 日";
            labelDate.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // labelTitle
            // 
            labelTitle.BackColor = Color.Transparent;
            labelTitle.Dock = DockStyle.Left;
            labelTitle.Font = new Font("Microsoft YaHei UI", 12F);
            labelTitle.Location = new Point(0, 0);
            labelTitle.Margin = new Padding(0);
            labelTitle.Name = "labelTitle";
            labelTitle.Size = new Size(280, 34);
            labelTitle.TabIndex = 13;
            labelTitle.Text = "比例尺精度报告书";
            labelTitle.TextAlign = ContentAlignment.MiddleRight;
            // 
            // printDocument1
            // 
            printDocument1.PrintPage += PrintDocument1_PrintPage;
            // 
            // printPreviewDialog1
            // 
            printPreviewDialog1.AutoScrollMargin = new Size(0, 0);
            printPreviewDialog1.AutoScrollMinSize = new Size(0, 0);
            printPreviewDialog1.ClientSize = new Size(400, 300);
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.Enabled = true;
            printPreviewDialog1.Icon = (System.Drawing.Icon)resources.GetObject("printPreviewDialog1.Icon");
            printPreviewDialog1.Name = "printPreviewDialog1";
            printPreviewDialog1.Visible = false;
            // 
            // ScaleAccuracyReport
            // 
            AutoScaleMode = AutoScaleMode.Inherit;
            Controls.Add(panel1);
            Font = new Font("Microsoft YaHei UI", 12F);
            Name = "ScaleAccuracyReport";
            Size = new Size(420, 622);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel3.ResumeLayout(false);
            panel8.ResumeLayout(false);
            panel2.ResumeLayout(false);
            tablePanel.ResumeLayout(false);
            panel7.ResumeLayout(false);
            panel16.ResumeLayout(false);
            panel15.ResumeLayout(false);
            panel14.ResumeLayout(false);
            panel13.ResumeLayout(false);
            panel12.ResumeLayout(false);
            panel11.ResumeLayout(false);
            panel10.ResumeLayout(false);
            panel9.ResumeLayout(false);
            panel6.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel5.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private AntdUI.Panel panel5;
        private Label label_date11;
        private Label labelDate;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private PrintPreviewDialog printPreviewDialog1;
        private PageSetupDialog pageSetupDialog1;
        private AntdUI.Panel panel4;
        private AntdUI.Button button2;
        private AntdUI.Panel panel6;
        private AntdUI.Button button3;
        private Label blacknessMethod_OriginImage_Zone;
        private Label label3;
        private Label label2;
        private Label label1;
        private AntdUI.Panel panel7;
        private AntdUI.Button button4;
        private AntdUI.Panel panel16;
        private AntdUI.Checkbox checkbox_Ding_Night;
        private AntdUI.Button button13;
        private AntdUI.Panel panel15;
        private AntdUI.Checkbox checkbox_Ding_Day;
        private AntdUI.Button button12;
        private AntdUI.Panel panel14;
        private AntdUI.Checkbox checkbox_Bing_Night;
        private AntdUI.Button button11;
        private AntdUI.Panel panel13;
        private AntdUI.Checkbox checkbox_Bing_Day;
        private AntdUI.Button button10;
        private AntdUI.Panel panel12;
        private AntdUI.Checkbox checkbox_Yi_Night;
        private AntdUI.Button button9;
        private AntdUI.Panel panel11;
        private AntdUI.Checkbox checkbox_Yi_Day;
        private AntdUI.Button button8;
        private AntdUI.Panel panel10;
        private AntdUI.Checkbox checkbox_Jia_Night;
        private AntdUI.Button button7;
        private AntdUI.Panel panel9;
        private AntdUI.Checkbox checkbox_Jia_Day;
        private AntdUI.Button button6;
        private Label label4;
        private Label label_Analyst;
        private Label labelTitle;
        private AntdUI.Panel panel2;
        private AntdUI.Avatar avatarScaleRenderImage;
        private TableLayoutPanel tablePanel;
        private AntdUI.Input textEdgeDAPixels;
        private AntdUI.Input textEdgeCDPixels;
        private AntdUI.Input textEdgeBCPixels;
        private AntdUI.Input textEdgeABPixels;
        private AntdUI.Input inputEdgeDALength;
        private AntdUI.Input inputEdgeCDLength;
        private AntdUI.Input inputEdgeBCLength;
        private AntdUI.Input inputEdgeABLength;
        private AntdUI.Label labelInsideOP;
        private AntdUI.Label labelSurfaceDR;
        private AntdUI.Label labelSurfaceCE;
        private AntdUI.Label labelAB;
        private AntdUI.Panel panel8;
        private AntdUI.Label labelAreaScale;
        private AntdUI.Label labelLengthScale;
        private AntdUI.Label labelTopGrade;
        private AntdUI.Steps stepsCalculate;
        private AntdUI.Label labelMPE;
        private AntdUI.Label label5;
        private AntdUI.Label labelSame;
        private AntdUI.Label label6;
        private AntdUI.Label labelAverage;
        private AntdUI.Label label7;
        private AntdUI.Label labelStandardDiviation;
        private AntdUI.Label label10;
        private AntdUI.Label labelStandardError;
        private AntdUI.Label label11;
        private AntdUI.Label labelUncertainty;
        private AntdUI.Label label12;
        private AntdUI.Label labelDistribution;
        private AntdUI.Avatar avatarErrorFormula;
        private AntdUI.Avatar avatarMPE;
        private AntdUI.Avatar avatarNumbers;
        private AntdUI.Avatar avatarAverage;
        private AntdUI.Avatar avatarStandardDiviation;
        private AntdUI.Avatar avatarStandardError;
        private AntdUI.Avatar avatarUncertaincy;
        private AntdUI.Avatar avatarDistribution;
        private AntdUI.Avatar avatarWholeFormula;
        private AntdUI.Label label13;
        private AntdUI.Label labelConfidence;
    }
}