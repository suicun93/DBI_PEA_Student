namespace DBI_PE_Submit_Tool
{
    partial class LoginForm
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
            this.label = new System.Windows.Forms.Label();
            this.exitButton = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.examCodeTextBox = new System.Windows.Forms.TextBox();
            this.paperNoTextBox = new System.Windows.Forms.TextBox();
            this.usernameTextBox = new System.Windows.Forms.TextBox();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.domainTextBox = new System.Windows.Forms.TextBox();
            this.loginButton = new System.Windows.Forms.Button();
            this.restoreCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label.ForeColor = System.Drawing.Color.DarkRed;
            this.label.Location = new System.Drawing.Point(29, 223);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(391, 16);
            this.label.TabIndex = 14;
            this.label.Text = "Register the exam may take some minutes! Please wait!";
            // 
            // exitButton
            // 
            this.exitButton.Location = new System.Drawing.Point(269, 187);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(118, 23);
            this.exitButton.TabIndex = 13;
            this.exitButton.Text = "Exit";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(37, 53);
            this.label9.Margin = new System.Windows.Forms.Padding(3);
            this.label9.Name = "label9";
            this.label9.Padding = new System.Windows.Forms.Padding(3);
            this.label9.Size = new System.Drawing.Size(61, 19);
            this.label9.TabIndex = 1;
            this.label9.Text = "Paper No:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(46, 149);
            this.label10.Margin = new System.Windows.Forms.Padding(3);
            this.label10.Name = "label10";
            this.label10.Padding = new System.Windows.Forms.Padding(3);
            this.label10.Size = new System.Drawing.Size(52, 19);
            this.label10.TabIndex = 4;
            this.label10.Text = "Domain:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(34, 85);
            this.label11.Margin = new System.Windows.Forms.Padding(3);
            this.label11.Name = "label11";
            this.label11.Padding = new System.Windows.Forms.Padding(3);
            this.label11.Size = new System.Drawing.Size(64, 19);
            this.label11.TabIndex = 2;
            this.label11.Text = "Username:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(36, 117);
            this.label12.Margin = new System.Windows.Forms.Padding(3);
            this.label12.Name = "label12";
            this.label12.Padding = new System.Windows.Forms.Padding(3);
            this.label12.Size = new System.Drawing.Size(62, 19);
            this.label12.TabIndex = 3;
            this.label12.Text = "Password:";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 21);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(3);
            this.label1.Size = new System.Drawing.Size(69, 19);
            this.label1.TabIndex = 15;
            this.label1.Text = "Exam code:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // examCodeTextBox
            // 
            this.examCodeTextBox.Location = new System.Drawing.Point(117, 20);
            this.examCodeTextBox.Name = "examCodeTextBox";
            this.examCodeTextBox.Size = new System.Drawing.Size(271, 20);
            this.examCodeTextBox.TabIndex = 16;
            this.examCodeTextBox.Text = "PE_DBI_1_SP2019_567899";
            // 
            // paperNoTextBox
            // 
            this.paperNoTextBox.Location = new System.Drawing.Point(117, 52);
            this.paperNoTextBox.Name = "paperNoTextBox";
            this.paperNoTextBox.Size = new System.Drawing.Size(271, 20);
            this.paperNoTextBox.TabIndex = 17;
            this.paperNoTextBox.Text = "1";
            // 
            // usernameTextBox
            // 
            this.usernameTextBox.Location = new System.Drawing.Point(117, 84);
            this.usernameTextBox.Name = "usernameTextBox";
            this.usernameTextBox.Size = new System.Drawing.Size(271, 20);
            this.usernameTextBox.TabIndex = 18;
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Location = new System.Drawing.Point(117, 116);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.Size = new System.Drawing.Size(271, 20);
            this.passwordTextBox.TabIndex = 19;
            this.passwordTextBox.UseSystemPasswordChar = true;
            // 
            // domainTextBox
            // 
            this.domainTextBox.Enabled = false;
            this.domainTextBox.Location = new System.Drawing.Point(117, 148);
            this.domainTextBox.Name = "domainTextBox";
            this.domainTextBox.Size = new System.Drawing.Size(271, 20);
            this.domainTextBox.TabIndex = 20;
            this.domainTextBox.Text = "34.215.75.169";
            // 
            // loginButton
            // 
            this.loginButton.Location = new System.Drawing.Point(73, 187);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(118, 23);
            this.loginButton.TabIndex = 21;
            this.loginButton.Text = "Login";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.BtnLogin_Click);
            // 
            // restoreCheckBox
            // 
            this.restoreCheckBox.AutoSize = true;
            this.restoreCheckBox.Location = new System.Drawing.Point(405, 23);
            this.restoreCheckBox.Name = "restoreCheckBox";
            this.restoreCheckBox.Size = new System.Drawing.Size(15, 14);
            this.restoreCheckBox.TabIndex = 22;
            this.restoreCheckBox.UseVisualStyleBackColor = true;
            this.restoreCheckBox.Visible = false;
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(448, 251);
            this.Controls.Add(this.restoreCheckBox);
            this.Controls.Add(this.loginButton);
            this.Controls.Add(this.domainTextBox);
            this.Controls.Add(this.passwordTextBox);
            this.Controls.Add(this.usernameTextBox);
            this.Controls.Add(this.paperNoTextBox);
            this.Controls.Add(this.examCodeTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label);
            this.Controls.Add(this.exitButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "LoginForm";
            this.Text = "PEA Login";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LoginForm_Closing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox examCodeTextBox;
        private System.Windows.Forms.TextBox paperNoTextBox;
        private System.Windows.Forms.TextBox usernameTextBox;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.TextBox domainTextBox;
        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.CheckBox restoreCheckBox;
    }
}

