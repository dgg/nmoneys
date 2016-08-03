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
	$coveralls_dir = Find-Versioned-Folder -base $base\tools -beginning 'coveralls'
	$coveralls = Join-Path $coveralls_dir tools\csmacnz.Coveralls.exe
	
	$token = Get-ChildItem Env:COVERALLS_TOKEN
	$coverage_result = Join-Path $base release\CoverageResult.xml
	
	& $coveralls --opencover -i $coverage_result --repoToken $env:COVERALLS_TOKEN --useRelativePaths --serviceName appveyor --commitId $env:APPVEYOR_REPO_COMMIT --commitBranch $env:APPVEYOR_REPO_BRANCH --commitAuthor $env:APPVEYOR_REPO_COMMIT_AUTHOR --commitEmail $env:APPVEYOR_REPO_COMMIT_AUTHOR_EMAIL --commitMessage $env:APPVEYOR_REPO_COMMIT_MESSAGE --jobId $env:APPVEYOR_BUILD_NUMBER
	Throw-If-Error "Could not upload coverage"
	
	$env:Path += ";C:\\Python34;C:\\Python34\\Scripts;"
	pip install codecov
	& "codecov" -f $coverage_result -token "4a14c6dd-42bd-45d4-88db-f804e53a4082"
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