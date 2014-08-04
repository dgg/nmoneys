param($installPath, $toolsPath, $package, $project)

$projectPath = ([System.IO.Directory]::GetParent($project.FullName))
$codePath = Join-Path -Path $projectPath -ChildPath "Infrastructure" | Join-Path -ChildPath "Serialization" | Join-Path -ChildPath "Json_NET.cs"

(Get-Content $codePath) | 
Foreach-Object {$_ -replace "Newtonsoft", "Raven.Imports.Newtonsoft"} | 
Set-Content $codePath