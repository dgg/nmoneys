$configuration = Get-ChildItem Env:CONFIGURATION

Copy-Artifacts . $configuration.Value