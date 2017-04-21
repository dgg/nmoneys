function push-package-artifact($packageFragment, $artifactName)
{
	$pkg = Get-ChildItem -File ".\release\$packageFragment*.nupkg" |
		? { $_.Name -match "$packageFragment\.(\d(?:\.\d){2})" }
	Push-AppveyorArtifact $pkg -DeploymentName $artifactName
}

function push-zip-artifact($zipFragment, $artifactName, $zipType)
{
	$zip = Get-ChildItem -File ".\release\$zipFragment*-$zipType.zip" |
		? { $_.Name -match "$zipFragment\.(\d(?:\.\d){2})" }
	Push-AppveyorArtifact $zip -DeploymentName $artifactName
}

function push-coverage($base)
{
	$coveralls_dir = Find-Versioned-Folder -base $base\tools -beginning 'coveralls'
	$coveralls = Join-Path $coveralls_dir tools\csmacnz.Coveralls.exe
	
	$coverage_result = Join-Path $base release\CoverageResult.xml
	
	& $coveralls --opencover -i $coverage_result --repoToken $env:COVERALLS_TOKEN --useRelativePaths --serviceName appveyor --commitId $env:APPVEYOR_REPO_COMMIT --commitBranch $env:APPVEYOR_REPO_BRANCH --commitAuthor $env:APPVEYOR_REPO_COMMIT_AUTHOR --commitEmail $env:APPVEYOR_REPO_COMMIT_AUTHOR_EMAIL --commitMessage $env:APPVEYOR_REPO_COMMIT_MESSAGE --jobId $env:APPVEYOR_BUILD_NUMBER
	Throw-If-Error "Could not upload coverage to Coveralls.io"
	
	$env:Path = "C:\\Python34;C:\\Python34\\Scripts;" + $env:Path
	pip install codecov
	& "codecov" --file $coverage_result --token "$env:CODECOV_TOKEN" -X gcov
	Throw-If-Error "Could not upload coverage to Codecov.io"
}

push-package-artifact 'NMoneys' 'nmoneys'

push-package-artifact 'NMoneys.Exchange' 'nmoneys_exchange'

push-package-artifact 'NMoneys.Serialization.Json_Net' 'nmoneys_serialization_json_net'

push-package-artifact 'NMoneys.Serialization.Service_Stack' 'nmoneys_serialization_service_stack'

push-package-artifact 'NMoneys.Serialization.Raven_DB' 'nmoneys_serialization_raven_db'

push-package-artifact 'NMoneys.Serialization.Mongo_DB.mongocsharpdriver' 'nmoneys_serialization_mongo_db_legacy'

push-package-artifact 'NMoneys.Serialization.Mongo_DB' 'nmoneys_serialization_mongo_db'

push-package-artifact 'NMoneys.Serialization.Entity_Framework' 'nmoneys_serialization_entity_framework'

push-zip-artifact 'NMoneys' 'nmoneys_net_zip' 'bin_net'
push-zip-artifact 'NMoneys' 'nmoneys_netstandard_zip' 'bin_netstandard'

push-zip-artifact 'NMoneys' 'nmoneys_signed_net_zip' 'signed_net'
push-zip-artifact 'NMoneys' 'nmoneys_signed_netstandard_zip' 'signed_netstandard'

push-zip-artifact 'NMoneys.Exchange' 'nmoneys_exchange_net_zip' 'bin_net'
push-zip-artifact 'NMoneys.Exchange' 'nmoneys_exchange_netstandard_zip' 'bin_netstandard'

push-coverage .