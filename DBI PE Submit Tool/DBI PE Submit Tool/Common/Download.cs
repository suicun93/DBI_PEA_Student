using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace DBI_PE_Submit_Tool.Common
{
    class Download
    {
        public static void DownloadFrom(string url, TextBox textBox)
        {
            try
            {
                // Get file name
                Uri uri = new Uri(url);
                string filename = System.IO.Path.GetFileName(uri.LocalPath);
                // Open windown to choose the path
                SaveFileDialog locationChooser = new SaveFileDialog();
                locationChooser.FileName = filename;  // This should be replaced by 'DB.rar' or something like that
                locationChooser.InitialDirectory = Convert.ToString(Environment.SpecialFolder.DesktopDirectory); ;
                locationChooser.FilterIndex = 1;
                locationChooser.Filter = "Zip files (*.rar,*.zip)|*.rar;*.zip;*.jpg|All files (*.*)|*.*";
                if (locationChooser.ShowDialog() == DialogResult.OK)
                {
                    WebClient client = new WebClient();
                    client.DownloadFile(url, locationChooser.FileName);
                    textBox.Text = locationChooser.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }


}
