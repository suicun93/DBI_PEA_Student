﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using DBI_PE_Submit_Tool.Entity;
using DBI_PE_Submit_Tool.Model;
using DBI_PE_Submit_Tool.Common;
using DBI_PE_Submit_Tool.UI;
using System.IO;
using System.IO.Compression;

namespace DBI_PE_Submit_Tool
{
    public partial class ClientForm : Form
    {
        // Information about student: studentID, paperNo, examCode, urlDB to get material, listAnswer;
        private List<RichTextBox> ListAnswer = new List<RichTextBox>();
        private List<Image> images = new List<Image>();
        private ShowImageForm showImageForm = null;
        private Submission Submission;
        private string UrlDBToDownload;
        private ExamInfo examInfo;

        // Merge draft and submit to 1 method with a variable named "forDraft"
        private bool forDraft = true, forSubmit = false;

        // Make student preview their answers before submitting
        private bool previewed = false;
        public bool Previewed
        {
            get => previewed;
            set
            {
                previewed = value;
                previewButton.Enabled = !previewed;
            }
        }

        // Data from server
        System.Windows.Forms.Timer timer = null;
        private ResponseData json;
        private int remainingTime;

        public ClientForm(string examCode, string paperNo, string StudentName, ResponseData _json, bool restored)
        {
            InitializeComponent();
            if (string.IsNullOrEmpty(examCode) || string.IsNullOrEmpty(paperNo) || string.IsNullOrEmpty(StudentName))
            {
                MessageBox.Show("Empty Information");
                Application.Exit();
            }
            else
            {
                json = _json;
                examInfo = new ExamInfo(examCode, paperNo);

                Thread t = new Thread(() =>
                {
                    string url = Constant.API_URL + "/questions";
                    this.images = Download.DownloadQuestions(url, json.Token, examInfo.ExamCode, examInfo.PaperNo);
                });
                t.Start();

                // Now I hard code a link to call api get material.
                UrlDBToDownload = Constant.API_URL + "/material";

                studentLabel.Text = StudentName;
                paperNoLabel.Text = paperNo;
                examCodeLabel.Text = examCode;
                Submission = new Submission(examCode, StudentName, paperNo, json.Token);
                Submission.Register();

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
            remainingTime -= 120;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (remainingTime > 0)
            {
                remainingTime--;
                if (timeLabel != null)
                {
                    if (timeLabel != null && timeLabel.Visible)
                    {
                        timeLabel.Text = TimeSpan.FromSeconds(remainingTime).ToString(@"hh\:mm\:ss");
                        timeLabel.Refresh();
                    }
                }
            }
            else
            {
                timer.Stop();
                DisableControls();
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
                Submission.Restore();
                for (int i = 0; i < json.QuestionNumber; i++)
                    ListAnswer[i].Text = Submission.ListAnswer[i];
            }

            // Add event to draft every time answers changed
            foreach (RichTextBox textBox in ListAnswer)
                textBox.TextChanged += new EventHandler(DraftAnswers);
            SetupTimer();
        }

        private void HelpButton_Click(object sender, EventArgs e) => MessageBox.Show("You should preview before submitting.");

        /// <summary>
        ///     Preview to re-check 10 answers
        /// </summary>
        private void PreviewButton_Click(object sender, EventArgs e)
        {
            // Sum up answer to a string
            int i = 0;
            string answers = "";
            foreach (string answer in Submission.ListAnswer)
            {
                i++;
                answers += "Question " + i + "\n\n" + (string.IsNullOrEmpty(answer) ? "(empty)" : answer)
                    + "\n\n================================================\n\n";
            }
            // Show preview form.
            PreviewForm previewForm = new PreviewForm(completion: () => { Previewed = true; previewForm = null; }, answers: answers);
            previewForm.Show();
        }


        private void DownloadMaterialButton_Click(object sender, EventArgs e) =>
            Download.PostDownloadMaterial(UrlDBToDownload, json.Token);

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            if (!Previewed)
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
            draftStatusLabel.Text = "Draft Status: N/A";
            draftStatusLabel.ForeColor = Color.Red;
            draftStatusLabel.Refresh();

            // Process
            Submission.ClearAnswer();
            foreach (RichTextBox richTextBox in ListAnswer)
                Submission.AddAnswer(richTextBox.Text);
            try
            {
                Submission.SaveToLocal();
                if (forDraft)
                {
                    // TODO: Call API to draft all answer, exam code, student's rollID, paper number
                    // CallAPIToDraft()
                    // Change UI Draft Status UI to draft success
                    Previewed = false;

                    draftStatusLabel.Text = "Draft Status: Success";
                    draftStatusLabel.ForeColor = Color.Green;
                    draftStatusLabel.Refresh();
                }
                else
                {
                    // Submit by calling API
                    Thread t = new Thread(HandleSubmit);
                    t.Start();
                }
            }
            catch (Exception)
            {
                // Change UI Draft Status UI failed
                draftStatusLabel.Text = forDraft ? "Draft" : "Submit" + " Status: N/A";
                draftStatusLabel.ForeColor = Color.Red;
                draftStatusLabel.Refresh();
            }
        }

        private void HandleSubmit()
        {
            // Call API to submit
            bool result = Submission.Submit((text) =>
            {
                Console.WriteLine(text);
            });

            if (result)
            {
                // Stop time when submit successfully
                timer?.Stop();

                // Change UI Draft Status UI to submit success
                draftStatusLabel.Text = "Submit Status: Success";
                draftStatusLabel.ForeColor = Color.Green;

                // Disable all controls.
                DisableControls();
            }
            else
            {
                // Change UI Draft Status UI to submit success
                draftStatusLabel.Text = "Submit Status: Failed";
                draftStatusLabel.ForeColor = Color.Red;
            }
            draftStatusLabel.Refresh();
        }

        private void FontSize_ValueChanged(object sender, EventArgs e)
        {
            foreach (RichTextBox box in ListAnswer)
                box.Font = new Font(box.Font.FontFamily, (int)fontSize.Value);
        }

        private void ExamContentButton_Click(object sender, EventArgs e)
        {
            showImageForm = new ShowImageForm(images, () => { });
            showImageForm.Show();
        }

        private void DisableControls()
        {
            // Disable all controls.
            foreach (Control item in Controls)
                if (item is Button)
                    item.Enabled = false;
            foreach (TabPage item in tabBar.TabPages)
                foreach (Control bth in item.Controls)
                    bth.Enabled = false;
            readyToFinishCheckBox.Enabled = false;
        }

        /// <summary>
        ///     Handle when user close window -> application will be closed. 
        /// </summary>
        private void ClientForm_FormClosing(object sender, FormClosingEventArgs e) => Application.Exit();
    }
}
