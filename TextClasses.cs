using System;

using System.Collections.Generic;
using System.Text;

namespace JSEditor
{
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
            _text = initial;
            if (initial.IndexOf("\r\n") > -1) throw new ArgumentException("CJSLine(string): line contains return caret symbol");
            Length = initial.Length;
            LineNumber = num;
            Offset = 0;
            EndOffset = 0;
        }

        /// <summary>
        /// Creates line structure for specified line of text
        /// </summary>
        /// <param name="initial">Line's text</param>
        /// <param name="num">Ordering number in parent text</param>
        /// <param name="offset">Set offset of current text in initial line from left side</param>
        /// <param name="endOffset">Set offset of current text in initial line from right side</param>
        public CJSLine(string initial, int num, int offset, int endOffset)
        {
            _text = initial;
            if (initial.IndexOf("\r\n") > -1) throw new ArgumentException("CJSLine(string): line contains return caret symbol");
            Length = initial.Length;
            LineNumber = num;
            Offset = offset;
            EndOffset = endOffset;
        }
        #endregion

        #region Text
        private string _text;
        /// <summary>
        /// Gets or sets full text of line
        /// </summary>
        public string Text {
            get { return _text; }
            set { _text = value; Length = value.Length; }
        }
        #endregion

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

        #region Neibours properties
        public CJSLine LeftNeibour { get; set; }
        public CJSLine RightNeibour { get; set; }
        #endregion

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

        /// <summary>
        /// Get line as a string
        /// </summary>
        public override string ToString()
        {
            return Text;
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
        /// Creates an empty text
        /// </summary>
        public CJSText()
        {
            LineCount = 0;
            Lines = new List<CJSLine>();
        }
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
                Lines.Add(cur);
                //and prepare next step
                prev = cur;
                initialText = initialText.Substring(ind + 2);
            }
            //rest
            cur = new CJSLine(initialText, lines++);
            cur.LeftNeibour = prev;
            if (prev != null) prev.RightNeibour = cur;
            Lines.Add(cur);
            LineCount = lines;
        }
        #endregion

        #region Once-Set Properties
        private int _initialLineStart = -1;
        private int _initialLineCount = -1;
        /// <summary>
        /// Gets or once sets line start number when text is created by SubText
        /// </summary>
        public int InitialLineStart
        {
            get { return _initialLineStart; }
            set { if (_initialLineStart != -1) _initialLineStart = value; }
        }
        /// <summary>
        /// Gets or once sets lines count when text is created by SubText
        /// </summary>
        public int InitialLineCount
        {
            get { return _initialLineCount; }
            set { if (_initialLineCount != -1) _initialLineCount = value; }
        }
        #endregion

        #region Text methods
        #region GetLineIndex
        /// <summary>
        /// Gets line number in current text, where specified position is placed
        /// </summary>
        /// <param name="txtIx">index position in text</param>
        /// <returns>Line number</returns>
        public int GetLineIndexByTextIndex(int txtIx)
        {
            int ix = 0;
            while ((ix < LineCount || notInRange()) && GetLines(ix + 1).Length < txtIx) ix++;
            return ix;
        }
        #endregion

        #region Get lines as text
        /// <summary>
        /// Return specified number of lines from text
        /// </summary>
        /// <param name="count">If >0 then return (count) of fisrt lines, else return (-count) of last lines</param>
        /// <returns>Lines as one string</returns>
        public string GetLines(int count)
        {
            if (count == 0) return "";
            if (count > 0) return GetLines(0, count);
            else return GetLines(LineCount - count, -count);
        }

        /// <summary>
        /// Returns number of lines from specified first line
        /// </summary>
        /// <param name="first">Index of first line for selection</param>
        /// <param name="len">Number of lines to select</param>
        /// <returns>Lines as one string</returns>
        public string GetLines(int first, int count)
        {
            string[] ss = new string[count];
            for (int i = first; i < count; i++) ss[i - first] = Lines[i];
            return string.Join("\r\n", ss);
        }
        #endregion

        #region Get sub-text
        /// <summary>
        /// Gets exact text from specified start index and length with full lines
        /// </summary>
        /// <param name="start">Start text index</param>
        /// <param name="len">Length of text</param>
        /// <returns>Instance of CJSText, that contained ordered line</returns>
        public CJSText GetSubText(int start, int len)
        {
            return GetSubText(start, len, true);
        }

        /// <summary>
        /// Get subtext of internal with borders as specified
        /// </summary>
        /// <param name="start">Start text index</param>
        /// <param name="len">Length of text</param>
        /// <param name="fullLines">Use exact ordered text, or full lines, container selected positions</param>
        /// <returns>Instance of CJSText, that contained ordered line</returns>
        public CJSText GetSubText(int start, int len, bool fullLines)
        {
            if (start < 0 || len < 0) notInRange();
            int totalSymbols = 0;
            int lineIndex = 0;
            CJSText ret = new CJSText();
            //for needed lines
            while (lineIndex < LineCount)
            {
                if (totalSymbols + Lines[lineIndex].Length >= start && totalSymbols <= start + len)
                {
                    ret.InitialLineStart = lineIndex;
                    if (fullLines) ret.AddLine(new CJSLine(Lines[lineIndex], lineIndex));
                    else ret.AddLine(
                        new CJSLine(
                            Lines[lineIndex].Text.Substring(
                                Math.Max(start - totalSymbols, 0),
                                Math.Min(len - ret.ToString().Length, Lines[lineIndex].Length)),
                            lineIndex));
                }
                //go next
                totalSymbols += Lines[lineIndex].Length + 2;
                lineIndex++;
            }
            ret.InitialLineCount = ret.LineCount;
            return ret;
        }
        #endregion

        #region update text
        /// <summary>
        /// Reintegrate text, created from this, using SubText methods with its' changes
        /// </summary>
        /// <param name="sub">Edited SubText result</param>
        public void UpdateText(CJSText sub)
        {
            for (int i = 0; i < sub.LineCount; i++)
            {
                Lines[sub.Lines[i].LineNumber].Text = string.Format("{0}{1}{2}",
                    Lines[sub.Lines[i].LineNumber].Text.Substring(0, sub.Lines[i].Offset),
                    sub.Lines[i],
                    Lines[sub.Lines[i].LineNumber].Text.Substring(sub.Lines[i].Length - sub.Lines[i].EndOffset));
            }
        }
        #endregion

        #region Not in range exception
        /// <summary>
        /// Generate exception if reached end of lines list
        /// </summary>
        /// <returns>ArgumentOutOfRangeException</returns>
        private bool notInRange()
        {
            throw new ArgumentOutOfRangeException("Specified index is not valid");
        }
        #endregion
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
            string[] prAr = new string[txt.LineCount];
            int i = 0;
            foreach (string s in txt.Lines) prAr[i++] = s;
            return string.Join("\r\n", prAr);
        }

        /// <summary>
        /// Return text as a string
        /// </summary>
        /// <returns>Text</returns>
        public override string ToString()
        {
            string[] prAr = new string[LineCount];
            int i = 0;
            foreach (string s in Lines) prAr[i++] = s;
            return string.Join("\r\n", prAr);
        }
        #endregion

        #region Lines methods
        /// <summary>
        /// Remove all lines from text
        /// </summary>
        public void Clear()
        {
            LineCount = 0;
            Lines.Clear();
        }

        /// <summary>
        /// Add new line into the end of text
        /// </summary>
        public void AddLine(CJSLine cJSLine)
        {
            Lines.Add(cJSLine);
            if (LineCount != 0)
            {
                Lines[LineCount - 1].RightNeibour = cJSLine;
                cJSLine.LeftNeibour = Lines[LineCount - 1];
            }
            LineCount++;
        }
        #endregion
    }
}
