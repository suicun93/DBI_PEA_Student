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

namespace DBI_PE_Submit_Tool
{
    public partial class ClientForm : Form
    {
        // Information about student: studentID, paperNo, testName, urlDB to get material, listAnswer;
        private string studentID, paperNo, testName, urlDB;
        private List<RichTextBox> listAnswer = new List<RichTextBox>();

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
                urlDB = "https://www.w3schools.com/w3images/mac.jpg";
                studentLabel.Text = studentID = StudentName;
                paperNoLabel.Text = paperNo = PaperNo;
                testNameLabel.Text = testName = TestName;
                setUpUI();
            }
        }

        private void setUpUI()
        {
            // Add all answer rich text box to list to add event draft answer on text change
            listAnswer.Add(q1RichTextBox);
            listAnswer.Add(q2RichTextBox);
            listAnswer.Add(q3RichTextBox);
            listAnswer.Add(q4RichTextBox);
            listAnswer.Add(q5RichTextBox);
            listAnswer.Add(q6RichTextBox);
            listAnswer.Add(q7RichTextBox);
            listAnswer.Add(q8RichTextBox);
            listAnswer.Add(q9RichTextBox);
            listAnswer.Add(q10RichTextBox);
            // Add event
            for (int i = 0; i < listAnswer.Count; i++)
            {
                listAnswer[i].TextChanged += new EventHandler(draftAnswers);
            }
        }

        // Handle when user close window -> application will be closed.
        private void ClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        // Help Button CLick
        private void helpButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not supported yet");
        }

        // Download Material Button CLick
        private void downloadMaterialButton_Click(object sender, EventArgs e)
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
                    WebClient Client = new WebClient();
                    Client.DownloadFile(urlDB, locationChooser.FileName);
                    locationMaterialTextBox.Text = locationChooser.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Submit Button CLick
        private void submitButton_Click(object sender, EventArgs e)
        {
            if (readyToFinishCheckBox.Checked)
            {
                // Get all answer
                List<string> answers = new List<string>();
                foreach (RichTextBox richTextBox in listAnswer)
                {
                    answers.Add(richTextBox.Text);
                }
                // TODO: Call API to submit all answer, test name, studentID, paper number
                MessageBox.Show(studentID + " have submitted something");
            }
            else
            {
                MessageBox.Show("You have to confirmed to finish this exam.");
            }
        }

        // Draft Student's answer every time they edit their answer
        private void draftAnswers(object sender, System.EventArgs e)
        {
            // Get all answer
            List<string> answers = new List<string>();
            foreach (RichTextBox richTextBox in listAnswer)
            {
                answers.Add(richTextBox.Text);
            }
            // TODO: Call API to draft all answer, test name, student's rollID, paper number
        }
    }
}
