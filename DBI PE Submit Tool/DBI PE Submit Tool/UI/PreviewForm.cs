using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace DBI_PE_Submit_Tool.UI
{

    public partial class PreviewForm : Form
    {
        private Action completion;
        public PreviewForm(Action completion, string answers)
        {
            InitializeComponent();
            this.completion = completion;
            previewTextBox.Text = answers;
        }

        private void FinishPreviewButton_Click(object sender, EventArgs e)
        {
            completion();
            Dispose();
        }
    }
}
