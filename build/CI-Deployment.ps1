function PushArtifact($packageFragment, $artifactName)
{
	$pkg = Get-ChildItem -File ".\release\$packageFragment*.nupkg" | Where-Object {$_.Name -match "$packageFragment.\d.\d.\d.\d"}
	Push-AppveyorArtifact $pkg -DeploymentName $artifactName
}

PushArtifact 'NMoneys' 'nmoneys'

PushArtifact 'NMoneys.Exchange' 'nmoneys_exchange'

PushArtifact 'NMoneys.Serialization.Json_Net' 'nmoneys_serialization_json_net'

PushArtifact 'NMoneys.Serialization.Service_Stack' 'nmoneys_serialization_service_stack'

PushArtifact 'NMoneys.Serialization.Raven_DB' 'nmoneys_serialization_raven_db'