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
        private Form1 form;
        private GetBluetoothStatus bluetoothStatus;
        public NotifyIcon notifyIcon;
        public string prefix = "";
        private string selectedBluetoothDevice = "";
        private string lastDeviceBattery = "";

        private int minuteInterval = 5;
        private string scriptDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"../../.."));

        public BluetoothTray(Form1 form)
        {
            bluetoothStatus = new GetBluetoothStatus(scriptDirectory);

            var bluetoothDevices = bluetoothStatus.GetBluetoothDevicesWithBatteryProcentage();

            MenuItem[] bluetoothItems = bluetoothDevices.Select(b => new MenuItem(b, ChangeActiveBluetoothDevice)).ToArray();


            MenuItem[] menuItems =
            {
                new MenuItem("Update now", UpdateBattery),
                new MenuItem(MenuMerge.Add, 0, Shortcut.CtrlS, "Bluetooth Device", Show_Click, new System.EventHandler(Show_Click), new System.EventHandler(Show_Click), bluetoothItems),
                new MenuItem("Exit", Exit)
            };

            notifyIcon = new NotifyIcon()
            {
                Icon = SystemIcons.Application,
                ContextMenu = new ContextMenu(menuItems),
                Visible = true
            };

            form.trayIcon = this;
            this.form = form;


            notifyIcon.MouseClick += (sender, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    this.form.Show();
                    this.form.WindowState = FormWindowState.Normal;
                    this.form.BringToFront();
                }
            };

            // Set icon
            CreateDoubleIcon("----", "----");

            
            var startTimeSpan = TimeSpan.Zero;
            //var periodTimeSpan = TimeSpan.FromMinutes(5);
            var interval = TimeSpan.FromMinutes(10);

            var updateStatus = new System.Threading.Timer((e) =>
            {
                if (selectedBluetoothDevice != "")
                {
                    UpdateBattery();
                }
            }, null, startTimeSpan, interval);
        }

        public void UpdateBattery()
        {
            string updateTime = DateTime.Now.TimeOfDay.ToString(@"hh\:mm\:ss");
            notifyIcon.Text = selectedBluetoothDevice + "\r\nUPDATED " + updateTime;

            string newDeviceStatus = bluetoothStatus.GetSingleBluetoothDeviceBattery(selectedBluetoothDevice);
            lastDeviceBattery = newDeviceStatus;
            Console.WriteLine(newDeviceStatus);

            Console.WriteLine("UPDATED " + updateTime);

            Console.WriteLine("Prefix: " + prefix);

            CreateDoubleIcon(prefix, newDeviceStatus);
        }

        private void UpdateBattery(object sender, EventArgs e)
        {
            UpdateBattery();
        }

        void Exit(object sender, EventArgs e)
        {
            notifyIcon.Visible = false;

            Application.Exit();
        }
        void Show_Click(Object sender, System.EventArgs e) { }


        void ChangeActiveBluetoothDevice(object sender, EventArgs e)
        {
            MenuItem clickedItem = sender as MenuItem;

            Console.WriteLine("new device selected: " + clickedItem.Text);

            selectedBluetoothDevice = clickedItem.Text;
            UpdateBattery();
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


        public void CreateDoubleIcon(string topText, string bottomText)
        {
            Font fontToUse = new Font("Trebuchet MS", 10, FontStyle.Bold, GraphicsUnit.Pixel);
            Brush brushToUse = new SolidBrush(Color.White);

            Bitmap bitmap = new Bitmap(16, 16);
            Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            IntPtr hIcon;

            g.Clear(Color.Transparent);
            //g.TextRenderingHint = System.Drawing.Text.TextRenderingHint;
            //g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            
            g.DrawString(topText, fontToUse, new SolidBrush(Color.Yellow), 0, -4, StringFormat.GenericTypographic);
            g.DrawString(bottomText, fontToUse, new SolidBrush(Color.White), 0, 5);

            // Create an Icon from the Bitmap and assign it to the NotifyIcon
            notifyIcon.Icon = Icon.FromHandle(bitmap.GetHicon());
        }

    }
}
