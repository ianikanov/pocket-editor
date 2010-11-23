using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace JSEditor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Settings.Load();
            tabControl1.TabPages.RemoveAt(0);//remove initial
            addTab("Untitled");//and add normally
        }

        private CFileManager _FileMng = new CFileManager();

        #region Tabs

        private void addTab(string name)
        { 
            cRichTextBox txt = new cRichTextBox();
            txt.TextProcessor.Language = LanguagePack.Instance.Languages[DevelopLanguages.Php];
            txt.TabName = name;
            tabControl1.TabPages.Add(txt.Tab);
            tabControl1.SelectedIndex = tabControl1.TabPages.Count - 1;
        }

        private cRichTextBox CurrentControl
        {
            get
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
            if (tabControl1.TabPages.Count > 0 && CurrentControl.TextProcessor.Language != null)
            {
                foreach (MenuItem mi in menuLanguage.MenuItems) mi.Checked = false;
                switch (CurrentControl.TextProcessor.Language.Language)
                {
                    case DevelopLanguages.Php: miPhp.Checked = true; break;
                    case DevelopLanguages.ActionScript: miFlash.Checked = true; break;
                    case DevelopLanguages.Html: miHtml.Checked = true; break;
                    case DevelopLanguages.Javascript: miHtml.Checked = true; break;
                    case DevelopLanguages.CSharp: miCSharp.Checked = true; break;
                    case DevelopLanguages.Cpp: miCPlus.Checked = true; break;
                    case DevelopLanguages.Sql: miSql.Checked = true; break;
                }
            }
        }

        private void Form1_Closing(object sender, CancelEventArgs e)
        {
            int cnt = tabControl1.TabPages.Count;
            for (int cur = 0; cur < cnt; cur++)
            {
                tabControl1.SelectedIndex = cur;
                if (!CheckAndSave())
                {
                    e.Cancel = true;
                    break;
                }
            }
        }

        #endregion

        #region buttons
        private void btnComment_Click(object sender, EventArgs e)
        {
            CurrentControl.Comment();
            CurrentControl.FocusText();
        }

        private void btnUncomment_Click(object sender, EventArgs e)
        {
            CurrentControl.Uncomment();
            CurrentControl.FocusText();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            miSave_Click(null, null);
            CurrentControl.FocusText();
        }

        private void btnSaveAll_Click(object sender, EventArgs e)
        {
            miSaveAll_Click(null, null);
            CurrentControl.FocusText();
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
            OpenFile();
        }

        private void miSave_Click(object sender, EventArgs e)
        {
            if (CurrentControl.FileDescriptor == null) AskAndSave();
            else
            {
                try
                {
                    _FileMng.SaveFile(CurrentControl.FileDescriptor.Value, CurrentControl.TextProcessor.Text);
                    CurrentControl.TextProcessor.IsModified = false;
                }
                catch (Exception ex)
                {
                    string mes = string.Format("Error occured while saving file: {0}", ex);
                    Debug.WriteLine(mes);
                    Settings.Log(mes);
                    MessageBox.Show(mes);
                }
            }
        }

        private void miSaveAs_Click(object sender, EventArgs e)
        {
            JSFile? fd = CurrentControl.FileDescriptor;
            if (AskAndSave() && fd.HasValue) _FileMng.CloseFile(fd.Value);
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
            if (CheckAndSave())
            {
                if (CurrentControl.FileDescriptor.HasValue) _FileMng.CloseFile(CurrentControl.FileDescriptor.Value);
                tabControl1.TabPages.RemoveAt(tabControl1.SelectedIndex);
                if (tabControl1.TabPages.Count == 0) addTab("Untitled");
            }
        }

        #region File functions
        /// <summary>
        /// Open Save file dialog get path and store file
        /// </summary>
        /// <returns>True if file is stored, false otherwise</returns>
        private bool AskAndSave()
        {
            string path = _FileMng.ShowSaveFileDialog(CurrentControl.TextProcessor.Language.Language);
            if (path != null)
            {
                try
                {
                    CurrentControl.FileDescriptor = _FileMng.SaveFile(path, CurrentControl.TextProcessor.Text);
                    CurrentControl.TextProcessor.IsModified = false;
                    CurrentControl.TabName = _FileMng.GetName(CurrentControl.FileDescriptor.Value);
                }
                catch (Exception ex)
                { 
                    string mes = string.Format("Error occured while saving file {0}: {1}", path, ex);
                    Debug.WriteLine(mes);
                    Settings.Log(mes);
                    MessageBox.Show(mes);
                    return false;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check was file changed or not and store it
        /// </summary>
        /// <returns>False if cancelled by user, true otherwise</returns>
        private bool CheckAndSave()
        {
            if (CurrentControl.TextProcessor.IsModified)
            {
                DialogResult res = MessageBox.Show("You have unsaved changes. Do you want to save it?", "Unsaved", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                if (res == DialogResult.Cancel) return false;
                if (res == DialogResult.Yes) miSave_Click(null, null);
            }
            return true;
        }

        /// <summary>
        /// Call Open dialog, create instance of Text Control, add tab and set text
        /// </summary>
        private void OpenFile()
        {
            string path = _FileMng.ShowOpenFileDialog(CurrentControl.TextProcessor.Language.Language);
            if (path != null)
            {
                OpenFile(path);
            }
        }

        /// <summary>
        /// Open specified file
        /// </summary>
        /// <param name="path">Phisical path to file</param>
        internal void OpenFile(string path)
        {
            try
            {
                if (CurrentControl.FileDescriptor != null || CurrentControl.TextProcessor.IsModified) addTab("new");
                string text;
                JSFile fd = _FileMng.OpenFile(path, out text);
                CurrentControl.TextProcessor.Text = text;
                //language set
                foreach (MenuItem mi in menuLanguage.MenuItems) mi.Checked = false;
                switch (_FileMng.GetExtension(fd))
                {
                    case ".php": miPhp.Checked = true; CurrentControl.TextProcessor.Language = LanguagePack.Instance.Languages[DevelopLanguages.Php]; break;
                    case ".as": miFlash.Checked = true; CurrentControl.TextProcessor.Language = LanguagePack.Instance.Languages[DevelopLanguages.ActionScript]; break;
                    case ".html": miHtml.Checked = true; CurrentControl.TextProcessor.Language = LanguagePack.Instance.Languages[DevelopLanguages.Html]; break;
                    case ".htm": miHtml.Checked = true; CurrentControl.TextProcessor.Language = LanguagePack.Instance.Languages[DevelopLanguages.Html]; break;
                    case ".js": miHtml.Checked = true; CurrentControl.TextProcessor.Language = LanguagePack.Instance.Languages[DevelopLanguages.Javascript]; break;
                    case ".cs": miPhp.Checked = true; CurrentControl.TextProcessor.Language = LanguagePack.Instance.Languages[DevelopLanguages.CSharp]; break;
                    case ".cpp": miPhp.Checked = true; CurrentControl.TextProcessor.Language = LanguagePack.Instance.Languages[DevelopLanguages.Cpp]; break;
                    case ".sql": miPhp.Checked = true; CurrentControl.TextProcessor.Language = LanguagePack.Instance.Languages[DevelopLanguages.Sql]; break;
                    default: Settings.Log(string.Format("While trying to open file {0} its extension {1} was not recognised", _FileMng.GetFullName(fd), _FileMng.GetExtension(fd))); break;
                }
                CurrentControl.TextProcessor.IsModified = false;
                CurrentControl.TabName = _FileMng.GetName(fd);
                CurrentControl.FileDescriptor = fd;
            }
            catch (Exception ex)
            {
                string mes = string.Format("While opening file the error occured: {0}", ex);
                Debug.WriteLine(mes);
                Settings.Log(mes);
                MessageBox.Show(mes);
            }
        }
        #endregion
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
#warning set language
                    //GetCurrentControl().Language = i;
                    mi.Checked = true;
                }
            }
        }
        #endregion

        #region tabulate
        private void miTabRight_Click(object sender, EventArgs e)
        {
            CurrentControl.TabRight();
            CurrentControl.FocusText();
        }

        private void miTabLeft_Click(object sender, EventArgs e)
        {
            CurrentControl.TabLeft();
            CurrentControl.FocusText();
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
            int pos = -1;
            if (target == 2)
            {
                pos = CurrentControl.TextProcessor.SelectedText.IndexOf(pattern, fromPosition);
                if (pos > 0) SelectFromPosition(pos, pattern.Length);
            }
            if (target < 2)
            {
                pos = CurrentControl.TextProcessor.Text.IndexOf(pattern, fromPosition);
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
            CurrentControl.TextProcessor.ControlInstanse.Select(pos, p);
            this.Show();
        }
        #endregion
    }
}