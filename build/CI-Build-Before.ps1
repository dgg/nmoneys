Remove-Module [N]Moneys
Import-Module .\build\NMoneys.psm1 -DisableNameChecking

function fetch-extra-tools($base)
{
	$tools_dir = Join-Path $base tools
	
	$nuget = Join-Path $tools_dir Nuget\Nuget.exe
	& "$nuget" install opencover -OutputDirectory $tools_dir
	Throw-If-Error "Could not install package tool 'opencover'"
	& "$nuget" install coveralls.net -OutputDirectory $tools_dir
	Throw-If-Error "Could not install package tool 'coveralls.net'"
}

Ensure-Release-Folders .
fetch-extra-tools .
Restore-Packages .

