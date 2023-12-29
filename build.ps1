[CmdletBinding()]
Param(
	[ValidateSet('Debug', 'Release')]
	[string]$configuration = 'Release',
	[ValidateSet('q', 'm', 'n', 'd')]
	[string]$verbosity = 'q',
	[string[]]$task = @()
)

$base_dir = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)"

$tools_pattern = Join-Path $base_dir tools *
$psake_path = Get-ChildItem -Directory -Path $tools_pattern |
Where-Object { $_.Name.StartsWith('psake') }
# first psake directory
$psake_script = Join-Path $psake_path[0] psake.ps1

$build_file = Join-Path $base_dir scripts NMoneys.build.ps1
& $psake_script -nologo `
	-buildFile $build_file -taskList $task `
	-properties @{configuration = $configuration; verbosity = $verbosity }
