using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace JSEditor
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            LoadData();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SaveData();
            Close();
        }

        public void LoadData()
        {
            numTab.Value = Settings.TabSwap.Length;
        }

        public void SaveData()
        {
            try
            {
                Settings.TabSwap = new string(' ', (int)numTab.Value);
                Registry.SetValue("HKEY_CURRENT_USER\\Software\\JSEditor", "numTab", numTab.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
        }

        private void btnFreeRegistr_Click(object sender, EventArgs e)
        {
            try
            {
                Registry.CurrentUser.DeleteSubKey("Software\\JSEditor", true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
        }
    }
}