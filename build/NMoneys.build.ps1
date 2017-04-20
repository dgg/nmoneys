Framework "4.6"

properties {
  $configuration = 'Release'
  $base_dir  = resolve-path ..
  $release_dir = "$base_dir\release"
}

task default -depends Clean, Compile, Sign, Document, Test, CopyArtifacts, BuildArtifacts

task Clean -depends importModules {
	$msbuild = find-msbuild

	Exec { & $msbuild "$base_dir\NMoneys.sln" /t:clean /p:configuration=$configuration /m /v:m /clp:Summary }
	Remove-Item $release_dir -Recurse -Force -ErrorAction SilentlyContinue | out-null
}

task Compile -depends importModules {
	$msbuild = find-msbuild
	
	Exec { & $msbuild "$base_dir\NMoneys.sln" /p:configuration=$configuration /m /v:m /clp:Summary }
}

task Sign -depends ensureRelease, Compile { 
	Sign-Assemblies $base_dir $configuration
}

task Document -depends ensureRelease {
	Build-Documentation $base_dir $configuration
}

task ensureRelease -depends importModules {
	Ensure-Release-Folders $base_dir
}

task importModules {
	Remove-Module [N]Moneys
	Import-Module "$base_dir\build\NMoneys.psm1" -DisableNameChecking

	Remove-Module [V]SSetup
	$vssetup_dir = Find-Versioned-Folder -base $base_dir\tools -beginning 'VSSetup'
	Import-Module  $vssetup_dir\VSSetup.psd1 -DisableNameChecking
}

task Test -depends ensureRelease {
	$test_assemblies = Find-Test-Assemblies $base_dir $configuration

	#run-tests $base_dir $release_dir $test_assemblies
	Run-Core-Tests $base_dir $configuration

	report-on-test-results $base_dir $release_dir
}

task CopyArtifacts -depends ensureRelease {
	Copy-Artifacts $base_dir $configuration
}

task BuildArtifacts -depends ensureRelease {
	Generate-Packages $base_dir
	Generate-Zip-Files $base_dir
}

task ? -Description "Helper to display task info" {
	Write-Documentation
}

function run-tests($base, $release, $test_assemblies)
{
	$runner_dir = Find-Versioned-Folder -base $base\tools -beginning 'NUnit.ConsoleRunner'
	$nunit_console = Join-Path $runner_dir tools\nunit3-console.exe

	exec { & $nunit_console $test_assemblies --result:"$release\TestResult.xml" --noheader  }
}

function report-on-test-results($base, $release)
{
	$orange_dir = Find-Versioned-Folder -base $base\tools -beginning 'NUnitOrange'
	$nunit_orange = Join-Path $orange_dir tools\NUnitOrange.exe
	
	('TestResult', 'NMoneys.TestResult.core', 'NMoneys.Exchange.TestResult.core') |
	% { 
		$input_xml = Join-Path $release "$_.xml"
		$output_html = Join-Path $release "$_.html"
		exec { & $nunit_orange $input_xml $output_html } 
	}
}

function find-msbuild()
{
	$vs_dir = Get-VSSetupInstance | Select-Object -ExpandProperty InstallationPath
	$msbuild = Join-Path $vs_dir MSBuild\15.0\Bin\MSBuild.exe
	return $msbuild;
}