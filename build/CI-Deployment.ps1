function push-package-artifact($packageFragment, $artifactName)
{
	$pkg = Get-ChildItem -File ".\release\$packageFragment*.nupkg" |
		? { $_.Name -match "$packageFragment\.(\d(?:\.\d){3})" }
	Push-AppveyorArtifact $pkg -DeploymentName $artifactName
}

function push-zip-artifact($zipFragment, $artifactName, $zipType)
{
	$zip = Get-ChildItem -File ".\release\$zipFragment*-$zipType.zip" |
		? { $_.Name -match "$zipFragment\.(\d(?:\.\d){3})" }
	Push-AppveyorArtifact $zip -DeploymentName $artifactName
}

function push-coverage($base)
{
	$token = Get-ChildItem Env:CODECOV_TOKEN
	$tools_dir = Join-Path $base tools
	$codecov = Join-Path (Resolve-Path $tools_dir) codecov.sh
	$coverage_result = Join-Path $base release\CoverageResult.xml
	
	& "$codecov" -f $coverage_result -t $token -X gcov
	Throw-If-Error "Could not upload coverage"
}

push-package-artifact 'NMoneys' 'nmoneys'

push-package-artifact 'NMoneys.Exchange' 'nmoneys_exchange'

push-package-artifact 'NMoneys.Serialization.Json_Net' 'nmoneys_serialization_json_net'

push-package-artifact 'NMoneys.Serialization.Service_Stack' 'nmoneys_serialization_service_stack'

push-package-artifact 'NMoneys.Serialization.Raven_DB' 'nmoneys_serialization_raven_db'

push-package-artifact 'NMoneys.Serialization.Mongo_DB.mongocsharpdriver' 'nmoneys_serialization_mongo_db_legacy'

push-package-artifact 'NMoneys.Serialization.Mongo_DB' 'nmoneys_serialization_mongo_db'

push-package-artifact 'NMoneys.Serialization.Entity_Framework' 'nmoneys_serialization_entity_framework'

push-zip-artifact 'NMoneys' 'nmoneys_zip' 'bin'

push-zip-artifact 'NMoneys' 'nmoneys_signed_zip' 'signed'

push-zip-artifact 'NMoneys.Exchange' 'nmoneys_exchange_zip' 'bin'

push-coverage .