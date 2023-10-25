﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BluetoothTray
{
    public class BluetoothTray : ApplicationContext
    {
        private NotifyIcon notifyIcon;

        public BluetoothTray()
        {
            MenuItem[] menuItems =
            {
                new MenuItem("test", Exit),
                new MenuItem("Exit", Exit)
            };

            notifyIcon = new NotifyIcon()
            {
                Icon = SystemIcons.Application,
                ContextMenu = new ContextMenu(menuItems),
                Visible = true
            };
        }

        void Exit(object sender, EventArgs e)
        {
            notifyIcon.Visible = false;

            Application.Exit();
        }

    }
}