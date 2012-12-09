properties {
  $configuration = 'Release'
  $base_dir  = resolve-path .
  $release_dir = "$base_dir\release"
}

task default -depends Document

task Clean {
	Exec { msbuild "NMoneys.sln" /t:clean /p:configuration=$configuration /m }
	Remove-Item $release_dir -Recurse -Force -ErrorAction SilentlyContinue | out-null
	New-Item -ItemType directory -Path $release_dir | out-null
}

task Compile -depends Clean { 
	Exec { msbuild "NMoneys.sln" /p:configuration=$configuration /m }
}

task RunTests -depends Compile {
	$nunit_console = "$base_dir\tools\NUnit.Runners.lite\nunit-console.exe"
	$tests = test_assembly "NMoneys"
	$exchange_tests = test_assembly "NMoneys.Exchange"
	$serialization_tests = test_assembly "NMoneys.Serialization"
	$nunit_result = "$release_dir\TestResult.xml"
	exec { & $nunit_console $tests $exchange_tests $serialization_tests "/nodots" "/result=$nunit_result" }
	
	$summary_dir = "$base_dir\tools\NUnitSummary"
	$summary = "$summary_dir\nunit-summary.exe"
	exec { & $summary $nunit_result "-html" "-o=$release_dir\TestSummary.htm" }
	exec { & $summary $nunit_result "-xsl=$summary_dir\AlternativeNUnitDetails.xsl" "-o=$release_dir\TestDetails.htm " }
}

task CopyArtifacts -depends runTests {
	deploy "NMoneys" "dll"
	deploy "NMoneys.Exchange" "dll"
	deploy "NMoneys.Serialization.Json_NET" "dll"
	if ($configuration -eq 'Release') { 
		deploy "NMoneys" "XML"
		deploy "NMoneys.Exchange" "XML"
		deploy "NMoneys.Serialization.Json_NET" "XML"
	}
	elseif ($configuration -eq 'Debug') {
		deploy "NMoneys" "pdb"
		deploy "NMoneys.Exchange" "pdb"
		deploy "NMoneys.Serialization.Json_NET" "pdb"
	}
}

task Document -depends CopyArtifacts {
	if ($configuration -eq 'Release') {
		$immDocNet_dir = "$base_dir\tools\ImmDoc.NET"
		$immDocNet = "$immDocNet_dir\immDocNet.exe"
	
		build_document "NMoneys" "NMoneys"
		build_document "NMoneys.Exchange" "NMoneys.Exchange"
		build_document "NMoneys.Serialization" "NMoneys.Serialization.Json_NET"
	}
}

task ? -Description "Helper to display task info" {
	Write-Documentation
}

function global:test_assembly ($source)
{
  "$base_dir\src\$source.Tests\bin\$configuration\$source.Tests.dll"
}

function global:release_file ($source,$extension)
{
  "$base_dir\src\$source\bin\$configuration\$source.$extension"
}

function global:deploy ($source,$extension)
{
  $file = release_file $source $extension
  Copy-Item -Path $file -Destination $release_dir
}

function global:build_document ($title,$source)
{
	$immDocNet_dir = "$base_dir\tools\ImmDoc.NET"
	$immDocNet = "$immDocNet_dir\immDocNet.exe"
	$name = $title.Replace(".", "_")
	
	exec { & $immDocNet "-vl:1" "-fd" "-pn:$title" "-od:$release_dir\doc\$name" "-cn:$release_dir\doc\$name.chm" "-cp:$immDocNet_dir" "$release_dir\$source.XML" "$release_dir\$source.dll" }
}