$BASE_DIR = Resolve-Path .. -RelativeBasePath $PSScriptRoot
$RELEASE_DIR = Join-Path $BASE_DIR release

$artifacts_dir = Join-Path $RELEASE_DIR artifacts

$workflow = Join-Path $BASE_DIR .github workflows build.yml

& act `
	-W $workflow `
	-P ubuntu-22.04=catthehacker/ubuntu:pwsh-22.04 `
	--artifact-server-path "$artifacts_dir"
