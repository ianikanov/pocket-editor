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
        int textlen;
        public cRichTextBox()
        {
            InitializeComponent();
            textlen = 0;
            TextProcessor = new CTextProcessor();
            TextProcessor.ControlInstanse = txtMain;
        }

        public CTextProcessor TextProcessor { get; set; }

        public string FileName { get; set; }
        public bool IsChanged { get; set; }
        public override string Text { get { return txtMain.Text; } set { txtMain.Text = value; } }
        public string SelectedText { get { return txtMain.SelectedText; } }
        public int Language { get; set; }


        private void txtMain_TextChanged(object sender, EventArgs e)
        {
            int curPosition;
            IsChanged = true;
            if (txtMain.Text.Length > textlen && txtMain.SelectionStart > 0)//added and not inserted
            {
                if (txtMain.Text[txtMain.SelectionStart - 1] == '\n')
                {
                    recalcLines();
                    try
                    {
                        int spacesCount = 0;
                        int linePos = GetLineNumber();
                        string[] lines = GetLines();
                        string newSpace = "";
                        if (linePos != 1) spacesCount++;
                        while ((lines[linePos - 1].Length > spacesCount) && (lines[linePos - 1][spacesCount++] == ' ')) newSpace += " ";
                        curPosition = txtMain.SelectionStart;
                        txtMain.Text = txtMain.Text.Insert(curPosition, newSpace);
                        txtMain.SelectionStart = curPosition + newSpace.Length;
                        txtMain.ScrollToCaret();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                if (txtMain.Text[txtMain.SelectionStart - 1] == '\t')
                { 
                    curPosition = txtMain.SelectionStart;
                    txtMain.Text = txtMain.Text.Replace("\t", "  ");
                    txtMain.SelectionStart = curPosition + 1;
                    txtMain.ScrollToCaret();
                }
            }
            textlen = txtMain.Text.Length;
        }

        #region tech
        private void recalcLines()
        {
            int max = GetLines().Length;
            string ss = "";
            for (int i = 0; i < max; i++) ss += i + "\r\n";
            lblLines.Text = ss;
        }

        /// <summary>
        /// Получить начальный номер строки
        /// </summary>
        /// <returns></returns>
        private int GetLineNumber()
        {
            return GetLineNumber(false);
        }

        /// <summary>
        /// Получить номер строки выделения
        /// </summary>
        /// <param name="toEnd">false - первая строка, true - последняя строка</param>
        /// <returns></returns>
        private int GetLineNumber(bool toEnd)
        {
            return txtMain.Text.Substring(0, toEnd ? (txtMain.SelectionLength + txtMain.SelectionStart) : txtMain.SelectionStart).Split((char)Keys.Enter).Length - 1;
        }

        /// <summary>
        /// Получить текст, разбитый на строки
        /// </summary>
        /// <returns></returns>
        private string[] GetLines()
        {
            string txt = txtMain.Text;
            return txt.Split((char)Keys.Enter);
        }

        /// <summary>
        /// Вставить строчку в начало всех выделенных строк
        /// </summary>
        /// <param name="newSpace">Строка для вставки</param>
        private void InsertToLineStart(string newSpace)
        {
            int lst = GetLineNumber();
            int lfin = GetLineNumber(true);
            string[] ls = GetLines();
            string str = "";
            int ret = 0;
            for (int i = 0; i < ls.Length; i++)
            {
                if (i >= lst && i <= lfin)
                {
                    str += ls[i].Insert((i == 0) ? 0 : 1, newSpace) + "\r";
                    ret += newSpace.Length;
                }
                else
                {
                    str += ls[i] + "\r";
                }
            }
            int curPosition = txtMain.SelectionStart;
            int len = txtMain.SelectionLength;
            txtMain.Text = str.Substring(0, str.Length - 1);
            txtMain.SelectionStart = curPosition;
            if (len > 0) txtMain.SelectionLength = len + ret;
            txtMain.ScrollToCaret();
        }

        /// <summary>
        /// Удалить строчку из начала всех выделенных строк
        /// </summary>
        /// <param name="newSpace">Строка для удаления</param>
        /// <param name="allowSubStr">Удалять ли так же любую подстроку данной</param>
        private void RemoveFromLineStart(string newSpace, bool allowSubStr)
        {
            int lst = GetLineNumber();
            int lfin = GetLineNumber(true);
            string[] ls = GetLines();
            string str = "";
            string line;
            int cnt;
            string pattern;
            int ret = 0;
            for (int i = 0; i < ls.Length; i++)
            {
                if (i >= lst && i <= lfin)
                {
                    cnt = newSpace.Length;
                    pattern = newSpace;
                    if (ls[i].StartsWith("\n"))
                    {
                        str += "\n";
                        line = ls[i].Substring(1);
                    }
                    else line = ls[i];
                    //ищем любые вхождения
                    while (!line.StartsWith(pattern))
                    {
                        if (!allowSubStr)//если для любых запрещено - то выходим
                        {
                            cnt = 0;
                            break;
                        }
                        if (--cnt == 0) break;
                        pattern = newSpace.Substring(0, cnt);
                    }
                    //если нашли, то удаляем
                    if (cnt > 0)
                    {
                        str += line.Substring(cnt) + "\r";
                        ret += cnt;
                    }
                    else str += line + "\r";
                }
                else
                {
                    str += ls[i] + "\r";
                }

            }
            int curPosition = txtMain.SelectionStart;
            int len = txtMain.SelectionLength;
            txtMain.Text = str.Substring(0, str.Length - 1);
            txtMain.SelectionStart = curPosition;
            if (len > 0) txtMain.SelectionLength = len - ret;
            txtMain.ScrollToCaret();
        }

        /// <summary>
        /// Удалить строчку из начала всех выделенных строк
        /// </summary>
        /// <param name="newSpace">Строка для удаления</param>
        private void RemoveFromLineStart(string newSpace)
        {
            RemoveFromLineStart(newSpace, false);
        }

        internal void FocusText()
        {
            txtMain.Focus();
        }

        internal void SelectText(int pos, int len)
        {
            txtMain.SelectionStart = pos;
            txtMain.SelectionLength = len;
            txtMain.ScrollToCaret();
        }
        #endregion

        #region ContextMenu
        private void miCut_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(txtMain.SelectedText);
            int cursor = txtMain.SelectionStart;
            txtMain.Text = txtMain.Text.Substring(0, cursor) + txtMain.Text.Substring(cursor + txtMain.SelectionLength, txtMain.TextLength - (cursor + txtMain.SelectionLength));
            txtMain.SelectionStart = cursor;
            txtMain.ScrollToCaret();
        }

        private void miCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(txtMain.SelectedText);
        }

        private void miPaste_Click(object sender, EventArgs e)
        {
            int curPosition = txtMain.SelectionStart;
            string toPaste;
            try
            {
                toPaste = Clipboard.GetDataObject().GetData(typeof(string)).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            txtMain.Text = txtMain.Text.Remove(txtMain.SelectionStart, txtMain.SelectionLength).Insert(curPosition, toPaste);
            txtMain.SelectionStart = curPosition + toPaste.Length;
            txtMain.ScrollToCaret();
        }
        #endregion

        #region Tab
        public void TabRight()
        {
            InsertToLineStart(Settings.TabSwap);
        }

        public void TabLeft()
        {
            RemoveFromLineStart(Settings.TabSwap, true);
        }
        #endregion

        #region comment
        public void Comment()
        {
            InsertToLineStart("//");
        }

        public void Uncomment()
        {
            RemoveFromLineStart("//");
        }
        #endregion
    }
}
