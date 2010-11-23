using System;

using System.Collections.Generic;
using System.Windows.Forms;

namespace JSEditor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [MTAThread]
        static void Main(string[] Args)
        {
            Form1 main = new Form1();
            // open parameters
            try
            {
                if (Args.Length > 0)
                {
                    string foundUnexpectedParams = null;
                    Settings.InTestMode = false;
                    string fOpen = null;
                    int curArg = 0;
                    // check
                    while (curArg < Args.Length)
                    {
                        if (Args[curArg] == "-t") Settings.InTestMode = true;
                        else if (Args[curArg] == "-f")
                        {
                            curArg++;
                            fOpen = Args[curArg];
                        }
                        else
                        {
                            foundUnexpectedParams = (foundUnexpectedParams == null ? "" : foundUnexpectedParams + ",") + Args[curArg];
                        }
                        curArg++;
                    }
                    // do
                    if (fOpen != null) main.OpenFile(fOpen);
                    if (foundUnexpectedParams != null)
                    {
                        if (Args.Length == 1) main.OpenFile(foundUnexpectedParams);
                        else throw new ArgumentException("Wrong parameters:" + foundUnexpectedParams);
                    }
                }
            }
            catch (Exception ex)
            {
                Settings.Log("Loading parameters wrong: " + ex.Message);
                MessageBox.Show(ex.Message);
                MessageBox.Show("Usage:\r\n-t = test mode on;\r\n-f filename = open specified file");
            }
            // launch
            try
            {
                Application.Run(main);
            }
            catch (Exception ex)
            {
                Settings.Log(ex.ToString());
            }
        }
    }
}