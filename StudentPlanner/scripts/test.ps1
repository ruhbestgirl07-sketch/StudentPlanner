$ErrorActionPreference = "Stop"
$Root = (Resolve-Path (Join-Path $PSScriptRoot "../..")).Path
Set-Location $Root
dotnet restore StudentPlanner.sln
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
dotnet build StudentPlanner.sln --configuration Release
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
dotnet test StudentPlanner.sln --configuration Release --logger "console;verbosity=normal"
exit $LASTEXITCODE
