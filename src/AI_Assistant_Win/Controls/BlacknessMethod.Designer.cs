using System.Drawing;
using System.Windows.Forms;

namespace AI_Assistant_Win.Controls
{
    partial class BlacknessMethod
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
            header1 = new AntdUI.PageHeader();
            panel1 = new AntdUI.Panel();
            labelRenderAreaDescription = new AntdUI.Label();
            avatarRenderImage = new AntdUI.Avatar();
            panel2 = new AntdUI.Panel();
            selectScale = new AntdUI.Select();
            btnSetScale = new AntdUI.Button();
            btnPredict = new AntdUI.Button();
            lableRenderImageDisplayArea = new AntdUI.Label();
            panel4 = new AntdUI.Panel();
            labelOriginAreaDescription = new AntdUI.Label();
            avatarOriginImage = new AntdUI.Avatar();
            panel5 = new AntdUI.Panel();
            btnCameraRecover = new AntdUI.Button();
            btnCameraSetting = new AntdUI.Button();
            btnCameraCapture = new AntdUI.Button();
            btnUploadImage = new AntdUI.Button();
            labelOriginImageDisplayArea = new AntdUI.Label();
            flowLayoutPanel1 = new FlowLayoutPanel();
            panel6 = new AntdUI.Panel();
            label3 = new AntdUI.Label();
            panel18 = new AntdUI.Panel();
            inputAnalyst = new AntdUI.Input();
            labelAnalyst = new AntdUI.Label();
            panel17 = new AntdUI.Panel();
            btnNext = new AntdUI.Button();
            btnUpload = new AntdUI.Button();
            btnPrint = new AntdUI.Button();
            btnPre = new AntdUI.Button();
            btnHistory = new AntdUI.Button();
            btnClear = new AntdUI.Button();
            btnSave = new AntdUI.Button();
            panel16 = new AntdUI.Panel();
            inputSize = new AntdUI.Input();
            labelSize = new AntdUI.Label();
            panel15 = new AntdUI.Panel();
            inputCoilNumber = new AntdUI.Input();
            labelCoilNumber = new AntdUI.Label();
            panel14 = new AntdUI.Panel();
            selectTestNo = new AntdUI.Select();
            labelTestNo = new AntdUI.Label();
            panel8 = new AntdUI.Panel();
            selectWorkGroup = new AntdUI.Select();
            labelWorkGroup = new AntdUI.Label();
            divider2 = new AntdUI.Divider();
            panel13 = new AntdUI.Panel();
            radioResultNG = new AntdUI.Radio();
            radioResultOK = new AntdUI.Radio();
            panel12 = new AntdUI.Panel();
            inputInsideDR = new AntdUI.Input();
            labelInsideDR = new AntdUI.Label();
            panel11 = new AntdUI.Panel();
            inputInsideCE = new AntdUI.Input();
            labelInsideCE = new AntdUI.Label();
            panel10 = new AntdUI.Panel();
            inputInsideOP = new AntdUI.Input();
            labelInsideOP = new AntdUI.Label();
            panel9 = new AntdUI.Panel();
            inputSurfaceDR = new AntdUI.Input();
            labelSurfaceDR = new AntdUI.Label();
            panel7 = new AntdUI.Panel();
            inputSurfaceCE = new AntdUI.Input();
            labelSurfaceCE = new AntdUI.Label();
            panel3 = new AntdUI.Panel();
            inputSurfaceOP = new AntdUI.Input();
            labelSurfaceOP = new AntdUI.Label();
            labelResultJudgeArea = new AntdUI.Label();
            blacknessMethod_OpenFileDialog = new OpenFileDialog();
            blacknessMethod_FileSystemWatcher = new System.IO.FileSystemWatcher();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel4.SuspendLayout();
            panel5.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            panel6.SuspendLayout();
            panel18.SuspendLayout();
            panel17.SuspendLayout();
            panel16.SuspendLayout();
            panel15.SuspendLayout();
            panel14.SuspendLayout();
            panel8.SuspendLayout();
            panel13.SuspendLayout();
            panel12.SuspendLayout();
            panel11.SuspendLayout();
            panel10.SuspendLayout();
            panel9.SuspendLayout();
            panel7.SuspendLayout();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)blacknessMethod_FileSystemWatcher).BeginInit();
            SuspendLayout();
            // 
            // header1
            // 
            header1.Description = "本方法适用于GA板锌层密着性试验中V60试验后胶带的黑度评级。测试白板、产品特性、试验弯曲角度等有明显相关性，胶带的氧化发黄会对该评定有干扰。";
            header1.Dock = DockStyle.Top;
            header1.Font = new Font("Microsoft YaHei UI", 12F);
            header1.LocalizationDescription = "BlacknessMethod.Description";
            header1.LocalizationText = "BlacknessMethod.Text";
            header1.Location = new Point(0, 0);
            header1.Name = "header1";
            header1.Padding = new Padding(0, 0, 0, 10);
            header1.Size = new Size(1300, 74);
            header1.TabIndex = 0;
            header1.Text = "GA板锌层密着性V60黑度检测";
            header1.UseTitleFont = true;
            // 
            // panel1
            // 
            panel1.ArrowSize = 10;
            panel1.Controls.Add(labelRenderAreaDescription);
            panel1.Controls.Add(avatarRenderImage);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(lableRenderImageDisplayArea);
            panel1.Location = new Point(449, 3);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(7);
            panel1.Radius = 10;
            panel1.Shadow = 2;
            panel1.ShadowOpacityAnimation = true;
            panel1.Size = new Size(462, 584);
            panel1.TabIndex = 20;
            // 
            // labelRenderAreaDescription
            // 
            labelRenderAreaDescription.BackColor = Color.Transparent;
            labelRenderAreaDescription.Dock = DockStyle.Fill;
            labelRenderAreaDescription.Font = new Font("Microsoft YaHei UI", 10F);
            labelRenderAreaDescription.LocalizationText = "Click on the recognized image to zoom in for a preview. The recognition results of each part will be displayed in the Result Judgment Area on the right.";
            labelRenderAreaDescription.Location = new Point(9, 494);
            labelRenderAreaDescription.Name = "labelRenderAreaDescription";
            labelRenderAreaDescription.Padding = new Padding(2, 0, 2, 0);
            labelRenderAreaDescription.Size = new Size(444, 41);
            labelRenderAreaDescription.TabIndex = 12;
            labelRenderAreaDescription.Text = "点击识别后的图片可进行放大预览，各部位识别结果会在右侧结果判定区展示。";
            // 
            // avatarRenderImage
            // 
            avatarRenderImage.Dock = DockStyle.Top;
            avatarRenderImage.Image = Properties.Resources.blackness_template;
            avatarRenderImage.Location = new Point(9, 39);
            avatarRenderImage.Name = "avatarRenderImage";
            avatarRenderImage.Radius = 6;
            avatarRenderImage.Size = new Size(444, 455);
            avatarRenderImage.TabIndex = 9;
            avatarRenderImage.Click += AvatarRenderImage_Click;
            // 
            // panel2
            // 
            panel2.Back = Color.Transparent;
            panel2.BackColor = Color.Transparent;
            panel2.Controls.Add(selectScale);
            panel2.Controls.Add(btnSetScale);
            panel2.Controls.Add(btnPredict);
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(9, 535);
            panel2.Name = "panel2";
            panel2.Radius = 0;
            panel2.Size = new Size(444, 40);
            panel2.TabIndex = 13;
            // 
            // selectScale
            // 
            selectScale.AllowClear = true;
            selectScale.Dock = DockStyle.Left;
            selectScale.DropDownArrow = true;
            selectScale.Font = new Font("Microsoft YaHei UI", 10F);
            selectScale.LocalizationPlaceholderText = "Please select a scale";
            selectScale.Location = new Point(40, 0);
            selectScale.Name = "selectScale";
            selectScale.Padding = new Padding(5, 0, 0, 0);
            selectScale.PlaceholderText = "请选择比例尺";
            selectScale.Size = new Size(293, 40);
            selectScale.TabIndex = 19;
            selectScale.SelectedIndexChanged += SelectScale_SelectedIndexChanged;
            // 
            // btnSetScale
            // 
            btnSetScale.Dock = DockStyle.Left;
            btnSetScale.IconSvg = "ToolOutlined";
            btnSetScale.LoadingWaveVertical = true;
            btnSetScale.Location = new Point(0, 0);
            btnSetScale.Name = "btnSetScale";
            btnSetScale.Shape = AntdUI.TShape.Circle;
            btnSetScale.Size = new Size(40, 40);
            btnSetScale.TabIndex = 3;
            btnSetScale.Click += BtnSetScale_Click;
            // 
            // btnPredict
            // 
            btnPredict.Dock = DockStyle.Right;
            btnPredict.Enabled = false;
            btnPredict.Font = new Font("Microsoft YaHei UI", 10F);
            btnPredict.LocalizationText = "Predict";
            btnPredict.Location = new Point(371, 0);
            btnPredict.Name = "btnPredict";
            btnPredict.Size = new Size(73, 40);
            btnPredict.TabIndex = 0;
            btnPredict.Text = "识别";
            btnPredict.Type = AntdUI.TTypeMini.Error;
            btnPredict.Click += BtnPredict_Click;
            // 
            // lableRenderImageDisplayArea
            // 
            lableRenderImageDisplayArea.BackColor = Color.Transparent;
            lableRenderImageDisplayArea.Dock = DockStyle.Top;
            lableRenderImageDisplayArea.Font = new Font("Microsoft YaHei UI", 15F, FontStyle.Bold);
            lableRenderImageDisplayArea.ForeColor = Color.DarkGray;
            lableRenderImageDisplayArea.LocalizationText = "Recognition Display Area";
            lableRenderImageDisplayArea.Location = new Point(9, 9);
            lableRenderImageDisplayArea.Name = "lableRenderImageDisplayArea";
            lableRenderImageDisplayArea.Size = new Size(444, 30);
            lableRenderImageDisplayArea.TabIndex = 11;
            lableRenderImageDisplayArea.Text = "识 别 展 示 区";
            lableRenderImageDisplayArea.TextAlign = ContentAlignment.TopCenter;
            // 
            // panel4
            // 
            panel4.ArrowSize = 10;
            panel4.Controls.Add(labelOriginAreaDescription);
            panel4.Controls.Add(avatarOriginImage);
            panel4.Controls.Add(panel5);
            panel4.Controls.Add(labelOriginImageDisplayArea);
            panel4.Location = new Point(3, 3);
            panel4.Name = "panel4";
            panel4.Padding = new Padding(7);
            panel4.Radius = 10;
            panel4.Shadow = 2;
            panel4.ShadowOpacityAnimation = true;
            panel4.Size = new Size(440, 584);
            panel4.TabIndex = 19;
            // 
            // labelOriginAreaDescription
            // 
            labelOriginAreaDescription.BackColor = Color.Transparent;
            labelOriginAreaDescription.Dock = DockStyle.Fill;
            labelOriginAreaDescription.Font = new Font("Microsoft YaHei UI", 10F);
            labelOriginAreaDescription.LocalizationText = "Select to upload a local image or use the camera from the darkness detection workstation to take a photo.The result will be automatically recognized. ";
            labelOriginAreaDescription.Location = new Point(9, 494);
            labelOriginAreaDescription.Name = "labelOriginAreaDescription";
            labelOriginAreaDescription.Padding = new Padding(2, 0, 2, 0);
            labelOriginAreaDescription.Size = new Size(422, 41);
            labelOriginAreaDescription.TabIndex = 12;
            labelOriginAreaDescription.Text = "选择本地图片上传或者调用黑度检测工作台摄像头拍摄照片，上传成功后结果会自动识别。";
            // 
            // avatarOriginImage
            // 
            avatarOriginImage.Dock = DockStyle.Top;
            avatarOriginImage.Image = Properties.Resources.blackness_template;
            avatarOriginImage.Location = new Point(9, 39);
            avatarOriginImage.Name = "avatarOriginImage";
            avatarOriginImage.Radius = 6;
            avatarOriginImage.Size = new Size(422, 455);
            avatarOriginImage.TabIndex = 9;
            avatarOriginImage.Click += AvatarOriginImage_Click;
            // 
            // panel5
            // 
            panel5.Back = Color.Transparent;
            panel5.BackColor = Color.Transparent;
            panel5.Controls.Add(btnCameraRecover);
            panel5.Controls.Add(btnCameraSetting);
            panel5.Controls.Add(btnCameraCapture);
            panel5.Controls.Add(btnUploadImage);
            panel5.Dock = DockStyle.Bottom;
            panel5.Location = new Point(9, 535);
            panel5.Name = "panel5";
            panel5.Radius = 0;
            panel5.Size = new Size(422, 40);
            panel5.TabIndex = 13;
            // 
            // btnCameraRecover
            // 
            btnCameraRecover.Dock = DockStyle.Right;
            btnCameraRecover.Enabled = false;
            btnCameraRecover.Font = new Font("Microsoft YaHei UI", 10F);
            btnCameraRecover.LocalizationText = "CameraRecover";
            btnCameraRecover.Location = new Point(73, 0);
            btnCameraRecover.Name = "btnCameraRecover";
            btnCameraRecover.Size = new Size(119, 40);
            btnCameraRecover.TabIndex = 3;
            btnCameraRecover.Text = "恢复拍摄";
            btnCameraRecover.Type = AntdUI.TTypeMini.Success;
            btnCameraRecover.Click += BtnCameraRecover_Click;
            // 
            // btnCameraSetting
            // 
            btnCameraSetting.IconSvg = "SettingOutlined";
            btnCameraSetting.LoadingWaveVertical = true;
            btnCameraSetting.Location = new Point(0, 0);
            btnCameraSetting.Name = "btnCameraSetting";
            btnCameraSetting.Shape = AntdUI.TShape.Circle;
            btnCameraSetting.Size = new Size(40, 40);
            btnCameraSetting.TabIndex = 2;
            btnCameraSetting.Click += BtnCameraSetting_Click;
            // 
            // btnCameraCapture
            // 
            btnCameraCapture.Dock = DockStyle.Right;
            btnCameraCapture.Enabled = false;
            btnCameraCapture.Font = new Font("Microsoft YaHei UI", 10F);
            btnCameraCapture.LocalizationText = "CameraCapture";
            btnCameraCapture.Location = new Point(192, 0);
            btnCameraCapture.Name = "btnCameraCapture";
            btnCameraCapture.Size = new Size(119, 40);
            btnCameraCapture.TabIndex = 1;
            btnCameraCapture.Text = "拍摄相片";
            btnCameraCapture.Type = AntdUI.TTypeMini.Warn;
            btnCameraCapture.Click += BtnCameraCapture_Click;
            // 
            // btnUploadImage
            // 
            btnUploadImage.Dock = DockStyle.Right;
            btnUploadImage.Font = new Font("Microsoft YaHei UI", 10F);
            btnUploadImage.LocalizationText = "UploadImage";
            btnUploadImage.Location = new Point(311, 0);
            btnUploadImage.Name = "btnUploadImage";
            btnUploadImage.Size = new Size(111, 40);
            btnUploadImage.TabIndex = 0;
            btnUploadImage.Text = "本地图片";
            btnUploadImage.Type = AntdUI.TTypeMini.Primary;
            btnUploadImage.Click += BtnUploadImage_Click;
            // 
            // labelOriginImageDisplayArea
            // 
            labelOriginImageDisplayArea.BackColor = Color.Transparent;
            labelOriginImageDisplayArea.Dock = DockStyle.Top;
            labelOriginImageDisplayArea.Font = new Font("Microsoft YaHei UI", 15F, FontStyle.Bold);
            labelOriginImageDisplayArea.ForeColor = Color.DarkGray;
            labelOriginImageDisplayArea.LocalizationText = "Original Image Display Area";
            labelOriginImageDisplayArea.Location = new Point(9, 9);
            labelOriginImageDisplayArea.Name = "labelOriginImageDisplayArea";
            labelOriginImageDisplayArea.Size = new Size(422, 30);
            labelOriginImageDisplayArea.TabIndex = 11;
            labelOriginImageDisplayArea.Text = "原 图 展 示 区";
            labelOriginImageDisplayArea.TextAlign = ContentAlignment.TopCenter;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.Controls.Add(panel4);
            flowLayoutPanel1.Controls.Add(panel1);
            flowLayoutPanel1.Controls.Add(panel6);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(0, 74);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(1300, 560);
            flowLayoutPanel1.TabIndex = 5;
            // 
            // panel6
            // 
            panel6.ArrowSize = 10;
            panel6.Controls.Add(label3);
            panel6.Controls.Add(panel18);
            panel6.Controls.Add(panel17);
            panel6.Controls.Add(panel16);
            panel6.Controls.Add(panel15);
            panel6.Controls.Add(panel14);
            panel6.Controls.Add(panel8);
            panel6.Controls.Add(divider2);
            panel6.Controls.Add(panel13);
            panel6.Controls.Add(panel12);
            panel6.Controls.Add(panel11);
            panel6.Controls.Add(panel10);
            panel6.Controls.Add(panel9);
            panel6.Controls.Add(panel7);
            panel6.Controls.Add(panel3);
            panel6.Controls.Add(labelResultJudgeArea);
            panel6.Location = new Point(917, 3);
            panel6.Name = "panel6";
            panel6.Padding = new Padding(7);
            panel6.Radius = 10;
            panel6.Shadow = 2;
            panel6.ShadowOpacity = 0.18F;
            panel6.ShadowOpacityAnimation = true;
            panel6.Size = new Size(360, 584);
            panel6.TabIndex = 20;
            // 
            // label3
            // 
            label3.BackColor = Color.Transparent;
            label3.Dock = DockStyle.Fill;
            label3.Font = new Font("Microsoft YaHei UI", 10F);
            label3.ForeColor = Color.Red;
            label3.LocalizationText = "Note: This test is applied for darkness judgment of GA board V60, with a required board thickness ranging from 0.3 to 2.3mm.";
            label3.Location = new Point(9, 469);
            label3.Name = "label3";
            label3.Padding = new Padding(2, 0, 2, 0);
            label3.Size = new Size(342, 66);
            label3.TabIndex = 29;
            label3.Text = "注：此测试是应用于GA板V60黑度判定，板厚要求在0.3~2.3mm";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel18
            // 
            panel18.Back = Color.Transparent;
            panel18.BackColor = Color.Transparent;
            panel18.Controls.Add(inputAnalyst);
            panel18.Controls.Add(labelAnalyst);
            panel18.Dock = DockStyle.Top;
            panel18.Location = new Point(9, 436);
            panel18.Name = "panel18";
            panel18.Radius = 0;
            panel18.Size = new Size(342, 33);
            panel18.TabIndex = 27;
            // 
            // inputAnalyst
            // 
            inputAnalyst.Enabled = false;
            inputAnalyst.Font = new Font("Microsoft YaHei UI", 10F);
            inputAnalyst.Location = new Point(96, -1);
            inputAnalyst.Name = "inputAnalyst";
            inputAnalyst.Size = new Size(243, 34);
            inputAnalyst.TabIndex = 17;
            inputAnalyst.TextChanged += InputAnalyst_TextChanged;
            // 
            // labelAnalyst
            // 
            labelAnalyst.BackColor = Color.Transparent;
            labelAnalyst.Dock = DockStyle.Left;
            labelAnalyst.Enabled = false;
            labelAnalyst.Font = new Font("Microsoft YaHei UI", 10F);
            labelAnalyst.LocalizationText = "Analyst:";
            labelAnalyst.Location = new Point(0, 0);
            labelAnalyst.Name = "labelAnalyst";
            labelAnalyst.Size = new Size(90, 33);
            labelAnalyst.TabIndex = 16;
            labelAnalyst.Text = "分析人：";
            labelAnalyst.TextAlign = ContentAlignment.MiddleRight;
            // 
            // panel17
            // 
            panel17.Back = Color.Transparent;
            panel17.BackColor = Color.Transparent;
            panel17.Controls.Add(btnNext);
            panel17.Controls.Add(btnUpload);
            panel17.Controls.Add(btnPrint);
            panel17.Controls.Add(btnPre);
            panel17.Controls.Add(btnHistory);
            panel17.Controls.Add(btnClear);
            panel17.Controls.Add(btnSave);
            panel17.Dock = DockStyle.Bottom;
            panel17.Location = new Point(9, 535);
            panel17.Name = "panel17";
            panel17.Radius = 0;
            panel17.Size = new Size(342, 40);
            panel17.TabIndex = 26;
            // 
            // btnNext
            // 
            btnNext.Dock = DockStyle.Left;
            btnNext.Enabled = false;
            btnNext.IconSvg = "ArrowRightOutlined";
            btnNext.LoadingWaveVertical = true;
            btnNext.Location = new Point(200, 0);
            btnNext.Name = "btnNext";
            btnNext.Shape = AntdUI.TShape.Circle;
            btnNext.Size = new Size(40, 40);
            btnNext.TabIndex = 8;
            btnNext.Visible = false;
            btnNext.Click += BtnNext_Click;
            // 
            // btnUpload
            // 
            btnUpload.Dock = DockStyle.Left;
            btnUpload.Enabled = false;
            btnUpload.IconSvg = "CloudUploadOutlined";
            btnUpload.LoadingWaveVertical = true;
            btnUpload.Location = new Point(160, 0);
            btnUpload.Name = "btnUpload";
            btnUpload.Shape = AntdUI.TShape.Circle;
            btnUpload.Size = new Size(40, 40);
            btnUpload.TabIndex = 7;
            btnUpload.Visible = false;
            // 
            // btnPrint
            // 
            btnPrint.Dock = DockStyle.Left;
            btnPrint.IconSvg = "PrinterOutlined";
            btnPrint.LoadingWaveVertical = true;
            btnPrint.Location = new Point(120, 0);
            btnPrint.Name = "btnPrint";
            btnPrint.Shape = AntdUI.TShape.Circle;
            btnPrint.Size = new Size(40, 40);
            btnPrint.TabIndex = 6;
            btnPrint.Visible = false;
            btnPrint.Click += BtnPrint_Click;
            // 
            // btnPre
            // 
            btnPre.Dock = DockStyle.Left;
            btnPre.Enabled = false;
            btnPre.IconSvg = "ArrowLeftOutlined";
            btnPre.LoadingWaveVertical = true;
            btnPre.Location = new Point(80, 0);
            btnPre.Name = "btnPre";
            btnPre.Shape = AntdUI.TShape.Circle;
            btnPre.Size = new Size(40, 40);
            btnPre.TabIndex = 5;
            btnPre.Visible = false;
            btnPre.Click += BtnPre_Click;
            // 
            // btnHistory
            // 
            btnHistory.Dock = DockStyle.Left;
            btnHistory.IconSvg = "OrderedListOutlined";
            btnHistory.LoadingWaveVertical = true;
            btnHistory.Location = new Point(40, 0);
            btnHistory.Name = "btnHistory";
            btnHistory.Shape = AntdUI.TShape.Circle;
            btnHistory.Size = new Size(40, 40);
            btnHistory.TabIndex = 4;
            btnHistory.Click += BtnHistory_Click;
            // 
            // btnClear
            // 
            btnClear.Dock = DockStyle.Left;
            btnClear.IconSvg = "ClearOutlined";
            btnClear.LoadingWaveVertical = true;
            btnClear.Location = new Point(0, 0);
            btnClear.Name = "btnClear";
            btnClear.Shape = AntdUI.TShape.Circle;
            btnClear.Size = new Size(40, 40);
            btnClear.TabIndex = 3;
            btnClear.Click += BtnClear_Click;
            // 
            // btnSave
            // 
            btnSave.Dock = DockStyle.Right;
            btnSave.Enabled = false;
            btnSave.Font = new Font("Microsoft YaHei UI", 10F);
            btnSave.LocalizationText = "Save";
            btnSave.Location = new Point(269, 0);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(73, 40);
            btnSave.TabIndex = 0;
            btnSave.Text = "保存";
            btnSave.Type = AntdUI.TTypeMini.Error;
            btnSave.Click += BtnSave_Click;
            // 
            // panel16
            // 
            panel16.Back = Color.Transparent;
            panel16.BackColor = Color.Transparent;
            panel16.Controls.Add(inputSize);
            panel16.Controls.Add(labelSize);
            panel16.Dock = DockStyle.Top;
            panel16.Location = new Point(9, 403);
            panel16.Name = "panel16";
            panel16.Radius = 0;
            panel16.Size = new Size(342, 33);
            panel16.TabIndex = 25;
            // 
            // inputSize
            // 
            inputSize.Font = new Font("Microsoft YaHei UI", 10F);
            inputSize.Location = new Point(96, -1);
            inputSize.Name = "inputSize";
            inputSize.Size = new Size(243, 34);
            inputSize.TabIndex = 17;
            inputSize.TextChanged += InputSize_TextChanged;
            // 
            // labelSize
            // 
            labelSize.BackColor = Color.Transparent;
            labelSize.Dock = DockStyle.Left;
            labelSize.Font = new Font("Microsoft YaHei UI", 10F);
            labelSize.LocalizationText = "Size:";
            labelSize.Location = new Point(0, 0);
            labelSize.Name = "labelSize";
            labelSize.Size = new Size(90, 33);
            labelSize.TabIndex = 16;
            labelSize.Text = "尺寸：";
            labelSize.TextAlign = ContentAlignment.MiddleRight;
            // 
            // panel15
            // 
            panel15.Back = Color.Transparent;
            panel15.BackColor = Color.Transparent;
            panel15.Controls.Add(inputCoilNumber);
            panel15.Controls.Add(labelCoilNumber);
            panel15.Dock = DockStyle.Top;
            panel15.Location = new Point(9, 370);
            panel15.Name = "panel15";
            panel15.Radius = 0;
            panel15.Size = new Size(342, 33);
            panel15.TabIndex = 24;
            // 
            // inputCoilNumber
            // 
            inputCoilNumber.Enabled = false;
            inputCoilNumber.Font = new Font("Microsoft YaHei UI", 10F);
            inputCoilNumber.Location = new Point(96, -1);
            inputCoilNumber.Name = "inputCoilNumber";
            inputCoilNumber.Size = new Size(243, 34);
            inputCoilNumber.TabIndex = 17;
            inputCoilNumber.TextChanged += InputCoilNumber_TextChanged;
            // 
            // labelCoilNumber
            // 
            labelCoilNumber.BackColor = Color.Transparent;
            labelCoilNumber.Dock = DockStyle.Left;
            labelCoilNumber.Font = new Font("Microsoft YaHei UI", 10F);
            labelCoilNumber.LocalizationText = "CoilNumber:";
            labelCoilNumber.Location = new Point(0, 0);
            labelCoilNumber.Name = "labelCoilNumber";
            labelCoilNumber.Size = new Size(90, 33);
            labelCoilNumber.TabIndex = 16;
            labelCoilNumber.Text = "钢卷号：";
            labelCoilNumber.TextAlign = ContentAlignment.MiddleRight;
            // 
            // panel14
            // 
            panel14.Back = Color.Transparent;
            panel14.BackColor = Color.Transparent;
            panel14.Controls.Add(selectTestNo);
            panel14.Controls.Add(labelTestNo);
            panel14.Dock = DockStyle.Top;
            panel14.Location = new Point(9, 337);
            panel14.Name = "panel14";
            panel14.Radius = 0;
            panel14.Size = new Size(342, 33);
            panel14.TabIndex = 23;
            // 
            // selectTestNo
            // 
            selectTestNo.AllowClear = true;
            selectTestNo.Dock = DockStyle.Left;
            selectTestNo.DropDownArrow = true;
            selectTestNo.Font = new Font("Microsoft YaHei UI", 10F);
            selectTestNo.LocalizationPlaceholderText = "Please select a test no";
            selectTestNo.Location = new Point(90, 0);
            selectTestNo.Name = "selectTestNo";
            selectTestNo.Padding = new Padding(5, 0, 0, 0);
            selectTestNo.PlaceholderText = "请选择试样编号";
            selectTestNo.Size = new Size(249, 33);
            selectTestNo.TabIndex = 18;
            selectTestNo.SelectedIndexChanged += SelectTestNo_SelectedIndexChanged;
            // 
            // labelTestNo
            // 
            labelTestNo.BackColor = Color.Transparent;
            labelTestNo.Dock = DockStyle.Left;
            labelTestNo.Font = new Font("Microsoft YaHei UI", 10F);
            labelTestNo.LocalizationText = "TestNo:";
            labelTestNo.Location = new Point(0, 0);
            labelTestNo.Name = "labelTestNo";
            labelTestNo.Size = new Size(90, 33);
            labelTestNo.TabIndex = 16;
            labelTestNo.Text = "试样编号：";
            labelTestNo.TextAlign = ContentAlignment.MiddleRight;
            // 
            // panel8
            // 
            panel8.Back = Color.Transparent;
            panel8.BackColor = Color.Transparent;
            panel8.Controls.Add(selectWorkGroup);
            panel8.Controls.Add(labelWorkGroup);
            panel8.Dock = DockStyle.Top;
            panel8.Location = new Point(9, 304);
            panel8.Name = "panel8";
            panel8.Radius = 0;
            panel8.Size = new Size(342, 33);
            panel8.TabIndex = 22;
            // 
            // selectWorkGroup
            // 
            selectWorkGroup.AllowClear = true;
            selectWorkGroup.Dock = DockStyle.Left;
            selectWorkGroup.DropDownArrow = true;
            selectWorkGroup.Font = new Font("Microsoft YaHei UI", 10F);
            selectWorkGroup.Items.AddRange(new object[] { "甲-白", "甲-夜", "乙-白", "乙-夜", "丙-白", "丙-夜", "丁-白", "丁-夜" });
            selectWorkGroup.LocalizationPlaceholderText = "Please select a work group";
            selectWorkGroup.Location = new Point(90, 0);
            selectWorkGroup.Name = "selectWorkGroup";
            selectWorkGroup.Padding = new Padding(5, 0, 0, 0);
            selectWorkGroup.PlaceholderText = "请选择班组";
            selectWorkGroup.Size = new Size(249, 33);
            selectWorkGroup.TabIndex = 17;
            selectWorkGroup.SelectedIndexChanged += SelectWorkGroup_SelectedIndexChanged;
            // 
            // labelWorkGroup
            // 
            labelWorkGroup.BackColor = Color.Transparent;
            labelWorkGroup.Dock = DockStyle.Left;
            labelWorkGroup.Font = new Font("Microsoft YaHei UI", 10F);
            labelWorkGroup.LocalizationText = "WorkGroup:";
            labelWorkGroup.Location = new Point(0, 0);
            labelWorkGroup.Name = "labelWorkGroup";
            labelWorkGroup.Size = new Size(90, 33);
            labelWorkGroup.TabIndex = 16;
            labelWorkGroup.Text = "班组：";
            labelWorkGroup.TextAlign = ContentAlignment.MiddleRight;
            // 
            // divider2
            // 
            divider2.BackColor = Color.Transparent;
            divider2.Dock = DockStyle.Top;
            divider2.Font = new Font("Microsoft YaHei UI", 16F, FontStyle.Bold);
            divider2.ForeColor = Color.DarkGray;
            divider2.LocalizationText = "Data Entry Area";
            divider2.Location = new Point(9, 270);
            divider2.Name = "divider2";
            divider2.Size = new Size(342, 34);
            divider2.TabIndex = 21;
            divider2.Text = "数 据 录 入 区";
            divider2.Thickness = 2F;
            // 
            // panel13
            // 
            panel13.Back = Color.Transparent;
            panel13.BackColor = Color.Transparent;
            panel13.Controls.Add(radioResultNG);
            panel13.Controls.Add(radioResultOK);
            panel13.Dock = DockStyle.Top;
            panel13.Location = new Point(9, 237);
            panel13.Name = "panel13";
            panel13.Radius = 0;
            panel13.RightToLeft = RightToLeft.No;
            panel13.Size = new Size(342, 33);
            panel13.TabIndex = 20;
            // 
            // radioResultNG
            // 
            radioResultNG.AutoCheck = true;
            radioResultNG.AutoSizeMode = AntdUI.TAutoSize.Width;
            radioResultNG.Checked = true;
            radioResultNG.Fill = Color.FromArgb(100, 0, 0);
            radioResultNG.Font = new Font("Microsoft YaHei UI", 12F);
            radioResultNG.ForeColor = Color.Red;
            radioResultNG.Location = new Point(180, 0);
            radioResultNG.Name = "radioResultNG";
            radioResultNG.Size = new Size(74, 33);
            radioResultNG.TabIndex = 18;
            radioResultNG.Text = "NG";
            // 
            // radioResultOK
            // 
            radioResultOK.AutoCheck = true;
            radioResultOK.AutoSizeMode = AntdUI.TAutoSize.Width;
            radioResultOK.BackColor = Color.Transparent;
            radioResultOK.Fill = Color.FromArgb(250, 0, 0);
            radioResultOK.Font = new Font("Microsoft YaHei UI", 12F);
            radioResultOK.ForeColor = Color.Green;
            radioResultOK.Location = new Point(90, 0);
            radioResultOK.Name = "radioResultOK";
            radioResultOK.Size = new Size(73, 33);
            radioResultOK.TabIndex = 17;
            radioResultOK.Text = "OK";
            // 
            // panel12
            // 
            panel12.Back = Color.Transparent;
            panel12.BackColor = Color.Transparent;
            panel12.Controls.Add(inputInsideDR);
            panel12.Controls.Add(labelInsideDR);
            panel12.Dock = DockStyle.Top;
            panel12.Location = new Point(9, 204);
            panel12.Name = "panel12";
            panel12.Radius = 0;
            panel12.Size = new Size(342, 33);
            panel12.TabIndex = 19;
            // 
            // inputInsideDR
            // 
            inputInsideDR.Font = new Font("Microsoft YaHei UI", 10F);
            inputInsideDR.Location = new Point(96, -1);
            inputInsideDR.Name = "inputInsideDR";
            inputInsideDR.Size = new Size(243, 34);
            inputInsideDR.TabIndex = 17;
            // 
            // labelInsideDR
            // 
            labelInsideDR.BackColor = Color.Transparent;
            labelInsideDR.Dock = DockStyle.Left;
            labelInsideDR.Font = new Font("Microsoft YaHei UI", 10F);
            labelInsideDR.LocalizationText = "InsideDR:";
            labelInsideDR.Location = new Point(0, 0);
            labelInsideDR.Name = "labelInsideDR";
            labelInsideDR.Size = new Size(90, 33);
            labelInsideDR.TabIndex = 16;
            labelInsideDR.Text = "里面DR：";
            labelInsideDR.TextAlign = ContentAlignment.MiddleRight;
            // 
            // panel11
            // 
            panel11.Back = Color.Transparent;
            panel11.BackColor = Color.Transparent;
            panel11.Controls.Add(inputInsideCE);
            panel11.Controls.Add(labelInsideCE);
            panel11.Dock = DockStyle.Top;
            panel11.Location = new Point(9, 171);
            panel11.Name = "panel11";
            panel11.Radius = 0;
            panel11.Size = new Size(342, 33);
            panel11.TabIndex = 18;
            // 
            // inputInsideCE
            // 
            inputInsideCE.Font = new Font("Microsoft YaHei UI", 10F);
            inputInsideCE.Location = new Point(96, -1);
            inputInsideCE.Name = "inputInsideCE";
            inputInsideCE.Size = new Size(243, 34);
            inputInsideCE.TabIndex = 17;
            // 
            // labelInsideCE
            // 
            labelInsideCE.BackColor = Color.Transparent;
            labelInsideCE.Dock = DockStyle.Left;
            labelInsideCE.Font = new Font("Microsoft YaHei UI", 10F);
            labelInsideCE.LocalizationText = "InsideCE:";
            labelInsideCE.Location = new Point(0, 0);
            labelInsideCE.Name = "labelInsideCE";
            labelInsideCE.Size = new Size(90, 33);
            labelInsideCE.TabIndex = 16;
            labelInsideCE.Text = "里面CE：";
            labelInsideCE.TextAlign = ContentAlignment.MiddleRight;
            // 
            // panel10
            // 
            panel10.Back = Color.Transparent;
            panel10.BackColor = Color.Transparent;
            panel10.Controls.Add(inputInsideOP);
            panel10.Controls.Add(labelInsideOP);
            panel10.Dock = DockStyle.Top;
            panel10.Location = new Point(9, 138);
            panel10.Name = "panel10";
            panel10.Radius = 0;
            panel10.Size = new Size(342, 33);
            panel10.TabIndex = 17;
            // 
            // inputInsideOP
            // 
            inputInsideOP.Font = new Font("Microsoft YaHei UI", 10F);
            inputInsideOP.Location = new Point(96, -1);
            inputInsideOP.Name = "inputInsideOP";
            inputInsideOP.Size = new Size(243, 34);
            inputInsideOP.TabIndex = 17;
            // 
            // labelInsideOP
            // 
            labelInsideOP.BackColor = Color.Transparent;
            labelInsideOP.Dock = DockStyle.Left;
            labelInsideOP.Font = new Font("Microsoft YaHei UI", 10F);
            labelInsideOP.LocalizationText = "InsideOP:";
            labelInsideOP.Location = new Point(0, 0);
            labelInsideOP.Name = "labelInsideOP";
            labelInsideOP.Size = new Size(90, 33);
            labelInsideOP.TabIndex = 16;
            labelInsideOP.Text = "里面OP：";
            labelInsideOP.TextAlign = ContentAlignment.MiddleRight;
            // 
            // panel9
            // 
            panel9.Back = Color.Transparent;
            panel9.BackColor = Color.Transparent;
            panel9.Controls.Add(inputSurfaceDR);
            panel9.Controls.Add(labelSurfaceDR);
            panel9.Dock = DockStyle.Top;
            panel9.Location = new Point(9, 105);
            panel9.Name = "panel9";
            panel9.Radius = 0;
            panel9.Size = new Size(342, 33);
            panel9.TabIndex = 16;
            // 
            // inputSurfaceDR
            // 
            inputSurfaceDR.Font = new Font("Microsoft YaHei UI", 10F);
            inputSurfaceDR.Location = new Point(96, -1);
            inputSurfaceDR.Name = "inputSurfaceDR";
            inputSurfaceDR.Size = new Size(243, 34);
            inputSurfaceDR.TabIndex = 17;
            // 
            // labelSurfaceDR
            // 
            labelSurfaceDR.BackColor = Color.Transparent;
            labelSurfaceDR.Dock = DockStyle.Left;
            labelSurfaceDR.Font = new Font("Microsoft YaHei UI", 10F);
            labelSurfaceDR.LocalizationText = "SurfaceDR:";
            labelSurfaceDR.Location = new Point(0, 0);
            labelSurfaceDR.Name = "labelSurfaceDR";
            labelSurfaceDR.Size = new Size(90, 33);
            labelSurfaceDR.TabIndex = 16;
            labelSurfaceDR.Text = "表面DR：";
            labelSurfaceDR.TextAlign = ContentAlignment.MiddleRight;
            // 
            // panel7
            // 
            panel7.Back = Color.Transparent;
            panel7.BackColor = Color.Transparent;
            panel7.Controls.Add(inputSurfaceCE);
            panel7.Controls.Add(labelSurfaceCE);
            panel7.Dock = DockStyle.Top;
            panel7.Location = new Point(9, 72);
            panel7.Name = "panel7";
            panel7.Radius = 0;
            panel7.Size = new Size(342, 33);
            panel7.TabIndex = 15;
            // 
            // inputSurfaceCE
            // 
            inputSurfaceCE.Font = new Font("Microsoft YaHei UI", 10F);
            inputSurfaceCE.Location = new Point(96, -1);
            inputSurfaceCE.Name = "inputSurfaceCE";
            inputSurfaceCE.Size = new Size(243, 34);
            inputSurfaceCE.TabIndex = 17;
            // 
            // labelSurfaceCE
            // 
            labelSurfaceCE.BackColor = Color.Transparent;
            labelSurfaceCE.Dock = DockStyle.Left;
            labelSurfaceCE.Font = new Font("Microsoft YaHei UI", 10F);
            labelSurfaceCE.LocalizationText = "SurfaceCE:";
            labelSurfaceCE.Location = new Point(0, 0);
            labelSurfaceCE.Name = "labelSurfaceCE";
            labelSurfaceCE.Size = new Size(90, 33);
            labelSurfaceCE.TabIndex = 16;
            labelSurfaceCE.Text = "表面CE：";
            labelSurfaceCE.TextAlign = ContentAlignment.MiddleRight;
            // 
            // panel3
            // 
            panel3.Back = Color.Transparent;
            panel3.BackColor = Color.Transparent;
            panel3.Controls.Add(inputSurfaceOP);
            panel3.Controls.Add(labelSurfaceOP);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(9, 39);
            panel3.Name = "panel3";
            panel3.Radius = 0;
            panel3.Size = new Size(342, 33);
            panel3.TabIndex = 14;
            // 
            // inputSurfaceOP
            // 
            inputSurfaceOP.Font = new Font("Microsoft YaHei UI", 10F);
            inputSurfaceOP.Location = new Point(96, -1);
            inputSurfaceOP.Name = "inputSurfaceOP";
            inputSurfaceOP.Size = new Size(243, 34);
            inputSurfaceOP.TabIndex = 17;
            // 
            // labelSurfaceOP
            // 
            labelSurfaceOP.BackColor = Color.Transparent;
            labelSurfaceOP.Dock = DockStyle.Left;
            labelSurfaceOP.Font = new Font("Microsoft YaHei UI", 10F);
            labelSurfaceOP.LocalizationText = "SurfaceOP:";
            labelSurfaceOP.Location = new Point(0, 0);
            labelSurfaceOP.Name = "labelSurfaceOP";
            labelSurfaceOP.Size = new Size(90, 33);
            labelSurfaceOP.TabIndex = 16;
            labelSurfaceOP.Text = "表面OP：";
            labelSurfaceOP.TextAlign = ContentAlignment.MiddleRight;
            // 
            // labelResultJudgeArea
            // 
            labelResultJudgeArea.BackColor = Color.Transparent;
            labelResultJudgeArea.Dock = DockStyle.Top;
            labelResultJudgeArea.Font = new Font("Microsoft YaHei UI", 15F, FontStyle.Bold);
            labelResultJudgeArea.ForeColor = Color.DarkGray;
            labelResultJudgeArea.LocalizationText = "Result Judgment Area";
            labelResultJudgeArea.Location = new Point(9, 9);
            labelResultJudgeArea.Name = "labelResultJudgeArea";
            labelResultJudgeArea.Size = new Size(342, 30);
            labelResultJudgeArea.TabIndex = 12;
            labelResultJudgeArea.Text = "结 果 判 定 区";
            labelResultJudgeArea.TextAlign = ContentAlignment.TopCenter;
            // 
            // blacknessMethod_OpenFileDialog
            // 
            blacknessMethod_OpenFileDialog.FileName = "file";
            // 
            // blacknessMethod_FileSystemWatcher
            // 
            blacknessMethod_FileSystemWatcher.EnableRaisingEvents = true;
            blacknessMethod_FileSystemWatcher.SynchronizingObject = this;
            // 
            // BlacknessMethod
            // 
            Controls.Add(flowLayoutPanel1);
            Controls.Add(header1);
            Font = new Font("Microsoft YaHei UI", 16F);
            Name = "BlacknessMethod";
            Size = new Size(1300, 634);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel5.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            panel6.ResumeLayout(false);
            panel18.ResumeLayout(false);
            panel17.ResumeLayout(false);
            panel16.ResumeLayout(false);
            panel15.ResumeLayout(false);
            panel14.ResumeLayout(false);
            panel8.ResumeLayout(false);
            panel13.ResumeLayout(false);
            panel13.PerformLayout();
            panel12.ResumeLayout(false);
            panel11.ResumeLayout(false);
            panel10.ResumeLayout(false);
            panel9.ResumeLayout(false);
            panel7.ResumeLayout(false);
            panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)blacknessMethod_FileSystemWatcher).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private AntdUI.PageHeader header1;
        private AntdUI.Panel panel1;
        private AntdUI.Label labelRenderAreaDescription;
        private AntdUI.Label lableRenderImageDisplayArea;
        private AntdUI.Avatar avatarRenderImage;
        private AntdUI.Panel panel2;
        private AntdUI.Button btnPredict;
        private AntdUI.Panel panel4;
        private AntdUI.Label labelOriginAreaDescription;
        private AntdUI.Label labelOriginImageDisplayArea;
        private AntdUI.Avatar avatarOriginImage;
        private AntdUI.Panel panel5;
        private AntdUI.Button btnUploadImage;
        private FlowLayoutPanel flowLayoutPanel1;
        private AntdUI.Panel panel6;
        private OpenFileDialog blacknessMethod_OpenFileDialog;
        private System.IO.FileSystemWatcher blacknessMethod_FileSystemWatcher;
        private AntdUI.Button btnCameraCapture;
        private AntdUI.Label labelResultJudgeArea;
        private AntdUI.Panel panel3;
        private AntdUI.Button btnSave;
        private AntdUI.Button button7;
        private AntdUI.Panel panel12;
        private AntdUI.Button button6;
        private AntdUI.Panel panel11;
        private AntdUI.Button button5;
        private AntdUI.Panel panel10;
        private AntdUI.Button button4;
        private AntdUI.Panel panel9;
        private AntdUI.Button button3;
        private AntdUI.Panel panel7;
        private AntdUI.Button button2;
        private AntdUI.Label labelSurfaceOP;
        private AntdUI.Input inputSurfaceOP;
        private AntdUI.Input inputInsideDR;
        private AntdUI.Label labelInsideDR;
        private AntdUI.Input inputInsideCE;
        private AntdUI.Label labelInsideCE;
        private AntdUI.Input inputInsideOP;
        private AntdUI.Label labelInsideOP;
        private AntdUI.Input inputSurfaceDR;
        private AntdUI.Label labelSurfaceDR;
        private AntdUI.Input inputSurfaceCE;
        private AntdUI.Label labelSurfaceCE;
        private AntdUI.Panel panel13;
        private AntdUI.Radio radioResultOK;
        private AntdUI.Radio radioResultNG;
        private AntdUI.Divider divider2;
        private AntdUI.Panel panel16;
        private AntdUI.Input inputSize;
        private AntdUI.Label labelSize;
        private AntdUI.Panel panel15;
        private AntdUI.Input inputCoilNumber;
        private AntdUI.Label labelCoilNumber;
        private AntdUI.Panel panel14;
        private AntdUI.Label labelTestNo;
        private AntdUI.Panel panel8;
        private AntdUI.Label labelWorkGroup;
        private AntdUI.Panel panel17;
        private AntdUI.Select selectWorkGroup;
        private AntdUI.Select selectTestNo;
        private AntdUI.Button btnCameraSetting;
        private AntdUI.Button btnCameraRecover;
        private AntdUI.Button btnClear;
        private AntdUI.Button btn;
        private AntdUI.Button btnHistory;
        private AntdUI.Button btnPre;
        private AntdUI.Button btnNext;
        private AntdUI.Button btnPrint;
        private AntdUI.Button btnUpload;
        private AntdUI.Label label3;
        private AntdUI.Panel panel18;
        private AntdUI.Input inputAnalyst;
        private AntdUI.Label labelAnalyst;
        private AntdUI.Button button8;
        private AntdUI.Button btnSetScale;
        private AntdUI.Select selectScale;
    }
}