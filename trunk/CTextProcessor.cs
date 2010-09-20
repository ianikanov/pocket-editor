using System;

using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace JSEditor
{
    public class CTextProcessor
    {
        #region Properties
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
        public bool IsModified { get; set; }
        /// <summary>
        /// Language for current text
        /// </summary>
        public CLanguage Language { get; set; }
        #endregion
        #endregion

        #region private fields
        /// <summary>
        /// Stored length of Text
        /// </summary>
        int textlen = 0;
        #endregion

        #region public methods
        #region performing begin of line methods
        /// <summary>
        /// Insert specified pattern at the beginning of all selected lines
        /// </summary>
        /// <param name="newSpace">string to insert</param>
        public void AddBeginning(string newSpace)
        {
            string[] ls = Lines;
            string str = "";
            int ret = 0;
            for (int i = 0; i < LineCount; i++)
            {
#warning Not sure about \r
                if (i >= SelectedLineIndex && i < SelectedLineIndex + SelectedLinesCount)
                {
                    str += ls[i].Insert((i == 0) ? 0 : 1, newSpace) + "\r";
                    ret += newSpace.Length;
                }
                else
                {
                    str += ls[i] + "\r";
                }
            }
            int[] val = new int[] { SelectedLineIndex, SelectedLinesCount };
            Lines = ls;
            SelectedLineIndex = val[0];
            SelectedLinesCount = val[1];
            ControlInstanse.ScrollToCaret();
        }

        /// <summary>
        /// Remove specified pattern from begin of line
        /// </summary>
        /// <param name="newSpace">String to find</param>
        /// <param name="allowSubStr">Allows to remove also any substring of pattern</param>
        public void RemoveBeginning(string newSpace, bool allowSubStr)
        {
            string[] ls = Lines;
            string str = "";
            string line;
            int cnt;
            string pattern;
            int ret = 0;
            for (int i = 0; i < ls.Length; i++)
            {
                if (i >= SelectedLineIndex && i < SelectedLineIndex + SelectedLinesCount)
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
#warning Not sure about \r
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
            int[] val = new int[] { SelectedLineIndex, SelectedLinesCount };
            Lines = ls;
            SelectedLineIndex = val[0];
            SelectedLinesCount = val[1];
            ControlInstanse.ScrollToCaret();
        }
        #endregion

        #region Copy-Paste methods
        /// <summary>
        /// Return seleted text
        /// </summary>
        /// <returns></returns>
        public string Copy()
        {
            return SelectedText;
        }
        /// <summary>
        /// Remove selected text and return it
        /// </summary>
        /// <returns></returns>
        public string Cut()
        {
            string txt = SelectedText;
            int st = ControlInstanse.SelectionStart;
            Text = Text.Substring(0, st) + Text.Substring(st + txt.Length);
            ControlInstanse.SelectionStart = st;
            ControlInstanse.ScrollToCaret();
            return txt;
        }
        /// <summary>
        /// Remove current selection and insert pattern
        /// </summary>
        /// <param name="ptrn"></param>
        public void Paste(string ptrn)
        {
            int curPosition = ControlInstanse.SelectionStart;
            Text = Text.Remove(curPosition, ControlInstanse.SelectionLength).Insert(curPosition, ptrn);
            ControlInstanse.Select(curPosition, ptrn.Length);
            ControlInstanse.ScrollToCaret();
        }
        #endregion

        #region Text changed
        /// <summary>
        /// Make whole logic according to changes in text
        /// </summary>
        public void TextChanged()
        {
            int curPosition;
            IsModified = true;
            if (Text.Length > textlen && ControlInstanse.SelectionStart > 0)//added and not inserted
            {
                if (Text[ControlInstanse.SelectionStart - 1] == '\n')
                {
                    try
                    {
                        int spacesCount = 0;
                        string newSpace = "";
                        if (SelectedLineIndex != 1) spacesCount++;
                        while ((Lines[SelectedLineIndex - 1].Length > spacesCount) && (Lines[SelectedLineIndex - 1][spacesCount++] == ' ')) newSpace += " ";
                        AddBeginning(newSpace);
                        //curPosition = ControlInstanse.SelectionStart;
                        //Text = Text.Insert(curPosition, newSpace);
                        //txtMain.SelectionStart = curPosition + newSpace.Length;
                        //txtMain.ScrollToCaret();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                if (Text[ControlInstanse.SelectionStart - 1] == '\t')
                {
                    curPosition = ControlInstanse.SelectionStart;
                    Text = Text.Replace("\t", Settings.TabSwap);
                    ControlInstanse.SelectionStart = curPosition + 1;
                    ControlInstanse.ScrollToCaret();
                }
            }
            textlen = Text.Length;
        }
        #endregion
        #endregion

        #region Private methods
        /// <summary>
        /// Get word between separators, that contains specified index (or if null - Selection start)
        /// </summary>
        /// <param name="txtIndex"></param>
        /// <returns></returns>
        private Word GetWord(int? txtIndex)
        {
            Word wr = new Word();
            int i = txtIndex.HasValue ? txtIndex.Value : ControlInstanse.SelectionStart;
            while (!Language.Separators.Contains(Text[i].ToString()) && --i > 0) ;
            wr.startPosition = i++;
            while (!Language.Separators.Contains(Text[i].ToString()) && ++i > 0) ;
            wr.length = i - wr.startPosition;
            wr.text = Text.Substring(wr.startPosition, wr.length);
            return wr;
        }
        #endregion
    }

    public struct Word
    {
        public string text;
        public int startPosition;
        public int length;
    }
}
