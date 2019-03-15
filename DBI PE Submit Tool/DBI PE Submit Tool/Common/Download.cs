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
        public static void DownloadFrom(string url, string token)
        {
            try
            {
                Uri uri = new Uri(url);
                //string filename = System.IO.Path.GetFileName(uri.LocalPath);
                
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";

                    var parameters = new System.Collections.Specialized.NameValueCollection
                    {
                        { "token", token }
                    };

                    using (var stream = client.OpenRead(uri))
                    {
                        string header_contentDisposition = client.ResponseHeaders["content-disposition"];
                        string filename = new ContentDisposition(header_contentDisposition).FileName;

                        // Open windown to choose the path
                        SaveFileDialog locationChooser = new SaveFileDialog
                        {
                            FileName = filename,
                            InitialDirectory = Convert.ToString(Environment.SpecialFolder.DesktopDirectory)
                        };
                        ;
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

        // Download with POST method, goes with token
        public static void PostDownloadMaterial(string url, string token)
        {
            try
            {
                Uri uri = new Uri(url);

                HttpWebRequest request = null;
                request = (HttpWebRequest)WebRequest.Create(uri);
                
                request.ContentType = "application/x-www-form-urlencoded";
                request.Method = "POST";

                // POST with params
                string postString = string.Format("token={0}", token);
                request.ContentLength = postString.Length;
                StreamWriter requestWriter = new StreamWriter(request.GetRequestStream());
                requestWriter.Write(postString);
                requestWriter.Close();

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        string header_contentDisposition = response.Headers["content-disposition"];
                        string filename = new ContentDisposition(header_contentDisposition).FileName;
                        using (var stream = response.GetResponseStream())
                        {
                            // Open windown to choose the path
                            SaveFileDialog locationChooser = new SaveFileDialog
                            {
                                FileName = filename,
                                InitialDirectory = Convert.ToString(Environment.SpecialFolder.DesktopDirectory)
                            };
                            ;
                            locationChooser.FilterIndex = 1;
                            locationChooser.Filter = "All files (*.*)|*.*|Zip files (*.rar,*.zip)|*.rar;*.zip;";

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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
