properties {
  $configuration = 'Release'
  $base_dir  = resolve-path ..
  $release_dir = "$base_dir\release"
}

task default -depends Clean, Compile, Sign, Document, Test, CopyArtifacts, BuildArtifacts

task Clean {
	Exec { msbuild "$base_dir\NMoneys.sln" /t:clean /p:configuration=$configuration /m /v:m }
	Remove-Item $release_dir -Recurse -Force -ErrorAction SilentlyContinue | out-null
}

task Compile { 
	Exec { msbuild "$base_dir\NMoneys.sln" /p:configuration=$configuration /m /v:m }
}

task Sign -depends EnsureRelease, Compile { 
	Sign-Assemblies $base_dir $configuration
}

task Document -depends EnsureRelease {
	Build-Documentation $base_dir $configuration
}

task EnsureRelease -depends ImportModule {
	Ensure-Release-Folders $base_dir
}

task ImportModule {
	Remove-Module [N]Moneys
	Import-Module "$base_dir\build\NMoneys.psm1" -DisableNameChecking
}

task Test -depends EnsureRelease {
	$core = Test-Assembly $base_dir $configuration "NMoneys"
	$exchange = Test-Assembly $base_dir $configuration "NMoneys.Exchange"
	$serialization = Test-Assembly $base_dir $configuration "NMoneys.Serialization"

	Run-Tests $base_dir $release_dir ($core, $exchange, $serialization)
	Report-On-Test-Results $base_dir $release_dir
}

task CopyArtifacts -depends EnsureRelease {
	Copy-Artifacts $base_dir $configuration
}

task BuildArtifacts -depends EnsureRelease {
	Generate-Packages $base_dir
	Generate-Zip-Files $base_dir
}

task ? -Description "Helper to display task info" {
	Write-Documentation
}

function Test-Assembly($base, $config, $name)
{
	return "$base\src\$name.Tests\bin\$config\$name.Tests.dll"
}

function Run-Tests($base, $release, $test_assemblies){
	$nunit_console = "$base\tools\NUnit.Runners.lite\nunit-console.exe"
	
	exec { & $nunit_console $test_assemblies /nologo /nodots /result="$release\TestResult.xml"  }
}

function Report-On-Test-Results($base, $release)
{
	$nunit_summary_path = "$base\tools\NUnitSummary"
	$nunit_summary = Join-Path $nunit_summary_path "nunit-summary.exe"

	$alternative_details = Join-Path $nunit_summary_path "AlternativeNUnitDetails.xsl"
	$alternative_details = "-xsl=" + $alternative_details

	exec { & $nunit_summary $release\TestResult.xml -html -o="$release\TestSummary.htm" }
	exec { & $nunit_summary $release\TestResult.xml -html -o="$release\TestDetails.htm" $alternative_details -noheader }
}