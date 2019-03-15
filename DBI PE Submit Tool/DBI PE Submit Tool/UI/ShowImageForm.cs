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
        List<Image> images = new List<Image>();
        public ShowImageForm(string[] images)
        {
            InitializeComponent();
            foreach (var image in images)
                this.images.Add(Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, image)));
            Show();
            ShowImage();
        }
        private static Image ResizeImage(Image imgToResize, Size size) => new Bitmap(imgToResize, size);

        private void ShowImage()
        {
            flowLayoutPanel.Invoke((MethodInvoker)(() =>
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
            }));
        }

        private void LayoutResize(object sender, EventArgs e) => ShowImage();
    }
}
