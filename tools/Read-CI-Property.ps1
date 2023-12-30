[CmdletBinding()]
param(
    [Parameter(Mandatory = $true, HelpMessage = "Culture to copy information from")]
    [String]$CultureName,
    [Parameter(HelpMessage = "Property container from CultureInfo")]
    [string]$Container = 'NumberFormat',
    [Parameter(HelpMessage = "Property from CultureInfo's container property")]
    [String]$Property = 'CurrencyGroupSeparator'
)
$ci = [System.Globalization.CultureInfo]::GetCultureInfo($CultureName)

$value = $ci.$Container.$Property

Write-Host $value
