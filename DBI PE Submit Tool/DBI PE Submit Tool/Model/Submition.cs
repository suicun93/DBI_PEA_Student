using DBI_PE_Submit_Tool.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace DBI_PE_Submit_Tool.Model
{
    [Serializable]
    public class Submition
    {
        public string StudentID { get; set; }
        public string ExamCode { get; set; }
        public string PaperNo { get; set; }
        public List<string> ListAnswer { get; set; }
        [JsonIgnore]
        public SecureJsonSerializer<Submition> secureJsonSerializer;

        private readonly string apiUrl = Constant.API_URL;
        private readonly string Token;

        public Submition()
        {

        }

        public Submition(string examCode, string studentID, string paperNo, string token)
        {
            ExamCode = examCode;
            StudentID = studentID;
            PaperNo = paperNo;
            Token = token;
            ListAnswer = new List<string>();
        }

        public void Register()
        {
            var dir = ExamCode;
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            secureJsonSerializer = new SecureJsonSerializer<Submition>(Path.Combine(dir, StudentID + ".dat"));
        }

        public void AddAnswer(string answer) => ListAnswer.Add(answer);

        public void ClearAnswer() => ListAnswer.Clear();

        public void SaveToLocal()
        {
            // When drafting answers from student, we save it to local
            try
            {
                // Write file to path ExamCode/StudentID.dat
                secureJsonSerializer.Save(this);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public delegate void DoAfterSubmit(string text);

        // call POST '/submit' api.
        public bool Submit(DoAfterSubmit doAfterSubmit)
        {
            string submitUrl = apiUrl + "/submit";
            Uri uri = new Uri(submitUrl);
            using (WebClient client = new WebClient())
            {
                try
                {
                    client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    var parameters = new System.Collections.Specialized.NameValueCollection
                    {
                        { "username", StudentID },
                        { "examCode", ExamCode },
                        { "paperNo", PaperNo },
                        { "token", Token }
                    };

                    var data = JsonConvert.SerializeObject(this);
                    parameters.Add("answers", data);

                    byte[] responseBytes = client.UploadValues(uri, "POST", parameters);
                    string responseBody = System.Text.Encoding.UTF8.GetString(responseBytes);
                    doAfterSubmit(responseBody);
                    return true;
                }
                catch (Exception e)
                {
                    doAfterSubmit(e.Message);
                }
            }
            return false;
        }

        // Restore when students continue doing their exam.
        public void Restore()
        {
            try
            {
                var dir = ExamCode;
                if (Directory.Exists(dir))
                {
                    Submition submition;
                    try
                    {
                        submition = secureJsonSerializer.Load();
                        // Load successfully
                        ListAnswer = new List<string>();
                        foreach (var answer in submition.ListAnswer)
                        {
                            ListAnswer.Add(answer);
                        }
                    }
                    catch (Exception)
                    {
                        // Load fail
                        MessageBox.Show("Restore fail");
                        // Create new list
                        for (int i = 0; i < 10; i++)
                        {
                            ListAnswer.Add("");
                        }
                    }
                }
                else
                    throw new Exception("No file was found");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
