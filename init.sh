#!/usr/bin/env bash
# One-time developer setup: restores dotnet tools and installs the Husky.NET git hooks.
# Usage: ./init.sh
set -euo pipefail
cd "$(dirname "$0")"

dotnet tool restore
dotnet husky install

echo "Done: dotnet tools restored and git hooks installed via Husky.NET"
