using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBI_PE_Submit_Tool
{
    public partial class ClientForm : Form
    {
        public ClientForm(string TestName, string PaperNo, string StudentName)
        {
            InitializeComponent();
            if (String.IsNullOrEmpty(TestName) || String.IsNullOrEmpty(PaperNo) || String.IsNullOrEmpty(StudentName))
            {
                MessageBox.Show("Empty Information");
            } else
            {
                // TODO: Call API to get question here!
                studentLabel.Text = StudentName;
                paperNoLabel.Text = PaperNo;
                testNameLabel.Text = TestName;
            }
        }

        private void helpButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not supported yet");
        }

        private void downloadMaterialButton_Click(object sender, EventArgs e)
        {
            // TODO: Call API to get query to generate database.
            MessageBox.Show("Not supported yet");
        }

        private void ClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
