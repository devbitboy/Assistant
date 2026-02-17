$root = $PSScriptRoot
$proj = Join-Path $root "Assistant.csproj"
$dst  = Join-Path $root "publish-current"

if (!(Test-Path $proj)) {
    throw "No existe el csproj: $proj"
}

Remove-Item $dst -Recurse -Force -ErrorAction SilentlyContinue
New-Item -ItemType Directory -Force -Path $dst | Out-Null

dotnet publish $proj -c Release -r win-x64 --self-contained -o $dst
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

Write-Host "Published directly to: $dst"
