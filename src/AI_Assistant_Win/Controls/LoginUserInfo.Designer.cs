namespace AI_Assistant_Win
{
    partial class LoginUserInfo
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
            panel4 = new AntdUI.Panel();
            labelDescription = new System.Windows.Forms.Label();
            labelLastLoginTime = new System.Windows.Forms.Label();
            labelPhone = new System.Windows.Forms.Label();
            labelDepartment = new System.Windows.Forms.Label();
            labelUsername = new System.Windows.Forms.Label();
            avatar = new AntdUI.Avatar();
            panel5 = new AntdUI.Panel();
            btnSignout = new AntdUI.Button();
            panel4.SuspendLayout();
            panel5.SuspendLayout();
            SuspendLayout();
            // 
            // panel4
            // 
            panel4.ArrowSize = 10;
            panel4.Controls.Add(labelDescription);
            panel4.Controls.Add(labelLastLoginTime);
            panel4.Controls.Add(labelPhone);
            panel4.Controls.Add(labelDepartment);
            panel4.Controls.Add(labelUsername);
            panel4.Controls.Add(avatar);
            panel4.Controls.Add(panel5);
            panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            panel4.Location = new System.Drawing.Point(0, 0);
            panel4.Name = "panel4";
            panel4.Padding = new System.Windows.Forms.Padding(14);
            panel4.Radius = 10;
            panel4.ShadowOpacityAnimation = true;
            panel4.Size = new System.Drawing.Size(278, 423);
            panel4.TabIndex = 20;
            // 
            // labelDescription
            // 
            labelDescription.BackColor = System.Drawing.Color.Transparent;
            labelDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            labelDescription.Font = new System.Drawing.Font("方正舒体", 10F, System.Drawing.FontStyle.Underline);
            labelDescription.Location = new System.Drawing.Point(14, 310);
            labelDescription.Name = "labelDescription";
            labelDescription.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            labelDescription.Size = new System.Drawing.Size(250, 59);
            labelDescription.TabIndex = 16;
            labelDescription.Text = "无简介";
            // 
            // labelLastLoginTime
            // 
            labelLastLoginTime.BackColor = System.Drawing.Color.Transparent;
            labelLastLoginTime.Dock = System.Windows.Forms.DockStyle.Top;
            labelLastLoginTime.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            labelLastLoginTime.Location = new System.Drawing.Point(14, 267);
            labelLastLoginTime.Name = "labelLastLoginTime";
            labelLastLoginTime.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            labelLastLoginTime.Size = new System.Drawing.Size(250, 43);
            labelLastLoginTime.TabIndex = 15;
            labelLastLoginTime.Text = "上次登录：";
            labelLastLoginTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelPhone
            // 
            labelPhone.BackColor = System.Drawing.Color.Transparent;
            labelPhone.Dock = System.Windows.Forms.DockStyle.Top;
            labelPhone.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            labelPhone.Location = new System.Drawing.Point(14, 224);
            labelPhone.Name = "labelPhone";
            labelPhone.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            labelPhone.Size = new System.Drawing.Size(250, 43);
            labelPhone.TabIndex = 14;
            labelPhone.Text = "手机：";
            labelPhone.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelDepartment
            // 
            labelDepartment.BackColor = System.Drawing.Color.Transparent;
            labelDepartment.Dock = System.Windows.Forms.DockStyle.Top;
            labelDepartment.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            labelDepartment.Location = new System.Drawing.Point(14, 190);
            labelDepartment.Name = "labelDepartment";
            labelDepartment.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            labelDepartment.Size = new System.Drawing.Size(250, 34);
            labelDepartment.TabIndex = 12;
            labelDepartment.Text = "部门：";
            labelDepartment.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelUsername
            // 
            labelUsername.BackColor = System.Drawing.Color.Transparent;
            labelUsername.Dock = System.Windows.Forms.DockStyle.Top;
            labelUsername.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            labelUsername.Location = new System.Drawing.Point(14, 151);
            labelUsername.Name = "labelUsername";
            labelUsername.Size = new System.Drawing.Size(250, 39);
            labelUsername.TabIndex = 11;
            labelUsername.Text = "10001-Admin";
            labelUsername.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // avatar
            // 
            avatar.Dock = System.Windows.Forms.DockStyle.Top;
            avatar.Image = Properties.Resources.img1;
            avatar.Location = new System.Drawing.Point(14, 14);
            avatar.Name = "avatar";
            avatar.Radius = 6;
            avatar.Size = new System.Drawing.Size(250, 137);
            avatar.TabIndex = 9;
            // 
            // panel5
            // 
            panel5.Back = System.Drawing.Color.Transparent;
            panel5.BackColor = System.Drawing.Color.Transparent;
            panel5.Controls.Add(btnSignout);
            panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
            panel5.Location = new System.Drawing.Point(14, 369);
            panel5.Name = "panel5";
            panel5.Radius = 0;
            panel5.Size = new System.Drawing.Size(250, 40);
            panel5.TabIndex = 13;
            // 
            // btnSignout
            // 
            btnSignout.AutoSizeMode = AntdUI.TAutoSize.Auto;
            btnSignout.Dock = System.Windows.Forms.DockStyle.Right;
            btnSignout.IconSvg = "LogoutOutlined";
            btnSignout.LoadingWaveVertical = true;
            btnSignout.Location = new System.Drawing.Point(209, 0);
            btnSignout.Name = "btnSignout";
            btnSignout.Shape = AntdUI.TShape.Circle;
            btnSignout.Size = new System.Drawing.Size(41, 41);
            btnSignout.TabIndex = 1;
            btnSignout.Type = AntdUI.TTypeMini.Error;
            btnSignout.Click += BtnSignout_Click;
            // 
            // LoginUserInfo
            // 
            Controls.Add(panel4);
            Name = "LoginUserInfo";
            Size = new System.Drawing.Size(278, 423);
            panel4.ResumeLayout(false);
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.Panel panel4;
        private System.Windows.Forms.Label labelDepartment;
        private System.Windows.Forms.Label labelUsername;
        private AntdUI.Avatar avatar;
        private AntdUI.Panel panel5;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.Label labelLastLoginTime;
        private System.Windows.Forms.Label labelPhone;
        private AntdUI.Button btnSignout;
    }
}