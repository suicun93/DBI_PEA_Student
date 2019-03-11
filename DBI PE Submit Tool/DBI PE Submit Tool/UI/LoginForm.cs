using System;
using System.Net;
using System.Windows.Forms;
using Newtonsoft.Json;

using DBI_PE_Submit_Tool.Common;

namespace DBI_PE_Submit_Tool
{
    public partial class LoginForm : Form
    {
        private string Examcode { get; set; }
        private string PaperNo { get; set; }
        private string Username { get; set; }
        private string Password { get; set; }
        private string Domain { get; set; }

        private string apiUrl = Constant.API_URL;
        private string token;

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
                    ClientForm clientForm = new ClientForm(Examcode, PaperNo, Username, restored: restoreCheckBox.Checked, token);
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
                string loginUrl = apiUrl + "/login";
                Uri uri = new Uri(loginUrl);
                using (WebClient client = new WebClient())
                {
                    try
                    {
                        client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";

                        var parameters = new System.Collections.Specialized.NameValueCollection();
                        parameters.Add("username", Username);
                        parameters.Add("hashedPassword", Password);

                        byte[] responseBytes = client.UploadValues(uri, "POST", parameters);
                        string responseBody = System.Text.Encoding.UTF8.GetString(responseBytes);

                        var definition = new { Token = "" };
                        var json = JsonConvert.DeserializeAnonymousType(responseBody, definition);
                        token = json.Token;

                        //MessageBox.Show(json.Token);

                        return true;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Error!");
                    }
                }
            }
            return false;
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
