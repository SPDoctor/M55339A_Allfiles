# PowerShell Script to Upgrade All .NET Projects to .NET 10
# This script updates TargetFramework and common NuGet packages

param(
    [Parameter(Mandatory=$false)]
    [string]$RootPath = (Get-Location).Path,
    
    [Parameter(Mandatory=$false)]
    [switch]$WhatIf = $false
)

Write-Host "=== .NET 10 Upgrade Script ===" -ForegroundColor Cyan
Write-Host "Root Path: $RootPath" -ForegroundColor Yellow
if ($WhatIf) {
    Write-Host "Running in WhatIf mode - no changes will be made" -ForegroundColor Yellow
}
Write-Host ""

# Find all .csproj files
$projectFiles = Get-ChildItem -Path $RootPath -Recurse -Filter *.csproj -ErrorAction SilentlyContinue

Write-Host "Found $($projectFiles.Count) project files" -ForegroundColor Green
Write-Host ""

$updatedCount = 0
$skippedCount = 0
$errorCount = 0

foreach ($projectFile in $projectFiles) {
    Write-Host "Processing: $($projectFile.FullName)" -ForegroundColor Cyan
    
    try {
        # Read the project file
        $content = Get-Content -Path $projectFile.FullName -Raw
        $originalContent = $content
        $modified = $false
        
        # Update TargetFramework from net8.0 to net10.0
        if ($content -match '<TargetFramework>net8\.0') {
            $content = $content -replace '<TargetFramework>net8\.0([^<]*)</TargetFramework>', '<TargetFramework>net10.0$1</TargetFramework>'
            Write-Host "  + Updated TargetFramework to net10.0" -ForegroundColor Green
            $modified = $true
        }
        
        # Update TargetFrameworks (plural) from net8.0 to net10.0
        if ($content -match '<TargetFrameworks>.*net8\.0') {
            $content = $content -replace 'net8\.0', 'net10.0'
            Write-Host "  + Updated TargetFrameworks to include net10.0" -ForegroundColor Green
            $modified = $true
        }
        
        # Update Entity Framework Core packages
        if ($content -match 'Microsoft\.EntityFrameworkCore.*Version="8\.'  ) {
            $content = $content -replace '(Microsoft\.EntityFrameworkCore[^"]*" Version=")8\.[0-9\.]+', '${1}10.0.0'
            Write-Host "  + Updated Entity Framework Core to 10.0.0" -ForegroundColor Green
            $modified = $true
        }
        
        # Update ASP.NET Core packages
        if ($content -match 'Microsoft\.AspNetCore[^>]*Version="8\.'  ) {
            $content = $content -replace '(Microsoft\.AspNetCore[^"]*" Version=")8\.[0-9\.]+', '${1}10.0.0'
            Write-Host "  + Updated ASP.NET Core packages to 10.0.0" -ForegroundColor Green
            $modified = $true
        }
        
        # Update Microsoft.Extensions packages
        if ($content -match 'Microsoft\.Extensions[^>]*Version="8\.'  ) {
            $content = $content -replace '(Microsoft\.Extensions[^"]*" Version=")8\.[0-9\.]+', '${1}10.0.0'
            Write-Host "  + Updated Microsoft.Extensions packages to 10.0.0" -ForegroundColor Green
            $modified = $true
        }
        
        if ($modified) {
            if (-not $WhatIf) {
                # Write the updated content back to the file
                Set-Content -Path $projectFile.FullName -Value $content -NoNewline
                Write-Host "  + File updated successfully" -ForegroundColor Green
            } else {
                Write-Host "  [WhatIf] Would update this file" -ForegroundColor Yellow
            }
            $updatedCount++
        } else {
            Write-Host "  - No changes needed (already net10.0 or different target)" -ForegroundColor Gray
            $skippedCount++
        }
        
    } catch {
        Write-Host "  x Error: $_" -ForegroundColor Red
        $errorCount++
    }
    
    Write-Host ""
}

Write-Host "=== Summary ===" -ForegroundColor Cyan
Write-Host "Updated: $updatedCount projects" -ForegroundColor Green
Write-Host "Skipped: $skippedCount projects" -ForegroundColor Gray
Write-Host "Errors:  $errorCount projects" -ForegroundColor $(if ($errorCount -gt 0) { "Red" } else { "Gray" })

if (-not $WhatIf -and $updatedCount -gt 0) {
    Write-Host ""
    Write-Host "=== Running dotnet restore ===" -ForegroundColor Cyan
    
    # Find all .sln files
    $solutionFiles = Get-ChildItem -Path $RootPath -Recurse -Filter *.sln -ErrorAction SilentlyContinue
    
    if ($solutionFiles.Count -gt 0) {
        Write-Host "Found $($solutionFiles.Count) solution files" -ForegroundColor Green
        foreach ($sln in $solutionFiles) {
            Write-Host "Restoring: $($sln.FullName)" -ForegroundColor Cyan
            Push-Location (Split-Path $sln.FullName)
            try {
                dotnet restore
                if ($LASTEXITCODE -eq 0) {
                    Write-Host "  + Restore completed successfully" -ForegroundColor Green
                } else {
                    Write-Host "  x Restore failed" -ForegroundColor Red
                }
            } catch {
                Write-Host "  x Error: $_" -ForegroundColor Red
            } finally {
                Pop-Location
            }
            Write-Host ""
        }
    } else {
        Write-Host "No solution files found. Restoring individual projects..." -ForegroundColor Yellow
        foreach ($projectFile in $projectFiles) {
            Write-Host "Restoring: $($projectFile.FullName)" -ForegroundColor Cyan
            Push-Location (Split-Path $projectFile.FullName)
            try {
                dotnet restore
                if ($LASTEXITCODE -eq 0) {
                    Write-Host "  + Restore completed successfully" -ForegroundColor Green
                } else {
                    Write-Host "  x Restore failed" -ForegroundColor Red
                }
            } catch {
                Write-Host "  x Error: $_" -ForegroundColor Red
            } finally {
                Pop-Location
            }
            Write-Host ""
        }
    }
    
    Write-Host ""
    Write-Host "=== Building solutions ===" -ForegroundColor Cyan
    
    $buildSuccessCount = 0
    $buildFailCount = 0
    
    if ($solutionFiles.Count -gt 0) {
        Write-Host "Building $($solutionFiles.Count) solution files" -ForegroundColor Green
        foreach ($sln in $solutionFiles) {
            Write-Host "Building: $($sln.FullName)" -ForegroundColor Cyan
            Push-Location (Split-Path $sln.FullName)
            try {
                $buildOutput = dotnet build 2>&1
                if ($LASTEXITCODE -eq 0) {
                    Write-Host "  + Build completed successfully" -ForegroundColor Green
                    $buildSuccessCount++
                } else {
                    Write-Host "  x Build failed" -ForegroundColor Red
                    Write-Host "Build errors:" -ForegroundColor Red
                    $buildOutput | Where-Object { $_ -match 'error' } | ForEach-Object {
                        Write-Host "    $_" -ForegroundColor Red
                    }
                    $buildFailCount++
                }
            } catch {
                Write-Host "  x Error: $_" -ForegroundColor Red
                $buildFailCount++
            } finally {
                Pop-Location
            }
            Write-Host ""
        }
    } else {
        Write-Host "Building individual projects..." -ForegroundColor Yellow
        foreach ($projectFile in $projectFiles) {
            Write-Host "Building: $($projectFile.FullName)" -ForegroundColor Cyan
            Push-Location (Split-Path $projectFile.FullName)
            try {
                $buildOutput = dotnet build 2>&1
                if ($LASTEXITCODE -eq 0) {
                    Write-Host "  + Build completed successfully" -ForegroundColor Green
                    $buildSuccessCount++
                } else {
                    Write-Host "  x Build failed" -ForegroundColor Red
                    Write-Host "Build errors:" -ForegroundColor Red
                    $buildOutput | Where-Object { $_ -match 'error' } | ForEach-Object {
                        Write-Host "    $_" -ForegroundColor Red
                    }
                    $buildFailCount++
                }
            } catch {
                Write-Host "  x Error: $_" -ForegroundColor Red
                $buildFailCount++
            } finally {
                Pop-Location
            }
            Write-Host ""
        }
    }
    
    Write-Host "=== Build Summary ===" -ForegroundColor Cyan
    Write-Host "Successful: $buildSuccessCount" -ForegroundColor Green
    Write-Host "Failed:     $buildFailCount" -ForegroundColor $(if ($buildFailCount -gt 0) { "Red" } else { "Gray" })
    
    Write-Host ""
    Write-Host "Next steps:" -ForegroundColor Yellow
    Write-Host "1. Review any build errors above" -ForegroundColor White
    Write-Host "2. Test your applications thoroughly" -ForegroundColor White
}

if ($WhatIf) {
    Write-Host ""
    Write-Host "To apply these changes, run the script without the -WhatIf parameter" -ForegroundColor Yellow
}
