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
        public string Text { 
            get { return ControlInstanse.Text; } 
            set { ControlInstanse.Text = value; } 
        }
        /// <summary>
        /// Selected text
        /// </summary>
        public string SelectedText { get { return ControlInstanse.SelectedText; } set { ControlInstanse.SelectedText = value; } }
        #endregion

        #region Text properties
        /// <summary>
        /// Text, separated into lines
        /// </summary>
        [Obsolete("Deprecated")]
        public string[] Lines
        {
            get
            {
                List<string> ret = new List<string>();
                int subIndex;
                string text = ControlInstanse.Text;
                while ((subIndex = text.IndexOf("\r\n")) > -1)
                {
                    ret.Add(text.Substring(0, subIndex));
                    text = text.Substring(subIndex + 2);
                }
                ret.Add(text);
                return ret.ToArray();
            }
            set
            {
                ControlInstanse.Text = string.Join("\r\n", value);
            }
        }
        /// <summary>
        /// First line in selection index
        /// </summary>
        [Obsolete("Deprecated")]
        public int SelectedLineIndex
        {
            get 
            {
                List<string> ret = new List<string>();
                int subIndex;
                string text = ControlInstanse.Text.Substring(0, ControlInstanse.SelectionStart);
                while ((subIndex = text.IndexOf("\r\n")) > -1)
                {
                    ret.Add(text.Substring(0, subIndex));
                    text = text.Substring(subIndex + 2);
                }
                ret.Add(text);
                return ret.Count - 1;
            }
            set
            {
                int len = 0;
                for (int i = 0; i < value; i++) len += Lines[i].Length + 2;
                ControlInstanse.Select(len, 0);
            }
        }
        /// <summary>
        /// Count of selected lines
        /// </summary>
        [Obsolete("Deprecated")]
        public int SelectedLinesCount {
            get 
            {
                List<string> ret = new List<string>();
                int subIndex;
                string text = SelectedText;
                while ((subIndex = text.IndexOf("\r\n")) > -1)
                {
                    ret.Add(text.Substring(0, subIndex));
                    text = text.Substring(subIndex + 2);
                }
                ret.Add(text);
                return ret.Count;
            }
            set
            {
                int len = 0;
                for (int i = SelectedLineIndex; i < SelectedLineIndex + value; i++) len += Lines[i].Length + 2;
                ControlInstanse.Select(ControlInstanse.SelectionStart, len - 2);
            }
        }
        /// <summary>
        /// Total lines count
        /// </summary>
        public int LineCount { get { return _lines.Length; } }
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

        /// <summary>
        /// Internal lines array
        /// </summary>
        string[] _lines;
        #endregion

        #region public methods
        /// <summary>
        /// Make current word selected
        /// </summary>
        public void SelectWord()
        {
            Word wd = GetWord(null);
            ControlInstanse.Select(wd.startPosition, wd.length);
        }

        #region performing begin of line methods
        /// <summary>
        /// Insert specified pattern at the beginning of all selected lines
        /// </summary>
        /// <param name="newSpace">string to insert</param>
        public void AddBeginning(string newSpace)
        {
            string[] ls = Lines;
            for (int i = 0; i < SelectedLinesCount; i++)
            {
                ls[SelectedLineIndex + i] = ls[SelectedLineIndex + i].Insert(0, newSpace);
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
            string line;
            int cnt;
            string pattern;
            for (int i = 0; i < SelectedLinesCount; i++)
            {
                cnt = newSpace.Length;
                pattern = newSpace;
                line = ls[SelectedLineIndex + i];
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
                    ls[SelectedLineIndex + i] = line.Substring(cnt);
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
                        while ((Lines[SelectedLineIndex - 1].Length > spacesCount) && (Lines[SelectedLineIndex - 1][spacesCount++] == ' ')) newSpace += " ";
                        curPosition = ControlInstanse.SelectionStart;
                        AddBeginning(newSpace);
                        ControlInstanse.Select(curPosition + newSpace.Length, 0);
                        ControlInstanse.ScrollToCaret();
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

    #region Text classes
    public struct Word
    {
        public string text;
        public int startPosition;
        public int length;
    }

    /// <summary>
    /// Represent one text line for JSEditor
    /// </summary>
    public class CJSLine
    {
        #region Constructor
        /// <summary>
        /// Creates line structure for specified line of text
        /// </summary>
        /// <param name="initial">Line's text</param>
        /// <param name="num">Ordering number in parent text</param>
        public CJSLine(string initial, int num)
        {
            Text = initial;
            if (initial.IndexOf("\r\n") > -1) throw new ArgumentException("CJSLine(string): line contains return caret symbol");
            Length = initial.Length;
            LineNumber = num;
            Offset = 0;
            EndOffset = 0;
        }
        #endregion

        public string Text { get; set; }

        #region ReadOnly properties
        /// <summary>
        /// Gets length of current textline
        /// </summary>
        public int Length { get; private set; }
        /// <summary>
        /// Gets offset of this line in parent text
        /// </summary>
        public int Offset { get; private set; }
        /// <summary>
        /// Gets offset from line-end of this line in parent text
        /// </summary>
        public int EndOffset { get; private set; }
        /// <summary>
        /// Gets ordering number of this line in parent text
        /// </summary>
        public int LineNumber { get; private set; }
        #endregion

        public CJSLine LeftNeibour { get; set; }
        public CJSLine RightNeibour { get; set; }

        #region Indexer and implicit cast
        /// <summary>
        /// Gets specified char
        /// </summary>
        /// <param name="ind">char index in zero-based line</param>
        public char this[int ind]
        {
            get { return Text[ind]; }
        }

        /// <summary>
        /// Get line as a string
        /// </summary>
        public static implicit operator string(CJSLine ln)
        {
            return ln.Text;
        }
        #endregion
    }

    /// <summary>
    /// Represent text (set of lines) for JSEditor
    /// </summary>
    public class CJSText
    {
        #region Constructor
        /// <summary>
        /// Parse Text class from string, represented whole text
        /// </summary>
        /// <param name="initialText">Initial text for parsing</param>
        public CJSText(string initialText)
        { 
            int ind;
            int lines = 0;
            Lines = new List<CJSLine>();
            CJSLine prev = null, cur;
            //for each line
            while ((ind = initialText.IndexOf("\r\n")) > -1)
            {
                //create item
                cur = new CJSLine(initialText.Substring(0, ind), lines++);
                //fill left
                cur.LeftNeibour = prev;
                // for previous fill right
                if (prev != null) prev.RightNeibour = cur;
                //and prepare next step
                prev = cur;
                initialText = initialText.Substring(ind + 2);
            }
            //rest
            cur = new CJSLine(initialText, lines++);
            cur.LeftNeibour = prev;
            if (prev != null) prev.RightNeibour = cur;
            LineCount = lines;
        }
        #endregion

        #region ReadOnly properties
        /// <summary>
        /// Gets lines array
        /// </summary>
        public List<CJSLine> Lines { get; private set; }
        /// <summary>
        /// Gets count of lines
        /// </summary>
        public int LineCount { get; private set; }
        #endregion

        #region indexer and implicit cast
        /// <summary>
        /// Gets or sets special line
        /// </summary>
        /// <param name="ind">line zero-based index</param>
        /// <returns>indexed line</returns>
        public CJSLine this[int ind]
        {
            get { return Lines[ind]; }
            private set { Lines[ind] = value; }
        }

        /// <summary>
        /// Prepare all internal text as one string
        /// </summary>
        public static implicit operator string(CJSText txt)
        {
            string ret = "";
            foreach (string s in txt.Lines) ret = string.Join("\r\n", new string[] { ret, s });
            return ret;
        }
        #endregion
    }
    #endregion
}
