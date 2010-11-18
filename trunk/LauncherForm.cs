using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace JSEditor
{
    public partial class LauncherForm : Form
    {
        public LauncherForm()
        {
            InitializeComponent();
            Visible = false;
        }

        public string InitialString { get; set; }

        
        private void LauncherForm_Load(object sender, EventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            StartMainForm();
            Close();
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            Settings.Log(ex.ToString());
            ErrorForm ef = new ErrorForm();
            ef.Message = ex.Message;
            ef.Details = ex.ToString();
            ef.ShowDialog();
            ///
            /// problem in a framework: http://connect.microsoft.com/VisualStudio/feedback/details/94356/modal-windows-in-net-compact-framework-application-stop-working-if-unhandled-exception-on-modal-dialog-is-raised#
            ///
            if (ef.DialogResult == DialogResult.None) ef.ShowDialog();
            bool stop = false;
            switch (ef.DialogResult)
            {
                case DialogResult.Abort: stop = true; break;
                case DialogResult.Ignore: break;
                case DialogResult.Retry: StartMainForm();  break;
                default: stop = true; break;
            }
            if (stop) Close();
        }

        private void StartMainForm()
        {
            Form1 main = new Form1();
            if (!string.IsNullOrEmpty(InitialString))
            {
                main.OpenFile(InitialString);
                InitialString = null;
            }
            main.ShowDialog();
        }
    }
}