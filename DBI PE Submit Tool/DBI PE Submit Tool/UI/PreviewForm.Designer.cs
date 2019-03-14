namespace DBI_PE_Submit_Tool.UI
{
    partial class PreviewForm
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
            this.previewTextBox = new System.Windows.Forms.RichTextBox();
            this.finishPreviewButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // previewTextBox
            // 
            this.previewTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.previewTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.previewTextBox.Location = new System.Drawing.Point(12, 12);
            this.previewTextBox.Name = "previewTextBox";
            this.previewTextBox.ReadOnly = true;
            this.previewTextBox.Size = new System.Drawing.Size(404, 373);
            this.previewTextBox.TabIndex = 0;
            this.previewTextBox.Text = "";
            // 
            // finishPreviewButton
            // 
            this.finishPreviewButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.finishPreviewButton.Location = new System.Drawing.Point(162, 399);
            this.finishPreviewButton.Name = "finishPreviewButton";
            this.finishPreviewButton.Size = new System.Drawing.Size(105, 23);
            this.finishPreviewButton.TabIndex = 1;
            this.finishPreviewButton.Text = "Finish Preview";
            this.finishPreviewButton.UseVisualStyleBackColor = true;
            this.finishPreviewButton.Click += new System.EventHandler(this.FinishPreviewButton_Click);
            // 
            // PreviewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 436);
            this.Controls.Add(this.finishPreviewButton);
            this.Controls.Add(this.previewTextBox);
            this.Name = "PreviewForm";
            this.Text = "Preview";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PreviewForm_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox previewTextBox;
        private System.Windows.Forms.Button finishPreviewButton;
    }
}