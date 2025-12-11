# .NET 10 Upgrade Script

Automated PowerShell script to upgrade all .NET 8 projects to .NET 10 in this repository.

## Overview

The `Upgrade-ToNet10.ps1` script is a comprehensive tool that automatically:

? Updates `TargetFramework` from `net8.0*` to `net10.0*`  
? Updates Entity Framework Core packages to 10.0.0  
? Updates ASP.NET Core packages to 10.0.0  
? Updates Microsoft.Extensions packages to 10.0.0  
? Restores NuGet packages for all solutions  
? Builds all solutions and reports results  
? Provides detailed progress and error reporting  

## Quick Start

### Preview Changes (Safe - No Modifications)

```powershell
.\Upgrade-ToNet10.ps1 -WhatIf
```

This will show you exactly what will be changed without modifying any files.

### Perform the Upgrade

```powershell
.\Upgrade-ToNet10.ps1
```

This will:
1. Upgrade all .NET 8 projects to .NET 10
2. Restore packages for all solutions containing upgraded projects
3. Build all solutions and report success/failure

## Parameters

| Parameter | Description | Default |
|-----------|-------------|---------|
| `-RootPath` | Root directory to scan for projects | `current directory` |
| `-WhatIf` | Preview changes without modifying files | `false` |
| `-RebuildAll` | Rebuild all projects/solutions even if already .NET 10 | `false` |
| `-SkipRestore` | Skip the automatic package restore step | `false` |
| `-SkipBuild` | Skip the automatic build step | `false` |

## Usage Examples

```powershell
# Preview what will change (recommended first step)
.\Upgrade-ToNet10.ps1 -WhatIf

# Upgrade all projects with restore and build
.\Upgrade-ToNet10.ps1

# Rebuild all projects even if already upgraded to .NET 10
.\Upgrade-ToNet10.ps1 -RebuildAll

# Upgrade and restore packages, but skip building
.\Upgrade-ToNet10.ps1 -SkipBuild

# Upgrade only, no restore or build
.\Upgrade-ToNet10.ps1 -SkipRestore -SkipBuild

# Upgrade projects in a specific directory
.\Upgrade-ToNet10.ps1 -RootPath "C:\MyProjects"
```

## What Gets Updated

### 1. Target Framework

The script updates all variants of .NET 8 targets:

- `net8.0` ? `net10.0`
- `net8.0-windows` ? `net10.0-windows`
- `net8.0-android` ? `net10.0-android`
- `net8.0-ios` ? `net10.0-ios`
- `net8.0-maccatalyst` ? `net10.0-maccatalyst`

### 2. NuGet Packages

Microsoft packages are automatically updated to version 10.0.0:

- `Microsoft.EntityFrameworkCore*`
- `Microsoft.AspNetCore*`
- `Microsoft.Extensions*`

## Automated Build & Restore

After upgrading projects, the script will:

1. **Find all solution files** (*.sln) in the repository
2. **Identify solutions** that contain upgraded projects
3. **Run `dotnet restore`** on each solution (unless using `-RebuildAll`)
4. **Run `dotnet build`** on each solution
5. **Report detailed results** including any errors

### RebuildAll Mode

When using the `-RebuildAll` parameter:
- All solutions/projects are built regardless of whether they were upgraded
- `dotnet build` automatically handles any necessary restore operations
- Useful for verifying all projects build successfully after an upgrade session

**Note:** Without `-RebuildAll`, only solutions containing newly upgraded projects are restored and built, saving time when some projects are already at .NET 10.

### Sample Output

```
=== .NET 10 Upgrade Script ===
Root Path: C:\M55339A_Allfiles
Found 127 project files

Processing: C:\M55339A_Allfiles\Mod01\...\School.csproj
  ? Updated TargetFramework to net10.0
  ? Updated Entity Framework Core to 10.0.0
  ? File updated successfully

=== Summary ===
Updated: 45 projects
Skipped: 82 projects
Errors:  0 projects

=== Finding Solution Files ===
Found 42 solution files

Processing Solution: School.sln
  Restoring packages...
  ? Restore succeeded
  Building solution...
  ? Build succeeded

=== Restore & Build Summary ===
Restore Succeeded: 42 solutions
Restore Failed:    0 solutions
Build Succeeded:   40 solutions
Build Failed:      2 solutions

? All builds succeeded! Your projects are ready for .NET 10.
```

## Safety Features

? **WhatIf mode** - Preview all changes before applying  
? **Read-only scanning** - Only modifies .csproj files  
? **Error handling** - Continues processing even if individual projects fail  
? **Detailed logging** - See exactly what's happening at each step  
? **Selective processing** - Only builds solutions with upgraded projects  

## Best Practices

1. **Commit your work first** - Ensure all changes are committed to Git before running
2. **Use -WhatIf first** - Preview the changes before applying them
3. **Review the output** - Check for any errors or warnings
4. **Test thoroughly** - Run your applications to ensure compatibility

## Manual Steps You May Need

While the script handles most common scenarios, you may need to manually:

- Update third-party NuGet packages to .NET 10-compatible versions
- Update custom build configurations or MSBuild targets
- Review and update deprecated API usage
- Update platform-specific project settings

## Troubleshooting

### Build Failures

If builds fail after upgrading:

1. **Check for deprecated APIs** - Review the [.NET 10 breaking changes](https://docs.microsoft.com/dotnet)
2. **Update third-party packages** - Some may need .NET 10-specific versions
3. **Clear caches** - Run `dotnet nuget locals all --clear`
4. **Clean rebuild** - Run `dotnet clean && dotnet restore && dotnet build`

### Restore Failures

If package restore fails:

1. Verify .NET 10 SDK is installed: `dotnet --list-sdks`
2. Check NuGet package sources are accessible
3. Review firewall/proxy settings
4. Try restoring individual projects to isolate the issue

## Requirements

- **PowerShell** 5.1 or later
- **.NET 10 SDK** installed on your machine
- **Write permissions** to project files and directories
- **Internet connection** for NuGet package restore

## Repository Information

This repository contains **127+ projects** across multiple modules. The script will process all of them automatically, saving significant manual effort.

## Support

For issues or questions:
- Review the detailed error output from the script
- Check the .NET 10 migration documentation
- Ensure all prerequisites are installed

---

**Script Location:** `C:\M55339A_Allfiles\Upgrade-ToNet10.ps1`  
**Last Updated:** Enhanced with automatic restore and build capabilities
