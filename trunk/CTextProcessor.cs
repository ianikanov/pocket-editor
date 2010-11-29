using System;

using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace JSEditor
{
    public class CTextProcessor
    {
        #region Properties
        /// <summary>
        /// Text control
        /// </summary>
        public cRichTextBox ControlInstanse
        {
            get { return _ctrlInst; }
            set {
                _ctrlInst = value;
                _text = new CJSText(_ctrlInst.Text);
            }
        }

        #region Text properties
        /// <summary>
        /// First line in selection index
        /// </summary>
        public int SelectedLineIndex
        {
            get
            {
                return _text.GetLineIndexByTextIndex(ControlInstanse.SelectedIndex);
            }
            set
            {
                ControlInstanse.SelectedLength = 0;
                ControlInstanse.SelectedIndex = _text.GetLines(value).Length;
            }
        }
        /// <summary>
        /// Count of selected lines
        /// </summary>
        public int SelectedLinesCount {
            get
            {
                return _text.GetLineIndexByTextIndex(ControlInstanse.SelectedIndex + ControlInstanse.SelectedLength) - _text.GetLineIndexByTextIndex(ControlInstanse.SelectedIndex);
            }
            set
            {
                ControlInstanse.SelectedLength = _text.GetLines(SelectedLineIndex, value).Length;
            }
        }
        /// <summary>
        /// Total lines count
        /// </summary>
        public int LineCount { get { return _text.LineCount; } }
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
        /// Internal text view
        /// </summary>
        CJSText _text;
        cRichTextBox _ctrlInst;
        #endregion

        #region public methods
        /// <summary>
        /// Make current word selected
        /// </summary>
        public void SelectWord()
        {
            Word wd = GetWord(null);
        }

        #region performing begin of line methods
        /// <summary>
        /// Insert specified pattern at the beginning of all selected lines
        /// </summary>
        /// <param name="newSpace">string to insert</param>
        public void AddBeginning(string newSpace)
        {
            _text = new CJSText(_ctrlInst.Text);
            CJSText _selected = _text.GetSubText(_ctrlInst.SelectedIndex, _ctrlInst.SelectedLength);
            foreach (CJSLine line in _selected.Lines)
            {
                _text[line.LineNumber].Text = newSpace + line.Text;
            }
            _ctrlInst.Text = _text;
            SelectedLineIndex = _selected[0].LineNumber;
            SelectedLinesCount = _selected.LineCount;
            ControlInstanse.NavigateToSelection();
        }

        /// <summary>
        /// Remove specified pattern from begin of line
        /// </summary>
        /// <param name="newSpace">String to find</param>
        /// <param name="allowSubStr">Allows to remove also any substring of pattern</param>
        public void RemoveBeginning(string newSpace, bool allowSubStr)
        {
            _text = new CJSText(_ctrlInst.Text);
            CJSText _selected = _text.GetSubText(_ctrlInst.SelectedIndex, _ctrlInst.SelectedLength);
            int cnt;
            string pattern;
            foreach (CJSLine line in _selected.Lines)
            {
                cnt = newSpace.Length;
                pattern = newSpace;
                //ищем любые вхождения
                while (!line.Text.StartsWith(pattern))
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
                    _text[line.LineNumber].Text = line.Text.Substring(cnt);
                }
            }
            _ctrlInst.Text = _text;
            SelectedLineIndex = _selected[0].LineNumber;
            SelectedLinesCount = _selected.LineCount;
            ControlInstanse.NavigateToSelection();
        }
        #endregion

        #region Cut-Paste methods
        /// <summary>
        /// Remove selected text and return it
        /// </summary>
        /// <returns></returns>
        public string Cut()
        {
            IsModified = true;
            string txt = _ctrlInst.SelectedText;
            int st = _ctrlInst.SelectedIndex;
            _ctrlInst.Text = string.Format("{0}{1}", _ctrlInst.Text.Substring(0, st), _ctrlInst.Text.Substring(st + txt.Length));
            _ctrlInst.SelectedIndex = st;
            _ctrlInst.NavigateToSelection();
            return txt;
        }
        /// <summary>
        /// Remove current selection and insert pattern
        /// </summary>
        /// <param name="ptrn"></param>
        public void Paste(string ptrn)
        {
            IsModified = true;
            int st = _ctrlInst.SelectedIndex;
            int len = _ctrlInst.SelectedLength;
            _ctrlInst.Text = string.Format("{0}{1}{2}", 
                _ctrlInst.Text.Substring(0, st),
                ptrn,
                _ctrlInst.Text.Substring(st + len));
            _ctrlInst.SelectedIndex = st;
            _ctrlInst.SelectedLength = ptrn.Length;
            ControlInstanse.NavigateToSelection();
        }
        #endregion

        #region Key pressed
        /// <summary>
        /// Make whole logic according to changes in text
        /// </summary>
        /// <returns>Handled flag</returns>
        public bool KeyPressed(char c)
        {
            int curPosition;
            bool isHandled = false;
            IsModified = true;
            if (c == '\r' || c == '\t') // need to perform
            {
                curPosition = _ctrlInst.SelectedIndex;
                // when 'Enter' is pressed, key char is '\r', but in text there are already two chars: "\r\n"
                if (c == '\r')
                {
                    isHandled = true;
                    _text = new CJSText(ControlInstanse.Text.Insert(curPosition, "\r\n"));
                    curPosition += 2;
                    try
                    {
                        int curLine = _text.GetLineIndexByTextIndex(curPosition);
                        int spacesCount = 0;
                        while ((_text.Lines[curLine - 1].Length > spacesCount) && (_text.Lines[curLine - 1][spacesCount] == ' ')) spacesCount++;
                        _text.Lines[curLine].Text = new string(' ', spacesCount) + _text.Lines[curLine].Text;
                        _ctrlInst.Text = _text;
                        _ctrlInst.SelectedIndex = curPosition + spacesCount;
                        _ctrlInst.NavigateToSelection();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                if (c == '\t')
                {
                    _text = new CJSText(ControlInstanse.Text);
                    _ctrlInst.Text = _ctrlInst.Text.Replace("\t", Settings.TabSwap);
                    ControlInstanse.SelectedIndex = curPosition + Settings.TabSwap.Length - 1;
                    ControlInstanse.NavigateToSelection();
                }
            }
            return isHandled;
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
            //int i = txtIndex.HasValue ? txtIndex.Value : ControlInstanse.SelectionStart;
            //while (!Language.Separators.Contains(Text[i].ToString()) && --i > 0) ;
            //wr.startPosition = i++;
            //while (!Language.Separators.Contains(Text[i].ToString()) && ++i > 0) ;
            //wr.length = i - wr.startPosition;
            //wr.text = Text.Substring(wr.startPosition, wr.length);
            return wr;
        }
        #endregion
    }
}
