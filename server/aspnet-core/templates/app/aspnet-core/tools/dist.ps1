$excPath = Split-Path -Parent $MyInvocation.MyCommand.Definition
$solutionAbsPath = (Join-Path $excPath "../")

dotnet publish $solutionAbsPath -c Release