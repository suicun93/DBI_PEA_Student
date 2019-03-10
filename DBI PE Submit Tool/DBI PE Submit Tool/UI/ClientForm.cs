using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DBI_PE_Submit_Tool.Model;
using DBI_PE_Submit_Tool.Common;
using System.Threading;

namespace DBI_PE_Submit_Tool
{
    public partial class ClientForm : Form
    {
        // Information about student: studentID, paperNo, examCode, urlDB to get material, listAnswer;
        private string UrlDB;
        private List<RichTextBox> ListAnswer = new List<RichTextBox>();
        private Submition submition;
        // Merge draft and submit to 1 method with a variable named "forDraft"
        private bool forDraft = true, forSubmit = false; 
        // Make student preview their answers before submitting
        private bool previewed = false; 
        public string UrlDBToDownload { get => UrlDB; set => UrlDB = value; }

        public int QuestionNumber { get; set; } = 10;

        public ClientForm(string examCode, string PaperNo, string StudentName, bool restored)
        {
            InitializeComponent();
            if (String.IsNullOrEmpty(examCode) || String.IsNullOrEmpty(PaperNo) || String.IsNullOrEmpty(StudentName))
            {
                MessageBox.Show("Empty Information");
                Application.Exit();
            }
            else
            {
                // TODO: Call API to get question here!
                // Now I mock up an url to download (an image from w3school)
                UrlDBToDownload = "https://www.w3schools.com/w3images/mac.jpg";
                studentLabel.Text = StudentName;
                paperNoLabel.Text = PaperNo;
                examCodeLabel.Text = examCode;
                submition = new Submition(examCode, StudentName, PaperNo);
                submition.register();

                SetupTab();
                SetupUI(restored);
            }
        }

        private void SetupTab()
        {
            tabBar.TabPages.Clear();

            for (int i = 0; i < QuestionNumber; i++)
            {
                string title = "" + (i + 1);
                TabPage tab = new TabPage(title);

                RichTextBox textBox = new RichTextBox();
                textBox.Name = "textBox";
                textBox.Dock = DockStyle.Fill;
                tab.Controls.Add(textBox);

                tabBar.TabPages.Add(tab);
            }
        }

        /// <summary>
        ///     Add all answer rich text box to list to add event draft answer on text change
        /// </summary>
        private void SetupUI(bool restored)
        {
        //    ListAnswer.Add(q1RichTextBox);
        //    ListAnswer.Add(q2RichTextBox);
        //    ListAnswer.Add(q3RichTextBox);
        //    ListAnswer.Add(q4RichTextBox);
        //    ListAnswer.Add(q5RichTextBox);
        //    ListAnswer.Add(q6RichTextBox);
        //    ListAnswer.Add(q7RichTextBox);
        //    ListAnswer.Add(q8RichTextBox);
        //    ListAnswer.Add(q9RichTextBox);
        //    ListAnswer.Add(q10RichTextBox);

            for (int i = 0; i < QuestionNumber; i++)
            {
                RichTextBox box = (RichTextBox)tabBar.TabPages[i].Controls["textBox"];
                ListAnswer.Add(box);
            }

            // Check restore and restore answers
            if (restored)
            {
                // If student continue from break point, restore answers for them
                submition.Restore();
                for (int i = 0; i < QuestionNumber; i++)
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
            submition.Restore();
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
            try
            {
                submition.SaveToLocal();
                if (forDraft)
                {
                    // TODO: Call API to draft all answer, exam code, student's rollID, paper number
                    // CallAPIToDraft()
                    // Change UI Draft Status UI to draft success
                    draftStatusLabel.Text = "Draft Status: Success";
                    draftStatusLabel.ForeColor = Color.Green;
                }
                else
                {
                    // TODO: Call API to submit all answer, exam code, studentID, paper number

                    // CallAPIToSubmit()
                    // Make some time-out here!
                    Thread t = new Thread(handleSubmit);
                    t.Start();

                    // Change UI Draft Status UI to submit success
                    //draftStatusLabel.Text = "Submit Status: Success";
                    //draftStatusLabel.ForeColor = Color.Green;
                    //MessageBox.Show(submition.StudentID + " have submitted something");
                }
            }
            catch (Exception)
            {
                // Change UI Draft Status UI 
                draftStatusLabel.Text = "Draft Status: N/A";
                draftStatusLabel.ForeColor = Color.Red;
            }
            
        }

        private void handleSubmit()
        {
            void doAfterSubmit(string text)
            {
                MessageBox.Show(text);
            }
            bool result = submition.submit(doAfterSubmit);
            if (result)
            {
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
