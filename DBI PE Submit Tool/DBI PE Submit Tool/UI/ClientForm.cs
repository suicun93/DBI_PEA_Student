using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using DBI_PE_Submit_Tool.Entity;
using DBI_PE_Submit_Tool.Model;
using DBI_PE_Submit_Tool.Common;
using DBI_PE_Submit_Tool.UI;

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

        System.Windows.Forms.Timer timer = null;
        private ResponseData json;
        private int remainingTime;

        public ClientForm(string examCode, string PaperNo, string StudentName, ResponseData _json, bool restored)
        {
            InitializeComponent();
            if (string.IsNullOrEmpty(examCode) || string.IsNullOrEmpty(PaperNo) || string.IsNullOrEmpty(StudentName))
            {
                MessageBox.Show("Empty Information");
                Application.Exit();
            }
            else
            {
                json = _json;
                SetupTimer();

                // Now I hard code a link to call api get material.
                UrlDBToDownload = Constant.API_URL + "/material";

                studentLabel.Text = StudentName;
                paperNoLabel.Text = PaperNo;
                examCodeLabel.Text = examCode;
                submition = new Submition(examCode, StudentName, PaperNo, json.Token);
                submition.Register();

                SetupTab();
                SetupUI(restored);
            }
        }

        private void SetupTimer()
        {
            timer = new System.Windows.Forms.Timer();
            timer.Tick += Timer_Tick;
            timer.Interval = 1000;
            timer.Start();

            int now = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            remainingTime = json.Exp - now;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (remainingTime > 0)
            {
                remainingTime--;
                timeLabel.Invoke((MethodInvoker)(() =>
                {
                    timeLabel.Text = TimeSpan.FromSeconds(remainingTime).ToString(@"hh\:mm\:ss");
                }));
            }
            else
            {
                // Make student to submit without preview
                SumUpAnswer(forSubmit);
            }
        }

        private void SetupTab()
        {
            tabBar.TabPages.Clear();

            for (int i = 0; i < json.QuestionNumber; i++)
            {
                string title = "" + (i + 1);
                TabPage tab = new TabPage(title);

                tab.Controls.Add(new RichTextBox
                {
                    Name = "textBox",
                    Dock = DockStyle.Fill
                });
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
                    ListAnswer[i].Text = submition.ListAnswer[i];
            }

            // Add event to draft every time answers changed
            foreach (RichTextBox textBox in ListAnswer)
                textBox.TextChanged += new EventHandler(DraftAnswers);
        }

        private void HelpButton_Click(object sender, EventArgs e) => MessageBox.Show("You should preview before submitting.");

        /// <summary>
        ///     Preview to re-check 10 answers
        /// </summary>
        private void PreviewButton_Click(object sender, EventArgs e)
        {
            // List answers to preview
            submition.Restore();
            if (submition == null)
                MessageBox.Show("Restore failed");
            else
            {
                // SUm up answer to a string
                int i = 0;
                string answers = "";
                foreach (string answer in submition.ListAnswer)
                {
                    i++;
                    answers += "Question " + i + "\n\n" + (string.IsNullOrEmpty(answer) ? "(empty)" : answer)
                        + "\n\n================================================\n\n";
                }
                // Show preview form.
                new PreviewForm(completion: () => { previewed = true; }, answers: answers).Show();
            }
        }


        private void DownloadMaterialButton_Click(object sender, EventArgs e) =>
            Download.PostDownloadMaterial(UrlDBToDownload, json.Token);

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
        private void DraftAnswers(object sender, EventArgs e) =>
            // Get all answer
            SumUpAnswer(forDraft);

        /// <summary>
        ///     Sum Up Answers and save to local
        /// </summary>
        /// <param name="forDraft"> if draft -> call API to draft 
        ///                         if submit -> call API to submit 
        /// </param>
        private void SumUpAnswer(bool forDraft)
        {
            // Change UI Draft Status UI 
            draftStatusLabel.Invoke((MethodInvoker)(() =>
            {
                draftStatusLabel.Text = "Draft Status: N/A";
                draftStatusLabel.ForeColor = Color.Red;
            }));

            // Process
            submition.ClearAnswer();
            foreach (RichTextBox richTextBox in ListAnswer)
                submition.AddAnswer(richTextBox.Text);
            try
            {
                submition.SaveToLocal();
                if (forDraft)
                {
                    // TODO: Call API to draft all answer, exam code, student's rollID, paper number
                    // CallAPIToDraft()
                    // Change UI Draft Status UI to draft success
                    previewed = false;
                    draftStatusLabel.Invoke((MethodInvoker)(() =>
                    {
                        draftStatusLabel.Text = "Draft Status: Success";
                        draftStatusLabel.ForeColor = Color.Green;
                    }));
                }
                else
                {
                    // Call API
                    Thread t = new Thread(HandleSubmit);
                    t.Start();
                }
            }
            catch (Exception)
            {
                // Change UI Draft Status UI 
                draftStatusLabel.Invoke((MethodInvoker)(() =>
                {
                    draftStatusLabel.Text = "Draft Status: N/A";
                    draftStatusLabel.ForeColor = Color.Red;
                }));
            }

        }

        private void HandleSubmit()
        {

            bool result = submition.Submit((text) =>
            {
                Console.WriteLine(text);
            });
            if (result)
            {
                // Stop time when submit successfully
                timer?.Stop();
                // Change UI Draft Status UI to submit success
                draftStatusLabel.Invoke((MethodInvoker)delegate
               {
                   draftStatusLabel.Text = "Submit Status: Success";
                   draftStatusLabel.ForeColor = Color.Green;
               });
                // Disable all controls.
                foreach (Control c in Controls)
                    if (c is Button || c is RichTextBox)
                        c.Enabled = false;
            }
        }

        private void FontSize_ValueChanged(object sender, EventArgs e)
        {
            foreach (RichTextBox box in ListAnswer)
                box.Font = new Font(box.Font.FontFamily, (int)fontSize.Value);
        }

        /// <summary>
        ///     Handle when user close window -> application will be closed. 
        /// </summary>
        private void ClientForm_FormClosing(object sender, FormClosingEventArgs e) => Application.Exit();
    }
}
