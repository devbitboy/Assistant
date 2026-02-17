$ErrorActionPreference = "Stop"

$root   = $PSScriptRoot
$proj   = Join-Path $root "Assistant.csproj"
$build  = Join-Path $root "publish-current"
$live   = Join-Path $root "app-current"
$exe    = Join-Path $live "Assistant.exe"

# 1) Publish release a publish-current
& (Join-Path $root "update-published.ps1")
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

# 2) Stop running Assistant (if any)
Get-Process Assistant -ErrorAction SilentlyContinue | Stop-Process -Force
Start-Sleep -Milliseconds 500

# 3) Swap build -> live (robocopy espejo)
New-Item -ItemType Directory -Force -Path $live | Out-Null

# /MIR = mirror, /R:2 /W:1 retries, /NFL /NDL reduce noise
robocopy $build $live /MIR /R:2 /W:1 /NFL /NDL | Out-Null
$rc = $LASTEXITCODE

# Robocopy codes: 0-7 are "ok-ish", >=8 error
if ($rc -ge 8) { throw "Robocopy failed with code $rc" }

# 4) Start Assistant from live
Start-Process -FilePath $exe -WorkingDirectory $live
