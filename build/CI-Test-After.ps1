$configuration = Get-ChildItem Env:CONFIGURATION

Generate-Coverage . $configuration

Generate-Packages .

Generate-Zip-Files .