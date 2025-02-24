properties {
	$configuration = 'Release'
	$verbosity = 'q'
	$nuget_apikey

	$BASE_DIR
	$RELEASE_DIR
}

taskSetup {
	$script:BASE_DIR = Resolve-Path ..
	$script:RELEASE_DIR = Join-Path $BASE_DIR release
}

task default -depends Clean, Restore, Compile, Test, Pack, CopyArtifacts

task Clean {
	exec { & dotnet clean -c $configuration -v $verbosity --nologo $BASE_DIR }
	# clear built packages
	Join-Path $BASE_DIR src NMoneys bin $configuration |
    	Get-ChildItem |
    	Where-Object { $_.Name -match '.nupkg' } |
    	Remove-Item
    # clear release folder
	Remove-Item $RELEASE_DIR -Recurse -Force -ErrorAction SilentlyContinue | Out-Null
}

task Restore {
	exec { & dotnet restore -v $verbosity $BASE_DIR }
}

task Compile {
	exec { & dotnet build --no-restore -c $configuration -v $verbosity --nologo $BASE_DIR }
}

task Test {
	$test_projects = @('NMoneys.Tests')
	foreach ($test_project in $test_projects) {
		$tests_dir = Join-Path $BASE_DIR tests $test_project
		$test_results_dir = Join-Path $RELEASE_DIR TestResults
		$runsettings_path = Join-Path $BASE_DIR tests .runsettings

		exec {
			& dotnet run --no-build -c $configuration -v $verbosity --project $tests_dir -- `
			--results-directory $test_results_dir `
			--coverage --coverage-output-format cobertura --coverage-output "$test_project.cobertura.xml" `
        	--settings $runsettings_path
		}
	}

	# TODO: generate extra HTML reports
}

task Pack {
	exec { & dotnet pack --no-build -c $configuration --nologo -v $verbosity $BASE_DIR }
}

task CopyArtifacts {
	@(
		(Join-Path $BASE_DIR src NMoneys bin $configuration)
	) |
	Get-ChildItem -Recurse |
	Where-Object { $_.Name -match '.dll|.xml|.pdb|.nupkg' } |
	Copy-Item -Destination $RELEASE_DIR
}

task Publish -depends Restore, Compile, Pack {
	@(
		(Join-Path $BASE_DIR src NMoneys bin $configuration)
	) |
	Get-ChildItem -Recurse |
	Where-Object { $_.Name -match '.nupkg' } |
	push
}

function push {
	param (
		[Parameter(ValueFromPipeline = $true)][System.IO.FileInfo]$nupkg
	)
	PROCESS {
		if ($null -eq $Env:NUGET_KEY) {
			Write-Error 'Environment variable $NUGET_KEY not set'
		}
		else {
			exec { & dotnet nuget push -s nuget.org $nupkg.FullName --api-key $Env:NUGET_KEY }
		}
	}
}
