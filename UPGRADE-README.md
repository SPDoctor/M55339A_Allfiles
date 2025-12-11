# .NET 10 Upgrade Scripts

This directory contains PowerShell scripts to automate upgrading .NET 8 projects to .NET 10.

## Scripts Available

### 1. `Upgrade-ToNet10.ps1` (Comprehensive)
A full-featured script with logging and safety features.

**Features:**
- Updates `TargetFramework` from `net8.0*` to `net10.0*`
- Updates Entity Framework Core packages to 10.0.0
- Updates ASP.NET Core packages to 10.0.0
- Updates Microsoft.Extensions packages to 10.0.0
- Supports WhatIf mode for safe testing
- Detailed progress reporting

**Usage:**

```powershell
# Test run (no changes made)
.\Upgrade-ToNet10.ps1 -WhatIf

# Upgrade all projects in the repository
.\Upgrade-ToNet10.ps1

# Upgrade projects in a specific directory
.\Upgrade-ToNet10.ps1 -RootPath "C:\MyProjects"
```

### 2. `Quick-Upgrade.ps1` (Simple)
A lightweight script for quick upgrades.

**Usage:**

```powershell
# Run from the directory containing your projects
.\Quick-Upgrade.ps1
```

This will upgrade all .NET 8 projects in the current directory and all subdirectories.

## What Gets Updated

Both scripts perform the following updates:

1. **Target Framework:**
   - `net8.0` ? `net10.0`
   - `net8.0-windows` ? `net10.0-windows`
   - `net8.0-android` ? `net10.0-android`
   - `net8.0-ios` ? `net10.0-ios`
   - `net8.0-maccatalyst` ? `net10.0-maccatalyst`

2. **NuGet Packages:**
   - `Microsoft.EntityFrameworkCore*` ? `10.0.0`
   - `Microsoft.AspNetCore*` ? `10.0.0`
   - `Microsoft.Extensions*` ? `10.0.0`

## After Running the Script

1. **Restore packages:**
   ```powershell
   dotnet restore
   ```

2. **Build your solutions:**
   ```powershell
   dotnet build
   ```

3. **Test your applications** to ensure compatibility

## Manual Steps You May Need

While these scripts handle the most common scenarios, you may need to manually update:

- Third-party NuGet packages that have .NET 10-specific versions
- Custom build configurations
- Platform-specific project settings
- Solution files (if they reference specific framework versions)

## Troubleshooting

If you encounter build errors after upgrading:

1. Check for deprecated APIs - review the [.NET 10 breaking changes documentation](https://docs.microsoft.com/dotnet)
2. Update third-party packages to .NET 10 compatible versions
3. Clear NuGet caches: `dotnet nuget locals all --clear`
4. Rebuild: `dotnet clean && dotnet restore && dotnet build`

## Safety

Both scripts only modify `.csproj` files. It's recommended to:
- Commit your changes to source control before running
- Use the `-WhatIf` parameter on `Upgrade-ToNet10.ps1` to preview changes
- Test in a development environment first

## Example Output

```
=== .NET 10 Upgrade Script ===
Root Path: C:\M55339A_Allfiles
Found 127 project files

Processing: C:\M55339A_Allfiles\Mod01\School\School.csproj
  ? Updated TargetFramework to net10.0
  ? Updated Entity Framework Core to 10.0.0
  ? File updated successfully

=== Summary ===
Updated: 45 projects
Skipped: 82 projects
Errors:  0 projects
```

## Requirements

- PowerShell 5.1 or later
- .NET 10 SDK installed on your machine
- Write permissions to the project files
