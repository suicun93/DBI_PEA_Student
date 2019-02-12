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
    public partial class LoginForm : Form
    {
        private string TestName { get; set; }
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
                    ClientForm clientForm = new ClientForm(TestName, PaperNo, Username);
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
                TestName = testNameTextBox.Text;
                PaperNo = paperNoTextBox.Text;
                Username = usernameTextBox.Text;
                Password = passwordTextBox.Text;
                Domain = domainTextBox.Text;
                if (String.IsNullOrEmpty(TestName) ||
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
    }

    //private string linkNorthwindDB = @"C:\Users\hoangduc\Desktop\NorthwindDB.sql";
    //private string sqlConnectionString = @"data source = localhost;
    //                                    initial catalog = Northwind; user = sa; password = 123;";
    //private SqlConnection conn = null;

    //public Form1()
    //{
    //    InitializeComponent();
    //    // Init DB
    //    //string scriptDB = File.ReadAllText(linkNorthwindDB);

    //    // Init connection
    //    conn = new SqlConnection(sqlConnectionString);
    //    conn.Open();
    //}

    //private void test()
    //{
    //    int marks = 0;

    //    // Get all Answer
    //    List<string> listQueries = new List<string>();
    //    listQueries.Add(txtQ1.Text);
    //    listQueries.Add(txtQ2.Text);
    //    listQueries.Add(txtQ3.Text);
    //    // Now we just check "select query"
    //    foreach (var query in listQueries)
    //    {
    //        try
    //        {
    //            SqlCommand sqlCommand = new SqlCommand("select COUNT(*) from (" + query + ") t", conn);
    //            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
    //            DataTable dataTable = new DataTable();
    //            var count = 0;
    //            sqlDataAdapter.Fill(dataTable);
    //            count = int.Parse(dataTable.Rows[0][0].ToString());
    //            if (count != 0)
    //            {
    //                marks++;
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            MessageBox.Show(e.ToString().Substring(0, 300));
    //        }

    //    }
    //    MessageBox.Show("You get " + marks + "/3");
    //}

    //private void test2()
    //{
    //    int marks = 0;
    //    // Get all solution
    //    List<string> solutions = new List<string>();
    //    solutions.Add("select * from Orders");
    //    solutions.Add("select * from Orders where OrderID = 1");
    //    solutions.Add("select * from Orders");

    //    // Get all Answer
    //    List<string> listQueries = new List<string>();
    //    listQueries.Add(txtQ1.Text);
    //    listQueries.Add(txtQ2.Text);
    //    listQueries.Add(txtQ3.Text);

    //    // Now we just check "select query"
    //    int i = 0;
    //    foreach (var query in listQueries)
    //    {
    //        try
    //        {
    //            // student
    //            SqlCommand sqlCommand = new SqlCommand(query, conn);
    //            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
    //            DataTable dataTableStudent = new DataTable();
    //            sqlDataAdapter.Fill(dataTableStudent);

    //            // solution
    //            SqlCommand sqlCommand2 = new SqlCommand(solutions[i], conn);
    //            i++;
    //            SqlDataAdapter sqlDataAdapter2 = new SqlDataAdapter(sqlCommand2);
    //            DataTable dataTable2 = new DataTable();
    //            sqlDataAdapter2.Fill(dataTable2);

    //            if (AreTablesTheSame(dataTable2, dataTableStudent))
    //            {
    //                marks++;
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            MessageBox.Show(e.ToString().Substring(0, 300));
    //        }

    //    }
    //    MessageBox.Show("You get " + marks + "/3");
    //}
    //private void run(object sender, EventArgs e)
    //{
    //    test2();
    //}

    //// Compare 2 datable to check performance
    //public static bool AreTablesTheSame(DataTable tbl1, DataTable tbl2)
    //{
    //    if (tbl1.Rows.Count != tbl2.Rows.Count || tbl1.Columns.Count != tbl2.Columns.Count)
    //        return false;


    //    for (int i = 0; i < tbl1.Rows.Count; i++)
    //    {
    //        for (int c = 0; c < tbl1.Columns.Count; c++)
    //        {
    //            if (!Equals(tbl1.Rows[i][c], tbl2.Rows[i][c]))
    //                return false;
    //        }
    //    }
    //    return true;
    //}

}
