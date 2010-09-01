using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace JSEditor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Settings.Load();
            Settings.Log("loaded");
        }

        #region Tabs

        private void addTab(string name)
        { 
            cRichTextBox txt = new cRichTextBox();
            if (tabControl1.TabPages.Count > 0) txt.Language = GetCurrentControl().Language;
            else txt.Language = 1;
            txt.Dock = DockStyle.Fill;
            TabPage tab = new TabPage();
            //tab.Name = name;
            tab.Text = name;
            tab.Controls.Add(txt);
            tabControl1.TabPages.Add(tab);
            tabControl1.SelectedIndex = tabControl1.TabPages.Count - 1;
        }

        private cRichTextBox GetCurrentControl()
        {
            try
            {
                return (tabControl1.TabPages[tabControl1.SelectedIndex].Controls[0] as cRichTextBox);
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region events

        private void inputPanel1_EnabledChanged(object sender, EventArgs e)
        {
            tabControl1.Height = this.Height - tabControl1.Top - 3;
            if (inputPanel1.Enabled) tabControl1.Height -= inputPanel1.Bounds.Height;
        }

        private void Form1_GotFocus(object sender, EventArgs e)
        {
            tabControl1.Height = this.Height - tabControl1.Top - 3;
            if (inputPanel1.Enabled) tabControl1.Height -= inputPanel1.Bounds.Height;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.TabPages.Count > 0)
            {
                int lng = GetCurrentControl().Language;
                foreach (MenuItem mi in menuLanguage.MenuItems) mi.Checked = false;
                switch (lng)
                {
                    case 1: miPhp.Checked = true; break;
                    case 2: miFlash.Checked = true; break;
                    case 3: miHtml.Checked = true; break;
                    case 4: miPhp.Checked = true; break;
                    case 5: miPhp.Checked = true; break;
                    case 6: miPhp.Checked = true; break;
                }
            }
        }

        private void Form1_Closing(object sender, CancelEventArgs e)
        {
            int prev = tabControl1.SelectedIndex;
            int cur = 0;
            foreach (TabPage pg in tabControl1.TabPages)
            {
                tabControl1.SelectedIndex = cur++;
                if (GetCurrentControl().IsChanged)
                {
                    DialogResult res = MessageBox.Show("File " + GetCurrentControl().FileName + " was changed. Save?", "Closing", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                    if (res == DialogResult.Cancel)
                    {
                        e.Cancel = true;
                        break;
                    }
                    if (res == DialogResult.Yes)
                    {
                        miSave_Click(null, null);
                    }
                }
            }
        }

        #endregion

        #region buttons
        private void btnComment_Click(object sender, EventArgs e)
        {
            GetCurrentControl().Comment();
            GetCurrentControl().FocusText();
        }

        private void btnUncomment_Click(object sender, EventArgs e)
        {
            GetCurrentControl().Uncomment();
            GetCurrentControl().FocusText();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            miSave_Click(null, null);
            GetCurrentControl().FocusText();
        }

        private void btnSaveAll_Click(object sender, EventArgs e)
        {
            miSaveAll_Click(null, null);
            GetCurrentControl().FocusText();
        }

        private void btnVerify_Click(object sender, EventArgs e)
        {

        }

        private void btnSnippet_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region menu

        #region File
        private void miOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FilterIndex = miPhp.Checked ? 0 :
                miFlash.Checked ? 2 :
                miHtml.Checked ? 3 :
                miCSharp.Checked ? 6 :
                miCPlus.Checked ? 7 :
                miSql.Checked ? 8 : -1;
            ofd.Filter = "PHP files|*.php|ActionScript files|*.as|HTM files|*.htm|HTML files|*.html|JavaScript files|*.js|C# files|*.cs|C++ files|*.cpp|SQL files|*.sql|All code files|*.php;*.as;*.htm;*.html;*.js;*.cs;*.cpp;*.sql|All files|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                FileInfo f = new FileInfo(ofd.FileName);
                if (!string.IsNullOrEmpty(GetCurrentControl().FileName) || GetCurrentControl().IsChanged) addTab(f.Name);
                GetCurrentControl().FileName = ofd.FileName;
                tabControl1.TabPages[tabControl1.SelectedIndex].Text = f.Name;
                foreach (MenuItem mi in menuLanguage.MenuItems) mi.Checked = false;
                switch (f.Extension)
                {
                    case ".php": miPhp.Checked = true; GetCurrentControl().Language = 1; break;
                    case ".as": miFlash.Checked = true; GetCurrentControl().Language = 2; break;
                    case ".html": miHtml.Checked = true; GetCurrentControl().Language = 3; break;
                    case ".htm": miHtml.Checked = true; GetCurrentControl().Language = 3; break;
                    case ".js": miHtml.Checked = true; GetCurrentControl().Language = 3; break;
                    case ".cs": miPhp.Checked = true; GetCurrentControl().Language = 4; break;
                    case ".cpp": miPhp.Checked = true; GetCurrentControl().Language = 5; break;
                    case ".sql": miPhp.Checked = true; GetCurrentControl().Language = 6; break;
                    default: Settings.Log(string.Format("While trying to open file {0} its extension {1} was not recognised", f.FullName, f.Extension)); break;
                }
                StreamReader sr = null;
                try
                {
                    sr = f.OpenText();
                    GetCurrentControl().Text = sr.ReadToEnd().Replace("\t", Settings.TabSwap);
                    sr.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    if (sr != null) sr.Close();
                }
            }
        }

        private void miSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(GetCurrentControl().FileName)) AskAndSave();
            else
            {
                SaveFile(GetCurrentControl().FileName);
                GetCurrentControl().IsChanged = false;
            }
        }

        private void miSaveAs_Click(object sender, EventArgs e)
        {
            AskAndSave();
        }

        private void miSaveAll_Click(object sender, EventArgs e)
        {
            int prev = tabControl1.SelectedIndex;
            int cur = 0;
            foreach (TabPage pg in tabControl1.TabPages)
            {
                tabControl1.SelectedIndex = cur++;
                miSave_Click(null, null);
            }
        }

        private void miNew_Click(object sender, EventArgs e)
        {
            addTab("Untitled");
        }

        private void miClose_Click(object sender, EventArgs e)
        {
            if (GetCurrentControl().IsChanged)
            {
                DialogResult res = MessageBox.Show("File was changed. Save?", "Closing", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (res == DialogResult.Cancel)
                {
                    return;
                }
                if (res == DialogResult.Yes)
                {
                    miSave_Click(null, null);
                }
            }
            tabControl1.TabPages.RemoveAt(tabControl1.SelectedIndex);
            if (tabControl1.TabPages.Count == 0) addTab("Untitled");
        }

        #endregion

        #region File functions
        private void SaveFile(string fname)
        {
            FileInfo f = new FileInfo(fname);
            GetCurrentControl().FileName = f.FullName;
            StreamWriter sw = null;
            try
            {
                sw = f.CreateText();
                sw.Write(GetCurrentControl().Text.Replace(Settings.TabSwap, "\t"));
                sw.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                if (sw != null) sw.Close();
            }
        }

        private void AskAndSave()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FilterIndex = miPhp.Checked ? 1 :
                miFlash.Checked ? 2 :
                miHtml.Checked ? 4 :
                miCSharp.Checked ? 6 :
                miCPlus.Checked ? 7 :
                miSql.Checked ? 8 : -1;
            sfd.Filter = "PHP files|*.php|ActionScript files|*.as|HTM files|*.htm|HTML files|*.html|JavaScript files|*.js|C# files|*.cs|C++ files|*.cpp|SQL files|*.sql|All code files|*.php;*.as;*.htm;*.html;*.js;*.cs;*.cpp;*.sql";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                SaveFile(sfd.FileName);
                GetCurrentControl().IsChanged = false;
                tabControl1.TabPages[tabControl1.SelectedIndex].Text = new FileInfo(sfd.FileName).Name;
            }
        }

        private bool CheckAndSave()
        {
            if (GetCurrentControl().IsChanged)
            {
                DialogResult res = MessageBox.Show("You have unsaved changes. Do you want to save it?", "Unsaved", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                if (res == DialogResult.Cancel) return false;
                if (res == DialogResult.Yes) AskAndSave();
            }
            return true;
        }
        #endregion

        #region other
        private void miAbout_Click(object sender, EventArgs e)
        {
            AboutForm fr = new AboutForm();
            fr.ShowDialog();
        }

        private void miSettings_Click(object sender, EventArgs e)
        {
            SettingsForm fr = new SettingsForm();
            fr.ShowDialog();
        }

        private void miLang_Click(object sender, EventArgs e)
        {
            int i = 0;
            foreach (MenuItem mi in menuLanguage.MenuItems)
            {
                i++;
                mi.Checked = false;
                if (mi.Equals(sender))
                {
                    GetCurrentControl().Language = i;
                    mi.Checked = true;
                }
            }
        }
        #endregion

        #region tab
        private void miTabRight_Click(object sender, EventArgs e)
        {
            GetCurrentControl().TabRight();
            GetCurrentControl().FocusText();
        }

        private void miTabLeft_Click(object sender, EventArgs e)
        {
            GetCurrentControl().TabLeft();
            GetCurrentControl().FocusText();
        }
        #endregion

        private void miSearch_Click(object sender, EventArgs e)
        {
            if (findForm == null)
            {
                startedTab = tabControl1.SelectedIndex;
                findForm = new FindForm();
                findForm.MainForm = this;
            }
            findForm.Show();
        }

        #endregion

        #region Search
        FindForm findForm;
        int startedTab;
        /// <summary>
        /// Make search aroung the files
        /// </summary>
        /// <param name="pattern">Search string</param>
        /// <param name="target">0 - Current document, 1 - All open documents, 2 - Selection</param>
        internal int DoSearch(string pattern, int target, int fromPosition)
        {
            cRichTextBox item = GetCurrentControl();
            int pos = -1;
            if (target == 2)
            {
                pos = item.SelectedText.IndexOf(pattern, fromPosition);
                if (pos > 0) SelectFromPosition(pos, pattern.Length);
            }
            if (target < 2)
            {
                pos = item.Text.IndexOf(pattern, fromPosition);
                if (pos > 0) SelectFromPosition(pos, pattern.Length);
                else if (target == 1 && startedTab != (tabControl1.SelectedIndex + 1) % tabControl1.TabPages.Count)
                {
                    tabControl1.SelectedIndex = (tabControl1.SelectedIndex + 1) % tabControl1.TabPages.Count;
                    pos = DoSearch(pattern, target, 0);
                }
            }

            return pos;
        }

        private void SelectFromPosition(int pos, int p)
        {
            GetCurrentControl().SelectText(pos, p);
            this.Show();
        }
        #endregion
    }
}