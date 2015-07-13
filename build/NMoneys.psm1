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

function Ensure-Release($base)
{
	("$base\release\doc\", "$base\release\lib\Net40-client\", "$base\release\content\Infrastructure\Serialization") |
		% { New-Item -Type directory $_ -Force | Out-Null }
}

export-modulemember -function Throw-If-Error, Ensure-Release