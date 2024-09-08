
param(
	[string]$deviceName
)

$device = Get-PnpDevice | Where-Object {$_.Class -eq "Bluetooth"} | Where-Object {$_.FriendlyName -eq $deviceName}
try {
	$BatteryLevel = Get-PnpDeviceProperty -InstanceId $device.InstanceId -KeyName '{104EA319-6EE2-4701-BD47-8DDBF425BBE5} 2' | Where-Object { $_.Type -ne 'Empty' } | Select-Object -ExpandProperty Data
} catch {}

if ($BatteryLevel) {
	Write-Output "$BatteryLevel"
} else {
	Write-Output "err"
}