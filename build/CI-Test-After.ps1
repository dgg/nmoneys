$configuration = Get-ChildItem Env:CONFIGURATION

Generate-Coverage . $configuration.Value

Generate-Packages .

Generate-Zip-Files .