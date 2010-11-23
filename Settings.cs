using System;

using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace JSEditor
{
    public static class Settings
    {
        public static string TabSwap { get; set; }
        public static bool InTestMode { get; set; }

        public static void Load()
        {
            string mes = "Не удалось загрузить настройки:";
            bool err = false;
            try
            {
                string tabS = (string)Registry.GetValue("HKEY_CURRENT_USER\\Software\\JSEditor", "numTab", 2);
                TabSwap = new string(' ', int.Parse(tabS));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                mes += "\nTabSwap";
                err = true;
                TabSwap = "  ";
            }
            if (err) MessageBox.Show(mes, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
        }

        internal static void Log(string p)
        {
            StreamWriter wr = null;
            try
            {
                wr = new StreamWriter(string.Format("{0}{1}.txt", new FileInfo(Assembly.GetExecutingAssembly().FullName).DirectoryName, DateTime.Now.ToString("yyyyMMdd")), true);
                wr.WriteLine(string.Format("{0}: {1}.", DateTime.Now.ToString(), p));
                wr.Close();
            }
            catch (Exception ex)
            {
                if (wr != null) wr.Close();
                Debug.WriteLine(ex);
            }
        }
    }
}
