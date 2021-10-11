$excPath = Split-Path -Parent $MyInvocation.MyCommand.Definition

$path = $excPath + "/common.json"

$common = (Get-Content $Path -Raw) | ConvertFrom-Json 

$versionArray =  $common.version.Split(".")
$fixVersion = [int]($versionArray[1])+1
$versionArray[1] = [string]$fixVersion

$common.version = $versionArray[0] + "." + $versionArray[1] + "." + "0"
$common.version
$common.upgradeTime = Get-Date -Format 'yyyy-MM-dd HH:mm:ss'
$common | ConvertTo-Json -depth 32| Set-Content -Path $path