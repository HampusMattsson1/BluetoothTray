﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BluetoothTray
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var form = new Form1();
            var searchForm = new SearchForm();
            var bluetoothTray = new BluetoothTray(form, searchForm);
            //var bluetoothTray2 = new BluetoothTray();

            Application.Run();
        }
    }
}
