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
            if (Args.Length > 0) main.OpenFile(Args[0]);
            Application.Run(main);
        }
    }
}