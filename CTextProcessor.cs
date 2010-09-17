using System;

using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace JSEditor
{
    public class CTextProcessor
    {
        #region Control-specific properties
        /// <summary>
        /// Exact Text control
        /// </summary>
        public TextBox ControlInstanse { get; set; }
        /// <summary>
        /// Full text
        /// </summary>
        public string Text { get { return ControlInstanse.Text; } set { ControlInstanse.Text = value; } }
        /// <summary>
        /// Selected text
        /// </summary>
        public string SelectedText { get { return ControlInstanse.SelectedText; } set { ControlInstanse.SelectedText = value; } }
        /// <summary>
        /// Text, separated into lines
        /// </summary>
        public string[] Lines
        {
            get
            {
                return ControlInstanse.Text.Split((char)Keys.Enter);
            }
            set
            {
                ControlInstanse.Text = string.Join(Keys.Enter.ToString(), value);
            }
        }
        /// <summary>
        /// First line in selection index
        /// </summary>
        public int SelectedLineIndex
        {
            get { return ControlInstanse.Text.Substring(0, ControlInstanse.SelectionStart).Split((char)Keys.Enter).Length; }
            set
            {
                int len = 0;
#warning May be not exact because of 'Enter' char
                for (int i = 0; i < value; i++) len += Lines[i].Length;
                ControlInstanse.Select(len, 0);
            }
        }
        /// <summary>
        /// Count of selected lines
        /// </summary>
        public int SelectedLinesCount {
            get { return ControlInstanse.Text.Substring(0, ControlInstanse.SelectionStart + ControlInstanse.SelectionLength).Split((char)Keys.Enter).Length; }
            set
            {
                int len = 0;
#warning May be not exact because of 'Enter' char
                for (int i = SelectedLineIndex; i < value; i++) len += Lines[i].Length;
                ControlInstanse.Select(ControlInstanse.SelectionStart, len);
            }
        }
        /// <summary>
        /// Total lines count
        /// </summary>
        public int LineCount { get { return Lines.Length; } }
        #endregion

        #region Common properties
        /// <summary>
        /// Gets is text in control was modified since open or last save
        /// </summary>
        public bool IsModified { get; private set; }

        #endregion
    }

    public enum DevelopLanguages
    { 
        Php,
        ActionScript,
        CSharp
    }
}
