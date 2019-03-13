using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using DBI_PE_Submit_Tool.Entity;
using DBI_PE_Submit_Tool.Model;
using DBI_PE_Submit_Tool.Common;

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

        //public int QuestionNumber { get; set; } = 10;

        private ResponseData json;
        private Int32 remainingTime;

        public ClientForm(string examCode, string PaperNo, string StudentName, ResponseData _json, bool restored)
        {
            InitializeComponent();
            if (String.IsNullOrEmpty(examCode) || String.IsNullOrEmpty(PaperNo) || String.IsNullOrEmpty(StudentName))
            {
                MessageBox.Show("Empty Information");
                Application.Exit();
            }
            else
            {
                this.json = _json;
                SetupTimer();
                // TODO: Call API to get question here!

                // Now I mock up an url to download (an image from w3school)
                UrlDBToDownload = Constant.API_URL + "/material";

                studentLabel.Text = StudentName;
                paperNoLabel.Text = PaperNo;
                examCodeLabel.Text = examCode;
                submition = new Submition(examCode, StudentName, PaperNo, json.Token);
                submition.register();

                SetupTab();
                SetupUI(restored);
            }
        }

        private void SetupTimer()
        {
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Tick += Timer_Tick;
            timer.Interval = 1000;
            timer.Start();

            Int32 now = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            remainingTime = json.Exp - now;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (remainingTime > 0)
            {
                remainingTime--;
                timeLabel.Text = TimeSpan.FromSeconds(remainingTime).ToString(@"hh\:mm\:ss");
            } else
            {
                //MessageBox.Show("Time out!");
                timeLabel.Text = "00:00:00";
                // Disable all controls.
                foreach (Control c in this.Controls)
                {
                    c.Enabled = false;
                }
            }
        }

        private void SetupTab()
        {
            tabBar.TabPages.Clear();

            for (int i = 0; i < json.QuestionNumber; i++)
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
            for (int i = 0; i < json.QuestionNumber; i++)
            {
                RichTextBox box = (RichTextBox)tabBar.TabPages[i].Controls["textBox"];
                ListAnswer.Add(box);
            }

            // Check restore and restore answers
            if (restored)
            {
                // If student continue from break point, restore answers for them
                submition.Restore();
                for (int i = 0; i < json.QuestionNumber; i++)
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
            //Download.DownloadFrom(UrlDBToDownload, Token , locationMaterialTextBox);
            Download.PostDownloadMaterial(UrlDBToDownload, json.Token);
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
                draftStatusLabel.Invoke((MethodInvoker)delegate
               {
                   draftStatusLabel.Text = "Submit Status: Success";
                   draftStatusLabel.ForeColor = Color.Green;

               });
                MessageBox.Show(submition.StudentID + " have submitted something");
            }
        }

        private void fontSize_ValueChanged(object sender, EventArgs e)
        {
            foreach (RichTextBox box in ListAnswer)
            {
                box.Font = new Font(box.Font.FontFamily, (int)fontSize.Value);
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
