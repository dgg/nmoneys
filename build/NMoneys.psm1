function Throw-If-Error
{
	[CmdletBinding()]
	param(
		[Parameter(Position=0,Mandatory=0)][string]$errorMessage = ('Error executing command {0}' -f $cmd)
	)
	if ($global:lastexitcode -ne 0) {
		throw ("Exec: " + $errorMessage)
	}
}

function Ensure-Release-Folders($base)
{
	("$base\release\doc\", "$base\release\lib\Net40-client\", "$base\release\content\Infrastructure\Serialization") |
		% { New-Item -Type directory $_ -Force | Out-Null }
}

function Build-Documentation($base, $configuration)
{
	ImmDocNet $base $configuration "NMoneys"
	ImmDocNet $base $configuration "NMoneys.Exchange"
}

function ImmDocNet($base, $configuration, $project)
{
	$immDocNet_path = "$base\tools\ImmDoc.NET"
	$immDocNet = "$immDocNet_path\immDocNet.exe"
	$name = $project.Replace(".", "_")

	& $immDocNet -vl:1 -fd "-pn:$project" "-od:$base\release\doc\$name" "-cn:$base\release\doc\$name.chm" "-cp:$immDocNet_path" "$base\src\$project\bin\$configuration\$project.XML" "$base\src\$project\bin\$configuration\$project.dll"
	Throw-If-Error
}

function Copy-Artifacts($base, $configuration)
{
	CopyBinaries $base $configuration
	CopySources $base $configuration
	CopyDoc $base $configuration
	CopyPackageManifests $base
}

function CopyBinaries($base, $configuration)
{
	$release_bin_dir = Join-Path $base release\lib\Net40-client
	
	Copy-Item $base\src\NMoneys\bin\$configuration\NMoneys.dll $release_bin_dir
	Copy-Item $base\src\NMoneys\bin\$configuration\NMoneys.XML $release_bin_dir
	Copy-Item $base\src\NMoneys.Exchange\bin\$configuration\NMoneys.Exchange.dll $release_bin_dir
	Copy-Item $base\src\NMoneys.Exchange\bin\$configuration\NMoneys.Exchange.XML $release_bin_dir
}

function CopyDoc($base){
	$release_bin_dir = Join-Path $base release\lib\Net40-client
	$release_doc_dir = Join-Path $base release\doc
	
	Get-ChildItem $release_doc_dir -Filter *.chm |
		Copy-Item -Destination $release_bin_dir
}

function CopyPackageManifests($base){
	$release_dir = Join-Path $base release
	
	Get-ChildItem $base -Filter '*.nuspec' |
		Copy-Item -Destination $release_dir
}

function CopySources()
{
	$src = Join-Path $base src\NMoneys.Serialization\
	$release_src_dir = Join-Path $base release\content\Infrastructure\Serialization
	
	Get-ChildItem -Path ("$src\Json_NET", "$src\Service_Stack") -Filter "*.cs" |
		Copy-Item -Destination $release_src_dir

	Get-ChildItem -Path "$src\Json_Net" -Filter "*.cs" |
		Get-Content |
		% {$_ -replace "Newtonsoft", "Raven.Imports.Newtonsoft"} | 
		% {$_ -replace ".Json_NET", ".Raven_DB"} |
		Set-Content "$release_src_dir\Raven_DB.cs"
}

function Generate-Packages($base)
{
	$nuget = Join-Path $base tools\nuget\nuget.exe
	$release_dir = Join-Path $base release

	Get-ChildItem -File -Filter '*.nuspec' -Path $release_dir  | 
		% { 
			& $nuget pack $_.FullName /o $release_dir
			Throw-If-Error
		}
}

export-modulemember -function Throw-If-Error, Ensure-Release-Folders, Build-Documentation, Copy-Artifacts, Generate-Packages