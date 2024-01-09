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
		$trx = Join-Path TestResults "$test_project.trx"
		$html = Join-Path TestResults "$test_project.html"

		exec {
			& dotnet test --no-build -c $configuration --nologo -v $verbosity $tests_dir `
				--results-directory $RELEASE_DIR `
				--collect:"XPlat Code Coverage" `
				-l:"console;verbosity=minimal;NoSummary=true" `
				-l:"trx;LogFileName=$trx" `
				-l:"html;LogFileName=$html" `
				-- NUnit.TestOutputXml=TestResults NUnit.OutputXmlFolderMode=RelativeToResultDirectory
		}
	}

	$test_results_dir = Join-Path $RELEASE_DIR TestResults
	# copy coverage file to $RELEASE_DIR/TestResults
	$RELEASE_DIR |
	Get-ChildItem -exclude TestResults |
	Get-ChildItem -Recurse |
	Where-Object { $_.Name -match 'coverage.cobertura.xml' } |
	Copy-Item -Destination $test_results_dir

	# clean coverage directories
	$RELEASE_DIR |
	Get-ChildItem -Directory -Exclude 'TestResults' |
	Remove-Item -Recurse

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
