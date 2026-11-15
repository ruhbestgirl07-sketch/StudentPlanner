#!/usr/bin/env bash
set -euo pipefail
ROOT="$(cd "$(dirname "$0")/../.." && pwd)"
cd "$ROOT"
dotnet restore StudentPlanner.sln
dotnet build StudentPlanner.sln --configuration Release
dotnet test StudentPlanner.sln --configuration Release --logger "console;verbosity=normal"
