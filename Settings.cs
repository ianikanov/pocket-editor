using System;

using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Diagnostics;

namespace JSEditor
{
    public static class Settings
    {
        public static string TabSwap { get; set; }

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
    }
}
