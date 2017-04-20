function generate-coverage($base, $configuration)
{
	$runner_dir = Find-Versioned-Folder -base $base\tools -beginning 'NUnit.ConsoleRunner'
	$nunit_console = Join-Path $runner_dir tools\nunit3-console.exe
	
	$test_assemblies = Find-Test-Assemblies $base $configuration
	$test_result = Join-Path $base release\TestResult.xml
	$test_args = $test_assemblies -join " "
	$test_args = $test_args + " --result:$test_result --noheader"
	
	$coverage_dir = Find-Versioned-Folder -base $base\tools -beginning 'OpenCover'
	$opencover = Join-Path $coverage_dir tools\OpenCover.Console.exe
	$coverage_result = Join-Path $base release\CoverageResult.xml
	
	& "$opencover" -target:$nunit_console -targetargs:"$test_args" -filter:"+[*]* -[*.Tests*]* -[*]*.*Config" -mergebyhash -skipautoprops -register:path64 -output:"$coverage_result"
}
generate-coverage . $env:CONFIGURATION

Run-Core-Tests . $env:CONFIGURATION

Generate-Packages .

Generate-Zip-Files .