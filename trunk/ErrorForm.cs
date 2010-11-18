using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace JSEditor
{
    public partial class ErrorForm : Form
    {
        public ErrorForm()
        {
            InitializeComponent();
        }

        public string Message
        {
            set { lblMessage.Text = value; }
        }

        public string Details
        {
            set { txtDetails.Text = value; }
        }
        // abort
        private void btnNo_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Abort;
            Close();
        }
        // continue
        private void btnYes_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Ignore;
            Close();
        }
        // restart
        private void btnRestart_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Retry;
            Close();
        }
    }
}