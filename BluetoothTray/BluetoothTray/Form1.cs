using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BluetoothTray
{
    public partial class Form1 : Form
    {
        public BluetoothTray trayIcon;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            trayIcon.prefix = prefixValue.Text;
            trayIcon.RefreshIcon();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true; // Cancel the close operation
                //this.WindowState = FormWindowState.Minimized; // Minimize the form
                this.ShowInTaskbar = false; // Hide the form from the taskbar
                this.Hide();
            }
        }
    }
}
