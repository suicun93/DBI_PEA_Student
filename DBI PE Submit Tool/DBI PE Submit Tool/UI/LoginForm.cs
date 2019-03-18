using System;
using System.Net;
using System.Windows.Forms;
using Newtonsoft.Json;

using DBI_PE_Submit_Tool.Common;
using DBI_PE_Submit_Tool.Entity;

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

        private ResponseData json;

        public LoginForm()
        {
            InitializeComponent();
            foreach (Control control in Controls)
                if (control is TextBox)
                    control.KeyPress += Control_KeyPress;
        }

        private void Control_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                BtnLogin_Click(null, null);
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (LoginSuccess)
                {
                    ClientForm clientForm = new ClientForm(Examcode.Trim(), PaperNo.Trim(), Username.Trim(), json, restoreCheckBox.Checked);
                    clientForm.Show();
                    Hide();
                }
                else
                    MessageBox.Show("Login Failed");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Button exit: Just exit now
        private void BtnExit_Click(object sender, EventArgs e) => Application.Exit();

        // Button Login: Login method(mock up)
        private bool LoginSuccess
        {
            get
            {
                Examcode = examCodeTextBox.Text;
                PaperNo = paperNoTextBox.Text;
                Username = usernameTextBox.Text;
                Password = passwordTextBox.Text;
                Domain = domainTextBox.Text;
                if (string.IsNullOrEmpty(Examcode) ||
                    string.IsNullOrEmpty(PaperNo) ||
                    string.IsNullOrEmpty(Username) ||
                    string.IsNullOrEmpty(Password) ||
                    string.IsNullOrEmpty(Domain))
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

                            var parameters = new System.Collections.Specialized.NameValueCollection
                            {
                                { "username", Username },
                                { "hashedPassword", Password },
                                { "examCode", Examcode },
                                { "paperNo", PaperNo }
                            };

                            byte[] responseBytes = client.UploadValues(uri, "POST", parameters);
                            string responseBody = System.Text.Encoding.UTF8.GetString(responseBytes);
                            json = JsonConvert.DeserializeObject<ResponseData>(responseBody);

                            return true;
                        }
                        catch (Exception e)
                        {
                            //MessageBox.Show("Error!\n" + e.Message);
                        }
                    }
                }
                return false;
            }
        }

        // When student quit.
        private void LoginForm_Closing(object sender, FormClosingEventArgs e) => Application.Exit();
    }
}
