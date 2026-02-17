Set-Location "C:\dev\Assistant"

$dst  = "C:\dev\Assistant\publish-current"
$proj = "C:\dev\Assistant\Assistant.csproj"  # AJUSTA

if (Test-Path $dst) { Remove-Item "$dst\*" -Recurse -Force }
New-Item -ItemType Directory -Force -Path $dst | Out-Null

dotnet publish $proj -c Release -r win-x64 --self-contained true -o $dst

Write-Host "Published directly to: $dst"
