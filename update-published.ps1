Set-Location "C:\dev\Assistant"

dotnet publish -c Release -r win-x64 --self-contained true

$dst = "C:\dev\Assistant\publish-current\"

New-Item -ItemType Directory -Force -Path $dst | Out-Null

# Copia el publish más reciente de forma determinística
$latestPublishDir = Get-ChildItem "C:\dev\Assistant\bin\Release" -Recurse -Directory -Filter "publish" |
  Sort-Object LastWriteTime -Descending |
  Select-Object -First 1

if (-not $latestPublishDir) {
    throw "No se encontró carpeta publish."
}

robocopy $latestPublishDir.FullName $dst /MIR | Out-Null

Write-Host "Updated publish-current from: $($latestPublishDir.FullName)"
