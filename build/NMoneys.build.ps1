properties {
  $configuration = 'Release'
  $base_dir  = resolve-path ..
  $release_dir = "$base_dir\release"
}

task default -depends Clean, Compile, RunTests, CopyArtifacts, Document, Pack

task Clean {
	Exec { msbuild "$base_dir\NMoneys.sln" /t:clean /p:configuration=$configuration /m }
	Remove-Item $release_dir -Recurse -Force -ErrorAction SilentlyContinue | out-null
}

task Compile { 
	Exec { msbuild "$base_dir\NMoneys.sln" /p:configuration=$configuration /m }
}

task RunTests {
	Ensure-Release-Folders $release_dir
	
	$core = Test-Assembly $base_dir $configuration "NMoneys"
	$exchange = Test-Assembly $base_dir $configuration "NMoneys.Exchange"
	$serialization = Test-Assembly $base_dir $configuration "NMoneys.Serialization"

	#Run-Tests $base_dir $release_dir ($core, $exchange, $serialization)
	#Report-On-Test-Results $base_dir $release_dir
}

task CopyArtifacts {
	$release_folders = Ensure-Release-Folders $release_dir

	$core = Bin-Folder $base_dir $configuration "NMoneys"
	$exchange = Bin-Folder $base_dir $configuration "NMoneys.Exchange"
	$serialization = Src-Folder $base_dir "NMoneys.Serialization"

	$except_content = $release_folders.Length-2
	$bin_release_folder = $release_folders[0..$except_content]
	$src_release_folder = $release_folders[$except_content+1]
	
	Get-ChildItem -Path ($core, $exchange) -Filter 'NMoneys*.dll' |
		Copy-To $bin_release_folder

	Get-ChildItem $base_dir -Filter '*.nuspec' |
		Copy-Item -Destination $release_dir

	Get-ChildItem -Path ("$serialization\Json_NET", "$serialization\Service_Stack") -Filter "*.cs" |
		Copy-Item -Destination $src_release_folder

	Get-ChildItem -Path "$serialization\Json_Net" -Filter "*.cs" |
		Get-Content |
		Foreach-Object {$_ -replace "Newtonsoft", "Raven.Imports.Newtonsoft"} | 
		Foreach-Object {$_ -replace ".Json_NET", ".Raven_DB"} |
		Set-Content "$src_release_folder\Raven_DB.cs"

	if ($configuration -eq 'Release') {
		Get-ChildItem -Path ($core, $exchange) -Filter 'NMoneys*.xml' |
			Copy-To $bin_release_folder
	}
	elseif ($configuration -eq 'Debug') {
		Get-ChildItem -Path ($core, $exchange) -Filter 'NMoneys*.pdb' |
			Copy-To $bin_release_folder
	}
}

task Document {
	if ($configuration -eq 'Release') {
		$release_folders = Ensure-Release-Folders $release_dir

		$doc_path = Generate-Documentation $base_dir $release_dir "NMoneys" "NMoneys"
		$doc_path = Generate-Documentation $base_dir $release_dir "NMoneys.Exchange" "NMoneys.Exchange"

		Get-ChildItem -Path ($doc_path) -Filter '*.chm' |
			Copy-To $release_folders
	}
}

task Pack {
	Ensure-Release-Folders $release_dir

	$nuget = "$base_dir\tools\nuget\nuget.exe"

	Get-ChildItem -File -Filter '*.nuspec' -Path $release_dir  | 
		ForEach-Object { exec { & $nuget pack $_.FullName /o $release_dir } }
}

task Sign -depends Clean, Compile, CopyArtifacts {

	$signed_dir = "$release_dir\signed"
	md $signed_dir -Force | Out-Null

	Exec { ildasm $release_dir\NMoneys.dll /out:$release_dir\NMoneys.il }
	Exec { ilasm $release_dir\NMoneys.il /dll /key=$base_dir\NMoneys.key.snk /output=$signed_dir\NMoneys.dll } | Out-Null
}

task ? -Description "Helper to display task info" {
	Write-Documentation
}

function Ensure-Release-Folders($base)
{
	$release_folders = ($base, "$base\lib\Net40-client", "$base\content\Infrastructure\Serialization")

	foreach ($f in $release_folders) { md $f -Force | Out-Null }

	return $release_folders
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

function Bin-Folder($base, $config, $name)
{
	$project = Src-Folder $base $name
	return Join-Path $project "bin\$config\"
}

function Src-Folder($base, $name)
{
	return "$base\src\$name\"
}

function Copy-To($destinations)
{
	Process { foreach ($d in $destinations) { Copy-Item -Path $_.FullName -Destination $d } }
}

function Generate-Documentation ($base, $release, $title, $source)
{
	$immDocNet_path = "$base\tools\ImmDoc.NET"
	$immDocNet = "$immDocNet_path\immDocNet.exe"
	$name = $title.Replace(".", "_")
	
	exec { & $immDocNet "-vl:1" "-fd" "-pn:$title" "-od:$release\doc\$name" "-cn:$release\doc\$name.chm" "-cp:$immDocNet_path" "$release\$source.XML" "$release\$source.dll" }

	return "$release\doc\"
}