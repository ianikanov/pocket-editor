using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace JSEditor
{
    public partial class FindForm : Form
    {
        public FindForm()
        {
            InitializeComponent();
            cmbWhere.SelectedIndex = 0;
        }

        public Form1 MainForm { get; set; }

        public string SearchField { get; set; }
        public int SearchPosition { get; set; }

        private void btnFind_Click(object sender, EventArgs e)
        {
            SearchField = cmbSearch.Text;
            if (cmbSearch.Items.Contains(SearchField)) cmbSearch.Items.Remove(SearchField);
            cmbSearch.Items.Insert(0, SearchField);
            SearchPosition = 0;
            DoSearch();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            DoSearch();
        }

        /// <summary>
        /// Make searching in files
        /// </summary>
        private void DoSearch()
        {
            SearchPosition = MainForm.DoSearch(SearchField, cmbWhere.SelectedIndex, SearchPosition);
            if (SearchPosition == -1) MessageBox.Show("Больше ничего не найдено");
            else SearchPosition += SearchField.Length;
        }
    }
}