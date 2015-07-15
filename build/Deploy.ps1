$script_directory = Split-Path -parent $PSCommandPath
$base_dir = Resolve-Path $script_directory\..

Remove-Module [N]Moneys
Import-Module "$script_directory\NMoneys.psm1" -DisableNameChecking

Generate-Zip-Files $base_dir