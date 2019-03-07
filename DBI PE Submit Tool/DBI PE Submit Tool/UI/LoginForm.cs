using System;
using System.Windows.Forms;

namespace DBI_PE_Submit_Tool
{
    public partial class LoginForm : Form
    {
        private string Examcode { get; set; }
        private string PaperNo { get; set; }
        private string Username { get; set; }
        private string Password { get; set; }
        private string Domain { get; set; }

        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (loginSuccess())
                {
                    ClientForm clientForm = new ClientForm(Examcode, PaperNo, Username, restored: restoreCheckBox.Checked);
                    clientForm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Login Failed");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Button exit: Just exit now
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Button Login: Login method(mock up)
        private bool loginSuccess()
        {
            try
            {
                Examcode = examCodeTextBox.Text;
                PaperNo = paperNoTextBox.Text;
                Username = usernameTextBox.Text;
                Password = passwordTextBox.Text;
                Domain = domainTextBox.Text;
                if (String.IsNullOrEmpty(Examcode) ||
                    String.IsNullOrEmpty(PaperNo) ||
                    String.IsNullOrEmpty(Username) ||
                    String.IsNullOrEmpty(Password) ||
                    String.IsNullOrEmpty(Domain))
                {
                    throw new Exception("Not enough information");
                }
                else
                {
                    // TODO: Call API to authorize but now It's hard to be true
                    return true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // When student quit.
        private void LoginForm_Closing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void usernameTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
