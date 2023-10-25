using System;
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
            MenuItem[] bluetoothItems =
            {
                new MenuItem("bt1", Exit)
            };


            MenuItem[] menuItems =
            {
                new MenuItem(MenuMerge.Add, 0, Shortcut.CtrlS, "Bluetooth Devices", Exit, new System.EventHandler(Show_Click), new System.EventHandler(Show_Click), bluetoothItems),
                new MenuItem("test", Exit),
                new MenuItem("Exit", Exit)
            };

            notifyIcon = new NotifyIcon()
            {
                Icon = SystemIcons.Application,
                ContextMenu = new ContextMenu(menuItems),
                Visible = true
            };

            void Show_Click(Object sender, System.EventArgs e)
            {
                
            }
        }

        void Exit(object sender, EventArgs e)
        {
            notifyIcon.Visible = false;

            Application.Exit();
        }

    }

    public static class GetBluetoothInfo
    {

    }
}
