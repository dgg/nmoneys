[CmdletBinding()]
Param(
	[string]$task,
	[ValidateSet('Debug', 'Release')]
	[string]$configuration = 'Release'
)

function Main () {
	$script_directory = Split-Path -parent $PSCommandPath   
	$base_dir  = resolve-path $script_directory

	$psake_dir = Get-ChildItem $base_dir\tools\* -Directory | where {$_.Name.StartsWith('psake')}
	# get first directory
	$psake_dir = $psake_dir[0]

	#& $psake_dir\tools\psake.ps1 $base_dir\build\NMoneys.build.ps1 $task -properties @{"configuration"=$configuration}
	remove-module [N]Moneys
	import-module $base_dir\build\NMoneys.psm1 -DisableNameChecking
	Ensure-Release $base_dir
}
Main