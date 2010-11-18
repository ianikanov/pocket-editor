using System;

using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace JSEditor
{
    /// <summary>
    /// 
    /// </summary>
    public class CFileManager
    {
        #region Private fields
        private string _fileExtensionsList = "PHP files|*.php|ActionScript files|*.as|HTM files|*.htm|HTML files|*.html|JavaScript files|*.js|C# files|*.cs|C++ files|*.cpp|SQL files|*.sql|All code files|*.php;*.as;*.htm;*.html;*.js;*.cs;*.cpp;*.sql|All files|*.*";
        private List<FileInfo> _OpenedFiles = new List<FileInfo>();
        #endregion

        #region Open file
        /// <summary>
        /// Open and read specified file and keep link
        /// </summary>
        /// <param name="path">file path to open</param>
        /// <param name="text">return file content</param>
        /// <returns>JSFile struct, that can be used in other methods</returns>
        public JSFile OpenFile(string path, out string text)
        {
            return OpenFile(path, true, out text);
        }

        /// <summary>
        /// Open and read specified file
        /// </summary>
        /// <param name="path">file path to open</param>
        /// <param name="keepInMemory">set is link to this file should be stored in memory</param>
        /// <param name="text">return file content</param>
        /// <returns>JSFile struct, that can be used in other methods</returns>
        public JSFile OpenFile(string path, bool keepInMemory, out string text)
        {
            JSFile fd = new JSFile();
            fd.descriptor = -1;
            FileInfo fil = new FileInfo(path);
            if (!fil.Exists)
            {
                throw new FileNotFoundException(string.Format("File {0} not found.", path));
            }
            text = null;
            StreamReader sr = null;
            try
            {
                sr = fil.OpenText();
                text = sr.ReadToEnd().Replace("\t", Settings.TabSwap);
                sr.Close();
            }
            catch (Exception ex)
            {
                if (sr != null) sr.Close();
                throw new EndOfStreamException("Can not read file " + path, ex);
            }
            if (keepInMemory)
            {
                _OpenedFiles.Add(fil);
                fd.descriptor = _OpenedFiles.Count - 1;
            }
            return fd;
        }
        #endregion

        #region Save file
        /// <summary>
        /// Save previously opened file with new text
        /// </summary>
        /// <param name="fd">Internal file descriptor</param>
        /// <param name="text">Text to store</param>
        public void SaveFile(JSFile fd, string text)
        {
            FileInfo fil = _OpenedFiles[fd.descriptor];
            SaveFile(fil, text);
        }

        /// <summary>
        /// Save file into specified path and store link in memory
        /// </summary>
        /// <param name="path">Phisical path of file</param>
        /// <param name="text">Text to store</param>
        /// <returns>Internal file descriptor</returns>
        public JSFile SaveFile(string path, string text)
        {
            return SaveFile(path, text, true);
        }

        /// <summary>
        /// Save file into specified path
        /// </summary>
        /// <param name="path">Phisical path of file</param>
        /// <param name="text">Text to store</param>
        /// <param name="keepInMemory">Specify need to store link in memory or not</param>
        /// <returns>Internal file descriptor</returns>
        public JSFile SaveFile(string path, string text, bool keepInMemory)
        {
            FileInfo fil = new FileInfo(path);
            SaveFile(fil, text);
            JSFile fd = new JSFile();
            fd.descriptor = -1;
            if (keepInMemory)
            {
                _OpenedFiles.Add(fil);
                fd.descriptor = _OpenedFiles.Count - 1;
            }
            return fd;
        }

        /// <summary>
        /// Make saving text into specified file
        /// </summary>
        /// <param name="fil">Info about target file</param>
        /// <param name="text">Text to store</param>
        private void SaveFile(FileInfo fil, string text)
        {
            StreamWriter sw = null;
            try
            {
                sw = fil.CreateText();
                sw.Write(text.Replace(Settings.TabSwap, "\t"));
                sw.Close();
            }
            catch (Exception ex)
            {
                if (sw != null) sw.Close();
                throw new EndOfStreamException("File can not be written: " + fil.Name, ex);
            }
        }
        #endregion

        #region Remove file
        /// <summary>
        /// Removed link to specified file from memeory
        /// </summary>
        /// <param name="fd">File descriptor</param>
        public void CloseFile(JSFile fd)
        {
            _OpenedFiles.RemoveAt(fd.descriptor);
        }
        #endregion

        #region FileInfo
        /// <summary>
        /// Returns extension of stored file
        /// </summary>
        /// <param name="fd">File descriptor</param>
        /// <returns>Extention</returns>
        public string GetExtension(JSFile fd)
        {
            return _OpenedFiles[fd.descriptor].Extension;
        }

        /// <summary>
        /// Returns full name with path of stored file
        /// </summary>
        /// <param name="fd">File descriptor</param>
        /// <returns>Extention</returns>
        public string GetFullName(JSFile fd)
        {
            return _OpenedFiles[fd.descriptor].FullName;
        }

        /// <summary>
        /// Returns name of stored file
        /// </summary>
        /// <param name="fd">File descriptor</param>
        /// <returns>Extention</returns>
        public string GetName(JSFile fd)
        {
            return _OpenedFiles[fd.descriptor].Name;
        }
        #endregion

        #region Dialogs
        /// <summary>
        /// Displays OpenFile dialog and return file path or null if cancel
        /// </summary>
        /// <param name="selected">default extension filter is set by selected language</param>
        /// <returns>File path</returns>
        public string ShowOpenFileDialog(DevelopLanguages selected)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = _fileExtensionsList;
            switch (selected)
            {
                case DevelopLanguages.ActionScript: ofd.FilterIndex = 2; break;
                case DevelopLanguages.Cpp: ofd.FilterIndex = 7; break;
                case DevelopLanguages.CSharp: ofd.FilterIndex = 6; break;
                case DevelopLanguages.Html: ofd.FilterIndex = 3; break;
                case DevelopLanguages.Javascript: ofd.FilterIndex = 5; break;
                case DevelopLanguages.Php: ofd.FilterIndex = 1; break;
                case DevelopLanguages.Sql: ofd.FilterIndex = 8; break;
                default: ofd.FilterIndex = 1; break;
            }
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                return ofd.FileName;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Displays SaveFile dialog and return file path or null if cancel
        /// </summary>
        /// <param name="selected">default extension filter is set by selected language</param>
        /// <returns>File path</returns>
        public string ShowSaveFileDialog(DevelopLanguages selected)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = _fileExtensionsList;
            switch (selected)
            {
                case DevelopLanguages.ActionScript: sfd.FilterIndex = 2; break;
                case DevelopLanguages.Cpp: sfd.FilterIndex = 7; break;
                case DevelopLanguages.CSharp: sfd.FilterIndex = 6; break;
                case DevelopLanguages.Html: sfd.FilterIndex = 3; break;
                case DevelopLanguages.Javascript: sfd.FilterIndex = 5; break;
                case DevelopLanguages.Php: sfd.FilterIndex = 1; break;
                case DevelopLanguages.Sql: sfd.FilterIndex = 8; break;
                default: sfd.FilterIndex = 1; break;
            }
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                return sfd.FileName;
            }
            else
            {
                return null;
            }
        }
        #endregion
    }

    /// <summary>
    /// Structure, used for represent file descriptor for JSEditor
    /// </summary>
    public struct JSFile
    {
        public int descriptor;
    }
}
