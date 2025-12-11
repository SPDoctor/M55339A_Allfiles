# Quick Upgrade Script - Run from any directory to upgrade projects in current directory tree
# This will upgrade all .NET 8 projects to .NET 10

$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$rootPath = Get-Location

Write-Host "=== Quick .NET 10 Upgrade ===" -ForegroundColor Cyan
Write-Host "Scanning from: $rootPath" -ForegroundColor Yellow
Write-Host ""

# Find all .csproj files in current directory and subdirectories
$projects = Get-ChildItem -Path $rootPath -Recurse -Filter *.csproj

foreach ($proj in $projects) {
    $content = Get-Content $proj.FullName -Raw
    $changed = $false
    
    # Check if it's a .NET 8 project
    if ($content -match 'net8\.0') {
        Write-Host "Upgrading: $($proj.Name)" -ForegroundColor Green
        
        # Update target framework
        $content = $content -replace '<TargetFramework>net8\.0', '<TargetFramework>net10.0'
        $content = $content -replace 'net8\.0-windows', 'net10.0-windows'
        $content = $content -replace 'net8\.0-android', 'net10.0-android'
        $content = $content -replace 'net8\.0-ios', 'net10.0-ios'
        $content = $content -replace 'net8\.0-maccatalyst', 'net10.0-maccatalyst'
        
        # Update common Microsoft packages to version 10.0.0
        $content = $content -replace '(Microsoft\.EntityFrameworkCore[^"]*" Version=")8\.[0-9\.]+', '$110.0.0'
        $content = $content -replace '(Microsoft\.AspNetCore[^"]*" Version=")8\.[0-9\.]+', '$110.0.0'
        $content = $content -replace '(Microsoft\.Extensions[^"]*" Version=")8\.[0-9\.]+', '$110.0.0'
        
        Set-Content $proj.FullName -Value $content -NoNewline
        Write-Host "  ? Updated $($proj.FullName)" -ForegroundColor Green
    }
}

Write-Host ""
Write-Host "Upgrade complete! Run 'dotnet restore' and 'dotnet build' to verify." -ForegroundColor Cyan
