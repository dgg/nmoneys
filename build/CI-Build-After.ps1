$configuration = Get-ChildItem Env:CONFIGURATION

Build-Documentation . $configuration.Value
