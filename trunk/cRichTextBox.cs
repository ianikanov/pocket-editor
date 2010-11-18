using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace JSEditor
{
    public partial class cRichTextBox : UserControl
    {
        public CTextProcessor TextProcessor { get; set; }

        public cRichTextBox()
        {
            InitializeComponent();
            _tab = new TabPage();
            this.Dock = DockStyle.Fill;
            _tab.Controls.Add(this);
            TextProcessor = new CTextProcessor();
            TextProcessor.ControlInstanse = txtMain;
            TextProcessor.Language = new CLanguage();
        }

        #region properties
        private TabPage _tab;
        public TabPage Tab
        {
            get 
            {
                return _tab;
            }
        }
        public string TabName { get { return _tab.Text; } set { _tab.Text = value; } }

        public JSFile? FileDescriptor { get; set; }
        #endregion

        #region events
        private void txtMain_TextChanged(object sender, EventArgs e)
        {
            TextProcessor.TextChanged();
        }

        public void FocusText()
        {
            txtMain.Focus();
        }
        #endregion

        #region ContextMenu
        private void miCut_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(TextProcessor.Cut());
        }

        private void miCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(TextProcessor.Copy());
        }

        private void miPaste_Click(object sender, EventArgs e)
        {
            try
            {
                TextProcessor.Paste(Clipboard.GetDataObject().GetData(typeof(string)).ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            
        }
        #endregion

        #region public control methods
        public void TabRight()
        {
            TextProcessor.AddBeginning(Settings.TabSwap);
        }

        public void TabLeft()
        {
            TextProcessor.RemoveBeginning(Settings.TabSwap, true);
        }

        public void Comment()
        {
            TextProcessor.AddBeginning("//");
        }

        public void Uncomment()
        {
            TextProcessor.RemoveBeginning("//", false);
        }
        #endregion
    }
}
