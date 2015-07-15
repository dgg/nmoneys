function PushPackageArtifact($packageFragment, $artifactName)
{
	$pkg = Get-ChildItem -File ".\release\$packageFragment*.nupkg" |
		? { $_.Name -match "$packageFragment\.(\d(?:\.\d){3})" }
	Push-AppveyorArtifact $pkg -DeploymentName $artifactName
}

function PushZipArtifact($zipFragment, $artifactName, $zipType)
{
	$zip = Get-ChildItem -File ".\release\$zipFragment*-$zipType.zip" |
		? { $_.Name -match "$zipFragment\.(\d(?:\.\d){3})" }
	Push-AppveyorArtifact $zip -DeploymentName $artifactName
}

PushPackageArtifact 'NMoneys' 'nmoneys'

PushPackageArtifact 'NMoneys.Exchange' 'nmoneys_exchange'

PushPackageArtifact 'NMoneys.Serialization.Json_Net' 'nmoneys_serialization_json_net'

PushPackageArtifact 'NMoneys.Serialization.Service_Stack' 'nmoneys_serialization_service_stack'

PushPackageArtifact 'NMoneys.Serialization.Raven_DB' 'nmoneys_serialization_raven_db'

PushZipArtifact 'NMoneys' 'nmoneys_zip' 'bin'

PushZipArtifact 'NMoneys' 'nmoneys_signed_zip' 'signed'

PushZipArtifact 'NMoneys.Exchange' 'nmoneys_exchange_zip' 'bin'