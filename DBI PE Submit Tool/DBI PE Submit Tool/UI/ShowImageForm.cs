using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DBI_PE_Submit_Tool.UI
{
    public partial class ShowImageForm : Form
    {
        //string root = @"C:\Users\hoangduc\Desktop\Project\Student\DBI PE Submit Tool\DBI PE Submit Tool\";
        private Action Clear = null;
        List<Image> images;
        public ShowImageForm(List<Image> images, Action clear)
        {
            InitializeComponent();
            this.images = images;
            Clear = clear;
        }
        private static Image ResizeImage(Image imgToResize, Size size) => new Bitmap(imgToResize, size);

        private void ShowImage()
        {
            if (flowLayoutPanel != null)
            {
                flowLayoutPanel?.Invoke((MethodInvoker)(() =>
                {
                    if (flowLayoutPanel != null)
                    {
                        flowLayoutPanel.SizeChanged -= LayoutResize;
                        flowLayoutPanel.Controls.Clear();
                        var sizeBar = SystemInformation.VerticalScrollBarWidth + 22;
                        foreach (Image image in images)
                        {
                            int ratio = image.Height / image.Width;
                            PictureBox p = new PictureBox
                            {
                                Width = flowLayoutPanel.Width,
                                Height = Width * ratio,
                                Image = ResizeImage(image, new Size(Width - sizeBar, Height - sizeBar)),
                                SizeMode = PictureBoxSizeMode.AutoSize,
                            };
                            flowLayoutPanel.Controls.Add(p);
                        }
                        flowLayoutPanel.SizeChanged += LayoutResize;
                    }
                }));
            }
        }

        private void LayoutResize(object sender, EventArgs e) => ShowImage();

        private void ShowImageForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true; // this cancels the close event.
            Clear();
        }

        private void ShowImageForm_Load(object sender, EventArgs e)
        {
            ShowImage();
        }
    }
}
