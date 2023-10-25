using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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
            var bluetoothDevices = GetBluetoothInfo.GetBluetoothDevicesWithBatteryProcentage();

            MenuItem[] bluetoothItems =
            {
                new MenuItem("bt1", Exit),
                new MenuItem(bluetoothDevices[0], Exit)
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

            // Custom icon
            //CreateTextIcon("J50\n12");
            CreateDoubleIcon();

            void Show_Click(Object sender, System.EventArgs e)
            {
                
            }
        }

        void Exit(object sender, EventArgs e)
        {
            notifyIcon.Visible = false;

            Application.Exit();
        }


        public void CreateTextIcon(string str)
        {
            Font fontToUse = new Font("Trebuchet MS", 10, FontStyle.Regular, GraphicsUnit.Pixel);
            Brush brushToUse = new SolidBrush(Color.White);
            Bitmap bitmapText = new Bitmap(16, 16);
            Graphics g = System.Drawing.Graphics.FromImage(bitmapText);

            IntPtr hIcon;

            g.Clear(Color.Transparent);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            g.DrawString(str, fontToUse, brushToUse, 0, -4);
            hIcon = (bitmapText.GetHicon());
            notifyIcon.Icon = System.Drawing.Icon.FromHandle(hIcon);
            //DestroyIcon(hIcon.ToInt32);
        }


        public void CreateDoubleIcon()
        {
            Font fontToUse = new Font("Trebuchet MS", 10, FontStyle.Bold, GraphicsUnit.Pixel);
            Brush brushToUse = new SolidBrush(Color.White);

            Bitmap bitmap = new Bitmap(16, 16);
            Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            IntPtr hIcon;

            g.Clear(Color.Transparent);
            //g.TextRenderingHint = System.Drawing.Text.TextRenderingHint;
            //g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            
            g.DrawString("jab", fontToUse, new SolidBrush(Color.Yellow), 0, -4, StringFormat.GenericTypographic);
            g.DrawString("50", fontToUse, new SolidBrush(Color.White), 0, 5);

            // Create an Icon from the Bitmap and assign it to the NotifyIcon
            notifyIcon.Icon = Icon.FromHandle(bitmap.GetHicon());
        }

    }

    public static class GetBluetoothInfo
    {
        public static string[] GetBluetoothDevicesWithBatteryProcentage()
        {
            var path = Path.GetFullPath("../../../"+ "GetBluetoothBatteryDevices.ps1");

            var result = GetBluetoothBatteryDevices(path);

            //result = new string[]
            //{
            //    path
            //};

            return result;
        }

        public static string[] GetBluetoothBatteryDevices(string script = "C:\\Users\\hjm\\source\\repos\\BTTaskbar\\GetBluetoothDevices.ps1")
        {
            var startInfo = new ProcessStartInfo()
            {
                FileName = "powershell.exe",
                Arguments = $"-NoProfile -ExecutionPolicy Bypass -File {script}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };
            //Process.Start(startInfo);

            List<string> devices = new List<string>();

            int counter = 0;
            using (var process = Process.Start(startInfo))
            {
                while (!process.StandardOutput.EndOfStream)
                {
                    var line = process.StandardOutput.ReadLine();
                    //Console.WriteLine(line);
                    devices.Add(line);
                    counter++;
                }
            }

            return devices.ToArray();
        }
    }
}
