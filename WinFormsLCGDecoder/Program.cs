﻿using System.Windows.Forms;

namespace WinFormsLCGDecoder
{
    internal static class Program
    {
        [System.STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Home());
        }
    }
}
