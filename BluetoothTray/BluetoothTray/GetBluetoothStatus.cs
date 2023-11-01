using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluetoothTray
{
    public static class GetBluetoothStatus
    {
        public static string[] GetBluetoothDevicesWithBatteryProcentage()
        {
            var path = Path.GetFullPath("../../../" + "GetBluetoothBatteryDevices.ps1");

            //var result = GetBluetoothBatteryDevices(path);
            var result = GetBluetoothBatteryDevices();

            //result = new string[]
            //{
            //    path
            //};

            return result;
        }

        public static string[] GetBluetoothBatteryDevices(string script = "C:\\Users\\hjm\\source\\repos\\BluetoothTray\\BluetoothTray\\GetBluetoothDevices.ps1")
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

        public static string GetSingleBluetoothDeviceBattery(string deviceName)
        {
            string script = "C:\\Users\\hjm\\source\\repos\\BluetoothTray\\BluetoothTray\\GetSingleBluetoothDeviceBattery.ps1";

            var runScript = new ProcessStartInfo()
            {
                FileName = "powershell.exe",
                Arguments = $"-NoProfile -ExecutionPolicy Bypass -File {script} \"{deviceName}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            string result = "";

            using (var process = Process.Start(runScript))
            {
                result = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
            }
            Console.WriteLine("RESULT: " + result);

            return result;
        }
    }
}
