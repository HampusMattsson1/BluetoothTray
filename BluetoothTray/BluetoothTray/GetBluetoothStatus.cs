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
            //var path = Path.GetFullPath("../../../" + "GetBluetoothBatteryDevices.ps1");

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
            //string script = "GetBluetoothDevices.ps1";
            string script = "Get-PnpDevice | Where-Object {$_.Class -eq \"Bluetooth\"} | ForEach-Object { $_.FriendlyName }";

            powershellInstance.Commands.Clear();
            //powershellInstance.Commands.AddCommand($"./{script}");
            powershellInstance.Commands.AddScript(script);
			powershellInstance.AddParameter("ExecutionPolicy", "Bypass");
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
            //string script = "GetSingleBluetoothDeviceBattery.ps1";
            string script = "$device = Get-PnpDevice | Where-Object {$_.FriendlyName -eq \"" + deviceName + "\" }\r\ntry {\r\n\t$BatteryLevel = Get-PnpDeviceProperty -InstanceId $device.InstanceId -KeyName '{104EA319-6EE2-4701-BD47-8DDBF425BBE5} 2' | Where-Object { $_.Type -ne 'Empty' } | Select-Object -ExpandProperty Data\r\n} catch {}\r\n\r\nif ($BatteryLevel) {\r\n\tWrite-Output \"$BatteryLevel\"\r\n} else {\r\n\tWrite-Output \"err\"\r\n}";

            //powershellInstance.Commands.AddScript($"./{script} \"{deviceName}\"");
            powershellInstance.Commands.AddScript(script);
			powershellInstance.AddParameter("ExecutionPolicy", "Bypass");
			var powershellResult = powershellInstance.Invoke();

            string result = "";

            foreach (var line in powershellResult) {
            
                if (line != null)
                    result = line.BaseObject.ToString();
            }

            Console.WriteLine("RESULT: " + result);

            return result;
        }

        public string[] GetAnyBatteryDevices(string name)
        {
            string script = "Get-PnpDevice -FriendlyName '*" + name + "*' | Select-Object Class,FriendlyName,@{L=\"Battery\";E={(Get-PnpDeviceProperty -DeviceID $_.PNPDeviceID -KeyName '{104EA319-6EE2-4701-BD47-8DDBF425BBE5} 2').Data}} | Where-Object { [string]::IsNullOrEmpty($_.Battery) -eq $false } | Select-Object -ExpandProperty FriendlyName";

            powershellInstance.Commands.AddScript(script);
			powershellInstance.AddParameter("ExecutionPolicy", "Bypass");
			var powershellResult = powershellInstance.Invoke();

            List<string> devices = new List<string>();

            foreach (var line in powershellResult)
            {
                if (line != null)
                    devices.Add(line.BaseObject.ToString());
            }

            return devices.ToArray();
        }
    }
}
