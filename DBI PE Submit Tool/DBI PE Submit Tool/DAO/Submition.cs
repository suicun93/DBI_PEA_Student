using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DBI_PE_Submit_Tool.DAO
{
    [Serializable]
    public class Submition
    {
        public Submition()
        {
            ListAnswer = new List<string>();
        }
        public string StudentID { get; set; }
        public string TestName { get; set; }
        public string PaperNo { get; set; }
        public List<string> ListAnswer { get; set; }

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
                string json = JsonConvert.SerializeObject(this);
                string encodedJson = Common.Coding.Encode(json);
                var dir = TestName;
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                File.WriteAllText(Path.Combine(dir, StudentID + ".dat") , encodedJson);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public Submition RestoreFromFile()
        {
            // Restore when students continue doing their exam.
            try
            {
                var dir = TestName;
                if (Directory.Exists(dir))
                {
                    string json = File.ReadAllText(Path.Combine(dir, StudentID + ".dat"));
                    string decodedJson = Common.Coding.Decode(json);
                    Submition submition = JsonConvert.DeserializeObject<Submition>(decodedJson);
                    return submition;
                }
                throw new Exception("No file was found!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }
    }
}
