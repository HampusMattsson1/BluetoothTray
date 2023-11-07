using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;

namespace BluetoothTray
{
    public class GetBluetoothStatus
    {
        private PowerShell powershellInstance;
        private string scriptLocation = "";

        public GetBluetoothStatus(string scriptLocation)
        {
            this.scriptLocation = scriptLocation;

            powershellInstance = PowerShell.Create();
            powershellInstance.AddScript("cd '" + scriptLocation + "'");
            powershellInstance.Invoke();
        }

        public string[] GetBluetoothDevicesWithBatteryProcentage()
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

        public string[] GetBluetoothBatteryDevices()
        {
            string script = "GetBluetoothDevices.ps1";

            powershellInstance.Commands.Clear();
            powershellInstance.Commands.AddCommand($"./{script}");
            var powershellResult = powershellInstance.Invoke();

            List<string> devices = new List<string>();

            foreach (var line in powershellResult)
            {
                if (line != null)
                    devices.Add(line.BaseObject.ToString());
            }

            return devices.ToArray();
        }

        public string GetSingleBluetoothDeviceBattery(string deviceName)
        {
            string script = "GetSingleBluetoothDeviceBattery.ps1";

            powershellInstance.Commands.AddScript($"./{script} \"{deviceName}\"");
            var powershellResult = powershellInstance.Invoke();

            string result = "";

            foreach (var line in powershellResult) {
            
                if (line != null)
                    result = line.BaseObject.ToString();
            }

            Console.WriteLine("RESULT: " + result);

            return result;
        }
    }
}
