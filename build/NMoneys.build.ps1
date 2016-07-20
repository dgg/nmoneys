Framework "4.6"

properties {
  $configuration = 'Release'
  $base_dir  = resolve-path ..
  $release_dir = "$base_dir\release"
}

task default -depends Clean, Compile, Sign, Document, Test, CopyArtifacts, BuildArtifacts

task Clean {
	Exec { msbuild "$base_dir\NMoneys.sln" /t:clean /p:configuration=$configuration /m /v:m /clp:Summary }
	Remove-Item $release_dir -Recurse -Force -ErrorAction SilentlyContinue | out-null
}

task Compile { 
	Exec { msbuild "$base_dir\NMoneys.sln" /p:configuration=$configuration /m /v:m /clp:Summary }
}

task Sign -depends ensureRelease, Compile { 
	Sign-Assemblies $base_dir $configuration
}

task Document -depends ensureRelease {
	Build-Documentation $base_dir $configuration
}

task ensureRelease -depends importModule {
	Ensure-Release-Folders $base_dir
}

task importModule {
	Remove-Module [N]Moneys
	Import-Module "$base_dir\build\NMoneys.psm1" -DisableNameChecking
}

task Test -depends ensureRelease {
	$core = get-test-assembly-name $base_dir $configuration "NMoneys"
	$exchange = get-test-assembly-name $base_dir $configuration "NMoneys.Exchange"
	$serialization = get-test-assembly-name $base_dir $configuration "NMoneys.Serialization"
	$mongo_db = get-test-assembly-name $base_dir $configuration "NMoneys.Serialization.Mongo_DB"

	run-tests $base_dir $release_dir ($core, $exchange, $serialization, $mongo_db)
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

function get-test-assembly-name($base, $config, $name)
{
	return "$base\src\$name.Tests\bin\$config\$name.Tests.dll"
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
	
	$input_xml = Join-Path $release TestResult.xml
	$output_html = Join-Path $release TestResult.html
	
	exec { & $nunit_orange $input_xml $output_html }
}