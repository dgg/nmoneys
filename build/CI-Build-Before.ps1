Remove-Module [N]Moneys
Import-Module .\build\NMoneys.psm1 -DisableNameChecking

Ensure-Release-Folders .
Fetch-Tools .