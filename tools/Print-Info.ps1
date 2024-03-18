using namespace System.Globalization
[CmdletBinding()]
param(
    [Parameter(Mandatory = $true, HelpMessage = "Culture to print information")]
    [Alias("c")]
    [String]$CultureName
)

$ci = [CultureInfo]::GetCultureInfo($CultureName)
$ri = New-Object -TypeName RegionInfo -ArgumentList $ci.LCID

Write-Host "RegionInfo" -ForegroundColor Blue
$ri | Format-List -Property CurrencyEnglishName,CurrencyNativeName
Write-Host "CultureInfo.NumberFormat" -ForegroundColor Blue
$ci.NumberFormat | Format-List -Property CurrencySymbol `
,CurrencyDecimalDigits,CurrencyDecimalSeparator `
,CurrencyGroupSeparator,CurrencyGroupSizes `
,CurrencyPositivePattern,CurrencyNegativePattern

