param(
    [Parameter(Mandatory=$true)]
    [string]$Version
)

Write-Host "Building NativeChat..." -ForegroundColor Cyan
Push-Location .\NativeChatClient
if (-not (Test-Path .\node_modules)) {
    Write-Host "Restoring NativeChat npm dependencies..." -ForegroundColor Cyan
    npm.cmd ci
    if ($LASTEXITCODE -ne 0) { Write-Host "npm ci failed." -ForegroundColor Red; exit 1 }
}
npm.cmd run build
if ($LASTEXITCODE -ne 0) { Write-Host "npm build failed." -ForegroundColor Red; exit 1 }
Pop-Location

Write-Host "Publishing .NET app..." -ForegroundColor Cyan
dotnet publish TransparentTwitchChatWPF\TransparentTwitchChatWPF.csproj -c Release --output .\publish
if ($LASTEXITCODE -ne 0) { Write-Host "dotnet publish failed." -ForegroundColor Red; exit 1 }

Write-Host "Packaging with vpk..." -ForegroundColor Cyan
$env:DOTNET_ROLL_FORWARD = "Major"
vpk pack --packId TransparentTwitchChat --packVersion $Version --packDir .\publish --mainExe TransparentTwitchChatWPF.exe --packTitle "Transparent-Twitch-ChatKR-Overlay" --packAuthors "baffler, moonkyu12"
if ($LASTEXITCODE -ne 0) { Write-Host "vpk packaging failed." -ForegroundColor Red; exit 1 }

Write-Host "Done! Published version $Version" -ForegroundColor Green
