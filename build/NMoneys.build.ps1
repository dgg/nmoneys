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

function run-tests($base, $release, $test_assemblies){
	$nunit_console = "$base\tools\NUnit.Runners.lite\nunit-console.exe"
	
	exec { & $nunit_console $test_assemblies /nologo /nodots /result="$release\TestResult.xml"  }
}

function report-on-test-results($base, $release)
{
	$nunit_summary_path = "$base\tools\NUnitSummary"
	$nunit_summary = Join-Path $nunit_summary_path "nunit-summary.exe"

	$alternative_details = Join-Path $nunit_summary_path "AlternativeNUnitDetails.xsl"
	$alternative_details = "-xsl=" + $alternative_details

	exec { & $nunit_summary $release\TestResult.xml -html -o="$release\TestSummary.htm" }
	exec { & $nunit_summary $release\TestResult.xml -html -o="$release\TestDetails.htm" $alternative_details -noheader }
}