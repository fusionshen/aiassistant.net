namespace AI_Assistant_Win
{
    partial class Login
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
            tablePanel = new System.Windows.Forms.TableLayoutPanel();
            inputPassword = new AntdUI.Input();
            inputUsername = new AntdUI.Input();
            label1 = new AntdUI.Label();
            label3 = new AntdUI.Label();
            tablePanel.SuspendLayout();
            SuspendLayout();
            // 
            // tablePanel
            // 
            tablePanel.ColumnCount = 2;
            tablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 186F));
            tablePanel.Controls.Add(inputPassword, 1, 1);
            tablePanel.Controls.Add(inputUsername, 1, 0);
            tablePanel.Controls.Add(label1, 0, 0);
            tablePanel.Controls.Add(label3, 0, 1);
            tablePanel.Dock = System.Windows.Forms.DockStyle.Top;
            tablePanel.Location = new System.Drawing.Point(0, 0);
            tablePanel.Name = "tablePanel";
            tablePanel.RowCount = 2;
            tablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            tablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            tablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            tablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            tablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            tablePanel.Size = new System.Drawing.Size(272, 92);
            tablePanel.TabIndex = 0;
            // 
            // inputPassword
            // 
            inputPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            inputPassword.Location = new System.Drawing.Point(89, 49);
            inputPassword.Name = "inputPassword";
            inputPassword.PasswordChar = '●';
            inputPassword.PlaceholderText = "请输入密码";
            inputPassword.Radius = 1;
            inputPassword.Size = new System.Drawing.Size(180, 40);
            inputPassword.TabIndex = 4;
            // 
            // inputUsername
            // 
            inputUsername.Dock = System.Windows.Forms.DockStyle.Fill;
            inputUsername.LocalizationPlaceholderText = "Input.{id}";
            inputUsername.Location = new System.Drawing.Point(89, 3);
            inputUsername.Name = "inputUsername";
            inputUsername.PlaceholderText = "请输入账号";
            inputUsername.Radius = 1;
            inputUsername.Size = new System.Drawing.Size(180, 40);
            inputUsername.TabIndex = 2;
            // 
            // label1
            // 
            label1.Dock = System.Windows.Forms.DockStyle.Fill;
            label1.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            label1.LocalizationText = "Accout";
            label1.Location = new System.Drawing.Point(3, 3);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(80, 40);
            label1.TabIndex = 0;
            label1.Text = "账号";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            label3.Dock = System.Windows.Forms.DockStyle.Fill;
            label3.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            label3.LocalizationText = "Password";
            label3.Location = new System.Drawing.Point(3, 49);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(80, 40);
            label3.TabIndex = 0;
            label3.Text = "密码";
            label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Login
            // 
            Controls.Add(tablePanel);
            Name = "Login";
            Size = new System.Drawing.Size(272, 96);
            tablePanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tablePanel;
        private AntdUI.Label label1;
        private AntdUI.Label label3;
        private AntdUI.Input inputUsername;
        private AntdUI.Input inputPassword;
    }
}