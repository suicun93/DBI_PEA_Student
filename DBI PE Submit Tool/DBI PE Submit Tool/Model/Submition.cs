using System;
using System.Collections.Generic;
using System.IO;
using DBI_PE_Submit_Tool.Common;
using System.Windows.Forms;

namespace DBI_PE_Submit_Tool.Model
{
    [Serializable]
    public class Submition
    {
        public string StudentID { get; set; }
        public string TestName { get; set; }
        public string PaperNo { get; set; }
        public List<string> ListAnswer { get; set; }
        public SecureJsonSerializer<Submition> secureJsonSerializer;

        public Submition(string testName, string studentID, string paperNo)
        {
            TestName = testName;
            StudentID = studentID;
            PaperNo = paperNo;
            ListAnswer = new List<string>();
        }

        public void register()
        {
            var dir = TestName;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            secureJsonSerializer = new SecureJsonSerializer<Submition>(Path.Combine(dir, StudentID + ".dat"));
        }

        public void AddAnswer(string answer)
        {
            ListAnswer.Add(answer);
        }

        public void ClearAnswer()
        {
            ListAnswer.Clear();
        }

        public void SaveToLocal()
        {
            // When drafting answers from student, we save it to local
            try
            {
                // Write file to path TestName/StudentID.dat
                secureJsonSerializer.Save(this);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        // Restore when students continue doing their exam.
        public void Restore()
        {
            try
            {
                var dir = TestName;
                if (Directory.Exists(dir))
                {
                    Submition submition;
                    try
                    {
                        submition = secureJsonSerializer.Load();
                        // Load successfully
                        this.ListAnswer = new List<string>();
                        foreach (var answer in submition.ListAnswer)
                        {
                            this.ListAnswer.Add(answer);
                        }
                    }
                    catch (Exception)
                    {
                        // Load fail
                        MessageBox.Show("Restore fail");
                        // Create new list
                        for (int i = 0; i < 10; i++)
                        {
                            this.ListAnswer.Add("");
                        }
                    }
                }
                else
                {
                    throw new Exception("No file was found");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
