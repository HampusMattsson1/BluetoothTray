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
        private SearchForm searchForm;
        private GetBluetoothStatus bluetoothStatus;
        public NotifyIcon notifyIcon;
        public string prefix = "";
        private string selectedBluetoothDevice = "";
        private string lastDeviceBattery = "";

        MenuItem[] menuItems = null;

        private int minuteInterval = 5;
        private string scriptDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"../../.."));

        private MenuItem[] anydevices = null;

        public BluetoothTray(Form1 form, SearchForm searchForm)
        {
            bluetoothStatus = new GetBluetoothStatus(scriptDirectory);

            var bluetoothDevices = bluetoothStatus.GetBluetoothDevicesWithBatteryProcentage();

            MenuItem[] bluetoothItems = bluetoothDevices.Select(b => new MenuItem(b, ChangeActiveBluetoothDevice)).ToArray();


            menuItems = new MenuItem[]
            {
                new MenuItem("Update now", UpdateBattery),
                new MenuItem("Search for any device", OpenSearchForm),
                new MenuItem(MenuMerge.Add, 0, Shortcut.None, "Any device results", Show_Click, new System.EventHandler(Show_Click), new System.EventHandler(Show_Click), anydevices),
                new MenuItem(MenuMerge.Add, 0, Shortcut.None, "Bluetooth devices", Show_Click, new System.EventHandler(Show_Click), new System.EventHandler(Show_Click), bluetoothItems),
                new MenuItem("Exit", Exit)
            };

            notifyIcon = new NotifyIcon()
            {
                Icon = SystemIcons.Application,
                ContextMenu = new ContextMenu(menuItems),
                Visible = true
            };

            form.trayIcon = this;
            searchForm.trayIcon = this;
            this.form = form;
            this.searchForm = searchForm;


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

        public void UpdateBattery(object sender = null, EventArgs e = null)
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

        void OpenSearchForm(object sender, EventArgs e)
        {
            this.searchForm.Show();
            this.searchForm.WindowState = FormWindowState.Normal;
            this.searchForm.BringToFront();
        }

        public void GetBatteryDevices(string name)
        {
            var a = bluetoothStatus.GetAnyBatteryDevices(name);

            anydevices = a.Select(b => new MenuItem(b, ChangeActiveBluetoothDevice)).ToArray();

            MenuItem item = menuItems[2];
            item.MenuItems.Clear();

            foreach (MenuItem menuItem in anydevices)
            {
                item.MenuItems.Add(menuItem);
            }
        }


        //public void CreateTextIcon(string str)
        //{
        //    Font fontToUse = new Font("Trebuchet MS", 10, FontStyle.Regular, GraphicsUnit.Pixel);
        //    Brush brushToUse = new SolidBrush(Color.White);
        //    Bitmap bitmapText = new Bitmap(16, 16);
        //    Graphics g = System.Drawing.Graphics.FromImage(bitmapText);

        //    IntPtr hIcon;

        //    g.Clear(Color.Transparent);
        //    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
        //    g.DrawString(str, fontToUse, brushToUse, 0, -4);
        //    hIcon = (bitmapText.GetHicon());
        //    notifyIcon.Icon = System.Drawing.Icon.FromHandle(hIcon);
        //    //DestroyIcon(hIcon.ToInt32);
        //}


        public void CreateDoubleIcon(string topText, string bottomText)
        {
            using (Font fontToUse = new Font("Trebuchet MS", 10, FontStyle.Bold, GraphicsUnit.Pixel))
            using (Brush brushToUse = new SolidBrush(Color.White))
            using (Bitmap bitmap = new Bitmap(16, 16))
            using (Graphics g = System.Drawing.Graphics.FromImage(bitmap))
            {
                g.Clear(Color.Transparent);
                //g.TextRenderingHint = System.Drawing.Text.TextRenderingHint;
                //g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                using (Brush yellowBrush = new SolidBrush(Color.Yellow))
                using (Brush whiteBrush = new SolidBrush(Color.White))
                {
                    g.DrawString(topText, fontToUse, yellowBrush, 0, -4, StringFormat.GenericTypographic);
                    g.DrawString(bottomText, fontToUse, whiteBrush, 0, 5);
                }

                // Create an Icon from the Bitmap and assign it to the NotifyIcon
                using (Icon icon = Icon.FromHandle(bitmap.GetHicon()))
                {
                    notifyIcon.Icon = (Icon)icon.Clone();
                }
            }
        }

    }
}
