$devices = Get-PnpDevice | Where-Object {$_.Class -eq "Bluetooth"} | ForEach-Object { $_.FriendlyName }

foreach($device in $devices)
{
	# Define the friendly name of your Bluetooth device
	$BTDeviceFriendlyName = $device

	# Get the PnP device with the specified friendly name
	$BTHDevices = Get-PnpDevice -FriendlyName "*$($BTDeviceFriendlyName)*"

	if ($BTHDevices) {
		# Loop through each device and get its battery level
		$BatteryLevels = foreach ($Device in $BTHDevices) {
			# Get the battery property of the device
			$BatteryProperty = Get-PnpDeviceProperty -InstanceId $Device.InstanceId -KeyName '{104EA319-6EE2-4701-BD47-8DDBF425BBE5} 2' | Where-Object { $_.Type -ne 'Empty' } | Select-Object -ExpandProperty Data

			if ($BatteryProperty) {
				$BatteryProperty
			}
		}

		if ($BatteryLevels) {
			# Print out the battery level of the device
			Write-Output "Battery Level of $($BTDeviceFriendlyName): $BatteryLevels%"
		} else {
			#Write-Output "No battery level information found for $($BTDeviceFriendlyName) devices."
		}
	}

}