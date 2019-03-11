using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
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
                //string filename = System.IO.Path.GetFileName(uri.LocalPath);
                
                using (WebClient client = new WebClient())
                {
                    using (var stream = client.OpenRead(uri))
                    {
                        string header_contentDisposition = client.ResponseHeaders["content-disposition"];
                        string filename = new ContentDisposition(header_contentDisposition).FileName;

                        textBox.Text = filename; // ???

                        // Open windown to choose the path
                        SaveFileDialog locationChooser = new SaveFileDialog();
                        locationChooser.FileName = filename;
                        locationChooser.InitialDirectory = Convert.ToString(Environment.SpecialFolder.DesktopDirectory); ;
                        locationChooser.FilterIndex = 1;
                        locationChooser.Filter = "Zip files (*.rar,*.zip)|*.rar;*.zip;*.jpg|All files (*.*)|*.*";

                        if (locationChooser.ShowDialog() == DialogResult.OK)
                        {
                            using (var file = File.Create(locationChooser.FileName))
                            {
                                stream.CopyTo(file);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }


}
