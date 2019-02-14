using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DBI_PE_Submit_Tool.DAO;
using DBI_PE_Submit_Tool.Common;

namespace DBI_PE_Submit_Tool
{
    public partial class ClientForm : Form
    {
        // Information about student: studentID, paperNo, testName, urlDB to get material, listAnswer;
        private string UrlDB;
        private List<RichTextBox> ListAnswer = new List<RichTextBox>();
        private Submition submition = new Submition();
        // Merge draft and submit to 1 method with a variable named "forDraft"
        private bool forDraft = true, forSubmit = false; 
        // Make student preview their answers before submitting
        private bool previewed = false; 
        public string UrlDBToDownload { get => UrlDB; set => UrlDB = value; }

        public ClientForm(string TestName, string PaperNo, string StudentName, bool restored)
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
                // Now I mock up an url to download (an image from w3school)
                UrlDBToDownload = "https://www.w3schools.com/w3images/mac.jpg";
                studentLabel.Text = submition.StudentID = StudentName;
                paperNoLabel.Text = submition.PaperNo = PaperNo;
                testNameLabel.Text = submition.TestName = TestName;
                SetupUI(restored);
            }
        }

        /// <summary>
        ///     Add all answer rich text box to list to add event draft answer on text change
        /// </summary>
        private void SetupUI(bool restored)
        {
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

            // Check restore and restore answers
            if (restored)
            {
                // If student continue from break point, restore answers for them
                submition = Submition.Restore(submition.TestName, submition.StudentID);
                for (int i = 0; i < 10; i++)
                {
                    ListAnswer[i].Text = submition.ListAnswer[i];
                }
            }

            // Add event to draft every time answers changed
            for (int i = 0; i < ListAnswer.Count; i++)
            {
                ListAnswer[i].TextChanged += new EventHandler(DraftAnswers);
            }
        }

        private void HelpButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You should preview before submitting.");
        }

        /// <summary>
        ///     Preview to re-check 10 answers
        /// </summary>
        private void PreviewButton_Click(object sender, EventArgs e)
        {
            // List answers to preview
            submition = Submition.Restore(submition.TestName, submition.StudentID);
            if (submition == null)
            {
                MessageBox.Show("Restore failed");
            }
            else
            {
                int i = 0;
                string answers = "";
                foreach (string answer in submition.ListAnswer)
                {
                    i++;
                    answers += "Question " + i + "\n\t" + (String.IsNullOrEmpty(answer) ? "(empty)" : answer) + "\n";
                }
                MessageBox.Show(answers);
                previewed = true;
            }
        }

        private void DownloadMaterialButton_Click(object sender, EventArgs e)
        {
            Download.DownloadFrom(UrlDBToDownload, locationMaterialTextBox);
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            if (!previewed)
                MessageBox.Show("You should preview before submitting.");
            else
                if (readyToFinishCheckBox.Checked)
                SumUpAnswer(forSubmit);
            else
                MessageBox.Show("You have to confirmed to finish this exam.");
        }

        /// <summary>
        ///     Draft Student's answer every time they edit their answer
        /// </summary>
        private void DraftAnswers(object sender, System.EventArgs e)
        {
            // Get all answer
            SumUpAnswer(forDraft);
        }

        /// <summary>
        ///     Sum Up Answers and save to local
        /// </summary>
        /// <param name="forDraft"> if draft -> call API to draft 
        ///                         if submit -> call API to submit 
        /// </param>
        private void SumUpAnswer(bool forDraft)
        {
            // Change UI Draft Status UI 
            draftStatusLabel.Text = "Draft Status: N/A";
            draftStatusLabel.ForeColor = Color.Red;
            // Process
            previewed = false;
            submition.ClearAnswer();
            foreach (RichTextBox richTextBox in ListAnswer)
            {
                submition.AddAnswer(richTextBox.Text);
            }
            submition.SaveToLocal();
            if (forDraft)
            {
                // TODO: Call API to draft all answer, test name, student's rollID, paper number
                // CallAPIToDraft()
                // Change UI Draft Status UI to draft success
                draftStatusLabel.Text = "Draft Status: Success";
                draftStatusLabel.ForeColor = Color.Green;
            }
            else
            {
                // TODO: Call API to submit all answer, test name, studentID, paper number
                // CallAPIToSubmit()
                // Change UI Draft Status UI to submit success
                draftStatusLabel.Text = "Submit Status: Success";
                draftStatusLabel.ForeColor = Color.Green;
                MessageBox.Show(submition.StudentID + " have submitted something");
            }
        }

        /// <summary>
        ///     Handle when user close window -> application will be closed. 
        /// </summary>
        private void ClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
