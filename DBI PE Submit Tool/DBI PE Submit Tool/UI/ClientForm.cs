using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using DBI_PE_Submit_Tool.DAO;

namespace DBI_PE_Submit_Tool
{
    public partial class ClientForm : Form
    {
        // Information about student: studentID, paperNo, testName, urlDB to get material, listAnswer;
        private string UrlDB;
        private List<RichTextBox> ListAnswer = new List<RichTextBox>();
        private Submition Submition = new Submition();

        public ClientForm(string TestName, string PaperNo, string StudentName)
        {
            InitializeComponent();
            if (String.IsNullOrEmpty(TestName) || String.IsNullOrEmpty(PaperNo) || String.IsNullOrEmpty(StudentName))
            {
                MessageBox.Show("Empty Information");
                Application.Exit();
            }
            else
            {
                // TODO: Call API to get question here!
                // Now I mock up an url like 'https://www.w3schools.com/w3images/mac.jpg' to download
                UrlDB = "https://www.w3schools.com/w3images/mac.jpg";
                studentLabel.Text = this.Submition.StudentID = StudentName;
                paperNoLabel.Text = this.Submition.PaperNo = PaperNo;
                testNameLabel.Text = this.Submition.TestName = TestName;
                SetUpUI();
            }
        }

        private void SetUpUI()
        {
            // Add all answer rich text box to list to add event draft answer on text change
            ListAnswer.Add(q1RichTextBox);
            ListAnswer.Add(q2RichTextBox);
            ListAnswer.Add(q3RichTextBox);
            ListAnswer.Add(q4RichTextBox);
            ListAnswer.Add(q5RichTextBox);
            ListAnswer.Add(q6RichTextBox);
            ListAnswer.Add(q7RichTextBox);
            ListAnswer.Add(q8RichTextBox);
            ListAnswer.Add(q9RichTextBox);
            ListAnswer.Add(q10RichTextBox);
            // Add event
            for (int i = 0; i < ListAnswer.Count; i++)
            {
                ListAnswer[i].TextChanged += new EventHandler(DraftAnswers);
            }
        }

        // Handle when user close window -> application will be closed.
        private void ClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        // Help Button CLick
        private void HelpButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not supported yet");
        }

        // Download Material Button CLick
        private void DownloadMaterialButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Open windown to choose the path
                SaveFileDialog locationChooser = new SaveFileDialog();
                locationChooser.FileName = "hi.jpg";  // This should be replaced by 'DB.rar' or something like that
                locationChooser.InitialDirectory = Convert.ToString(Environment.SpecialFolder.DesktopDirectory); ;
                locationChooser.FilterIndex = 1;
                if (locationChooser.ShowDialog() == DialogResult.OK)
                {
                    WebClient client = new WebClient();
                    client.DownloadFile(UrlDB, locationChooser.FileName);
                    locationMaterialTextBox.Text = locationChooser.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Submit Button CLick
        private void SubmitButton_Click(object sender, EventArgs e)
        {
            if (readyToFinishCheckBox.Checked)
            {
                SumUpAnswer();
                // TODO: Call API to submit all answer, test name, studentID, paper number
                // CallAPIToSubmit()
                MessageBox.Show(Submition.StudentID + " have submitted something");
            }
            else
            {
                MessageBox.Show("You have to confirmed to finish this exam.");
            }
        }

        // Draft Student's answer every time they edit their answer
        private void DraftAnswers(object sender, System.EventArgs e)
        {
            // Get all answer
            SumUpAnswer();
            // TODO: Call API to draft all answer, test name, student's rollID, paper number
            // CallAPIToDraft()
        }

        private void SumUpAnswer()
        {
            Submition.ClearAnswer();
            foreach (RichTextBox richTextBox in ListAnswer)
            {
                Submition.AddAnswer(richTextBox.Text);
            }
            Submition.SaveToLocal();
        }
    }
}
