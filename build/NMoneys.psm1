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
	("$base\release\doc\", "$base\release\lib\Net40-client\", "$base\release\content\Infrastructure\Serialization", "$base\release\signed\") |
		% { New-Item -Type directory $_ -Force | Out-Null }
}

function Build-Documentation($base, $configuration)
{
	imm-doc-net $base $configuration "NMoneys"
	imm-doc-net $base $configuration "NMoneys.Exchange"
}

function imm-doc-net($base, $configuration, $project)
{
	$immDocNet_path = "$base\tools\ImmDoc.NET"
	$immDocNet = "$immDocNet_path\immDocNet.exe"
	$name = $project.Replace(".", "_")

	& $immDocNet -vl:1 -fd "-pn:$project" "-od:$base\release\doc\$name" "-cn:$base\release\doc\$name.chm" "-cp:$immDocNet_path" "$base\src\$project\bin\$configuration\$project.XML" "$base\src\$project\bin\$configuration\$project.dll"
	Throw-If-Error
}

function Copy-Artifacts($base, $configuration)
{
	copy-binaries $base $configuration
	copy-sources $base $configuration
	copy-doc $base $configuration
}

function copy-binaries($base, $configuration)
{
	$release_bin_dir = Join-Path $base release\lib\Net40-client
	
	Copy-Item $base\src\NMoneys\bin\$configuration\NMoneys.dll $release_bin_dir
	Copy-Item $base\src\NMoneys\bin\$configuration\NMoneys.XML $release_bin_dir
	Copy-Item $base\src\NMoneys.Exchange\bin\$configuration\NMoneys.Exchange.dll $release_bin_dir
	Copy-Item $base\src\NMoneys.Exchange\bin\$configuration\NMoneys.Exchange.XML $release_bin_dir
}

function copy-doc($base){
	$release_bin_dir = Join-Path $base release\lib\Net40-client
	$release_doc_dir = Join-Path $base release\doc
	
	Get-ChildItem $release_doc_dir -Filter *.chm |
		Copy-Item -Destination $release_bin_dir
}

function copy-sources()
{
	$src = Join-Path $base src\NMoneys.Serialization\
	$release_src_dir = Join-Path $base release\content\Infrastructure\Serialization
	
	Get-ChildItem -Path ("$src\Json_NET", "$src\Service_Stack", "$src\Mongo_DB", "$src\Entity_Framework") -Filter "*.cs" |
		Copy-Item -Destination $release_src_dir

	Get-ChildItem -Path "$src\Json_Net" -Filter "*.cs" |
		Get-Content |
		% {$_ -replace "Newtonsoft", "Raven.Imports.Newtonsoft"} | 
		% {$_ -replace ".Json_NET", ".Raven_DB"} |
		Set-Content "$release_src_dir\Raven_DB.cs"

	$src = Join-Path $base src\NMoneys.Serialization.Mongo_DB\
	
	Get-ChildItem -Path "$src\" -Filter "*.cs" |
		Copy-Item -Destination $release_src_dir
}

function Generate-Packages($base)
{
	$nuget = Join-Path $base tools\nuget\nuget.exe
	$release_dir = Join-Path $base release

	Get-ChildItem -File -Filter '*.nuspec' -Path $base  | 
		% { 
			& $nuget pack $_.FullName -OutputDirectory $release_dir -BasePath $release_dir /verbosity quiet
			Throw-If-Error
		}
}

function Generate-Zip-Files($base)
{
	$version = get-version-from-package $base 'NMoneys'
	('NMoneys.dll', 'NMoneys.XML', 'NMoneys.chm') |
		% { zip-bin $base $version 'NMoneys' $_ | Out-Null }
		
	zip-signed $base $version 'NMoneys' 'NMoneys.dll' | Out-Null

	$version = get-version-from-package $base 'NMoneys.Exchange'
	('NMoneys.Exchange.dll', 'NMoneys.Exchange.XML', 'NMoneys_Exchange.chm') |
		% { zip-bin $base $version 'NMoneys.Exchange' $_ | Out-Null }
}

function zip-bin($base, $version, $zipName, $fileName)
{
	$zip_file = Join-Path $base "\release\$zipName.$version-bin.zip"
	$to_add = Join-Path $base "\release\lib\Net40-client\$fileName"
	
	zip $zip_file $to_add
	
	return $zip_file
}

function zip-signed($base, $version, $zipName, $fileName)
{
	$zip_file = Join-Path $base "\release\$zipName.$version-signed.zip"
	$to_add = Join-Path $base "\release\signed\$fileName"
	
	zip $zip_file $to_add
	
	return $zip_file
}

function zip($zip_file, $to_add)
{
	& "$base\tools\Info-Zip\zip.exe" -jq $zip_file $to_add
	Throw-If-Error "Cannot add '$to_add' to '$zip_file'"
	
	return $zip_file
}

function get-version-from-package($base, $packageFragment)
{
	$pkgVersion = Get-ChildItem -File "$base\release\$packageFragment*.nupkg" |
		? { $_.Name -match "$packageFragment\.(\d(?:\.\d){3})" } |
		select -First 1 -Property @{ Name = "value"; Expression = {$matches[1]} }
		
	return $pkgVersion.value
}

function Sign-Assemblies($base, $configuration)
{
	$assembly = Join-Path $base \src\NMoneys\bin\$configuration\NMoneys.dll
	$il_file = Join-Path $base \release\signed\NMoneys.il
	$res_file = Join-Path $Base \release\signed\NMoneys.res
	$signed_assembly = Join-Path $base \release\signed\NMoneys.dll
	
	ildasm $assembly /out:$il_file
	Throw-If-Error "Could disassemble $assembly_file"
	ilasm $il_file /dll /key=$base\NMoneys.key.snk /output=$signed_assembly /quiet /res=$res_file
	Throw-If-Error "Could not assemble $il_file"
}

function Find-Versioned-Folder($base, $beginning)
{
	$dir = Get-ChildItem (Join-Path $base *) -Directory | where {$_.Name.StartsWith($beginning, [System.StringComparison]::OrdinalIgnoreCase)}
	Write-Host (Join-Path $base *)
    # get first directory
    return  $dir[0]
}

function Find-Test-Assemblies($base, $configuration)
{
	$core = get-test-assembly $base $configuration "NMoneys"
	$exchange = get-test-assembly $base $configuration "NMoneys.Exchange"
	$serialization = get-test-assembly $base $configuration "NMoneys.Serialization"
	$mongo_db = get-test-assembly $base $configuration "NMoneys.Serialization.Mongo_DB"

	return ($core, $exchange, $serialization, $mongo_db)
}

function get-test-assembly($base, $config, $name)
{
	return "$base\src\$name.Tests\bin\$config\$name.Tests.dll"
}

export-modulemember -function Throw-If-Error, Ensure-Release-Folders, Build-Documentation, Copy-Artifacts, Generate-Packages, Generate-Zip-Files, Sign-Assemblies, Find-Versioned-Folder, Find-Test-Assemblies