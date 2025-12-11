# PowerShell Script to Upgrade All .NET Projects to .NET 10
# This script updates TargetFramework and common NuGet packages

param(
    [Parameter(Mandatory=$false)]
    [string]$RootPath = "",
    
    [Parameter(Mandatory=$false)]
    [switch]$WhatIf = $false,
    
    [Parameter(Mandatory=$false)]
    [switch]$SkipRestore = $false,
    
    [Parameter(Mandatory=$false)]
    [switch]$SkipBuild = $false
)

# If RootPath is not specified, use the directory where the script is located
if ([string]::IsNullOrWhiteSpace($RootPath)) {
    $RootPath = Split-Path -Parent $MyInvocation.MyCommand.Path
}

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
$updatedProjects = @()

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
            Write-Host "  ? Updated TargetFramework to net10.0" -ForegroundColor Green
            $modified = $true
        }
        
        # Update TargetFrameworks (plural) from net8.0 to net10.0
        if ($content -match '<TargetFrameworks>.*net8\.0') {
            $content = $content -replace 'net8\.0', 'net10.0'
            Write-Host "  ? Updated TargetFrameworks to include net10.0" -ForegroundColor Green
            $modified = $true
        }
        
        # Update Entity Framework Core packages
        if ($content -match 'Microsoft\.EntityFrameworkCore.*Version="8\.') {
            $content = $content -replace '(Microsoft\.EntityFrameworkCore[^"]*" Version=")8\.[0-9\.]+', '$110.0.0'
            Write-Host "  ? Updated Entity Framework Core to 10.0.0" -ForegroundColor Green
            $modified = $true
        }
        
        # Update ASP.NET Core packages
        if ($content -match 'Microsoft\.AspNetCore[^>]*Version="8\.') {
            $content = $content -replace '(Microsoft\.AspNetCore[^"]*" Version=")8\.[0-9\.]+', '$110.0.0'
            Write-Host "  ? Updated ASP.NET Core packages to 10.0.0" -ForegroundColor Green
            $modified = $true
        }
        
        # Update Microsoft.Extensions packages
        if ($content -match 'Microsoft\.Extensions[^>]*Version="8\.') {
            $content = $content -replace '(Microsoft\.Extensions[^"]*" Version=")8\.[0-9\.]+', '$110.0.0'
            Write-Host "  ? Updated Microsoft.Extensions packages to 10.0.0" -ForegroundColor Green
            $modified = $true
        }
        
        if ($modified) {
            if (-not $WhatIf) {
                # Write the updated content back to the file
                Set-Content -Path $projectFile.FullName -Value $content -NoNewline
                Write-Host "  ? File updated successfully" -ForegroundColor Green
                $updatedProjects += $projectFile.FullName
            } else {
                Write-Host "  [WhatIf] Would update this file" -ForegroundColor Yellow
            }
            $updatedCount++
        } else {
            Write-Host "  ? No changes needed (already net10.0 or different target)" -ForegroundColor Gray
            $skippedCount++
        }
        
    } catch {
        Write-Host "  ? Error: $_" -ForegroundColor Red
        $errorCount++
    }
    
    Write-Host ""
}

Write-Host "=== Summary ===" -ForegroundColor Cyan
Write-Host "Updated: $updatedCount projects" -ForegroundColor Green
Write-Host "Skipped: $skippedCount projects" -ForegroundColor Gray
Write-Host "Errors:  $errorCount projects" -ForegroundColor $(if ($errorCount -gt 0) { "Red" } else { "Gray" })
Write-Host ""

# Find all solution files if we're going to restore/build
if (-not $WhatIf -and $updatedCount -gt 0 -and (-not $SkipRestore -or -not $SkipBuild)) {
    Write-Host "=== Finding Solution Files ===" -ForegroundColor Cyan
    $solutionFiles = Get-ChildItem -Path $RootPath -Recurse -Filter *.sln -ErrorAction SilentlyContinue
    Write-Host "Found $($solutionFiles.Count) solution files" -ForegroundColor Green
    Write-Host ""
    
    $restoreSuccessCount = 0
    $restoreFailCount = 0
    $buildSuccessCount = 0
    $buildFailCount = 0
    
    foreach ($sln in $solutionFiles) {
        $slnName = $sln.Name
        $slnPath = $sln.FullName
        
        # Check if this solution contains any upgraded projects
        $slnDir = $sln.DirectoryName
        $hasUpgradedProject = $false
        foreach ($upgradedProj in $updatedProjects) {
            if ($upgradedProj.StartsWith($slnDir)) {
                $hasUpgradedProject = $true
                break
            }
        }
        
        if (-not $hasUpgradedProject) {
            Write-Host "Skipping $slnName (no upgraded projects)" -ForegroundColor Gray
            continue
        }
        
        Write-Host "Processing Solution: $slnName" -ForegroundColor Cyan
        
        # Restore packages
        if (-not $SkipRestore) {
            Write-Host "  Restoring packages..." -ForegroundColor Yellow
            $restoreOutput = & dotnet restore "$slnPath" 2>&1
            if ($LASTEXITCODE -eq 0) {
                Write-Host "  ? Restore succeeded" -ForegroundColor Green
                $restoreSuccessCount++
            } else {
                Write-Host "  ? Restore failed (exit code: $LASTEXITCODE)" -ForegroundColor Red
                Write-Host "  Error output:" -ForegroundColor Red
                $restoreOutput | Select-Object -Last 10 | ForEach-Object { Write-Host "    $_" -ForegroundColor Red }
                $restoreFailCount++
            }
        }
        
        # Build solution
        if (-not $SkipBuild) {
            Write-Host "  Building solution..." -ForegroundColor Yellow
            $buildOutput = & dotnet build "$slnPath" --no-restore 2>&1
            if ($LASTEXITCODE -eq 0) {
                Write-Host "  ? Build succeeded" -ForegroundColor Green
                $buildSuccessCount++
            } else {
                Write-Host "  ? Build failed (exit code: $LASTEXITCODE)" -ForegroundColor Red
                Write-Host "  Error output:" -ForegroundColor Red
                $buildOutput | Select-Object -Last 15 | ForEach-Object { Write-Host "    $_" -ForegroundColor Red }
                $buildFailCount++
            }
        }
        
        Write-Host ""
    }
    
    # Display restore/build summary
    if (-not $SkipRestore -or -not $SkipBuild) {
        Write-Host "=== Restore & Build Summary ===" -ForegroundColor Cyan
        if (-not $SkipRestore) {
            Write-Host "Restore Succeeded: $restoreSuccessCount solutions" -ForegroundColor Green
            Write-Host "Restore Failed:    $restoreFailCount solutions" -ForegroundColor $(if ($restoreFailCount -gt 0) { "Red" } else { "Gray" })
        }
        if (-not $SkipBuild) {
            Write-Host "Build Succeeded:   $buildSuccessCount solutions" -ForegroundColor Green
            Write-Host "Build Failed:      $buildFailCount solutions" -ForegroundColor $(if ($buildFailCount -gt 0) { "Red" } else { "Gray" })
        }
        Write-Host ""
        
        if ($buildFailCount -gt 0) {
            Write-Host "? Some builds failed. Review the errors above and:" -ForegroundColor Yellow
            Write-Host "  1. Check for deprecated APIs in .NET 10" -ForegroundColor White
            Write-Host "  2. Update third-party packages to .NET 10 compatible versions" -ForegroundColor White
            Write-Host "  3. Review breaking changes: https://docs.microsoft.com/dotnet" -ForegroundColor White
        } elseif ($buildSuccessCount -gt 0) {
            Write-Host "? All builds succeeded! Your projects are ready for .NET 10." -ForegroundColor Green
        }
    }
}

if (-not $WhatIf -and $updatedCount -gt 0) {
    Write-Host ""
    Write-Host "Upgrade complete!" -ForegroundColor Green
    Write-Host "Next steps:" -ForegroundColor Yellow
    Write-Host "1. Test your applications thoroughly" -ForegroundColor White
    Write-Host "2. Review any build failures above" -ForegroundColor White
    Write-Host "3. Update third-party packages if needed" -ForegroundColor White
}

if ($WhatIf) {
    Write-Host ""
    Write-Host "To apply these changes, run the script without the -WhatIf parameter" -ForegroundColor Yellow
}
