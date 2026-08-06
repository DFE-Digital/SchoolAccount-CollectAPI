#!/usr/bin/env bash
# Runs all tests with code coverage and generates an HTML report.
# Usage: ./coverage.sh [--open]
set -euo pipefail
cd "$(dirname "$0")"

rm -rf TestResults

dotnet tool restore
dotnet test -- --coverage --coverage-output-format cobertura --coverage-settings "$PWD/coverage.config"
dotnet reportgenerator \
  -reports:"TestResults/*.cobertura.xml" \
  -targetdir:TestResults/CoverageReport \
  -reporttypes:Html

echo "Report: TestResults/CoverageReport/index.html"
if [[ "${1:-}" == "--open" ]]; then
  openCmd=$([ -n "${WINDIR:-}" ] && echo "start" || echo "open")
  $openCmd TestResults/CoverageReport/index.html
fi
