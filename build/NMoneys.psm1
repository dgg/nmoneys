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

export-modulemember -function Throw-If-Error, Ensure-Release-Folders, Build-Documentation