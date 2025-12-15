param (
    [string]$rootPath = "."
)

# Print instructions if no arguments are provided
if ($rootPath -eq ".") {
  Write-Host "Cleans up all code in the specified directory."
  Write-Host "Usage: CodeFormat.ps1 <rootPath>"
  Write-Host "Example: .\CodeFormat.ps1 C:\M55340A_Allfiles\Mod01"
  exit 
}
$startPath = Get-Location

# Get all .sln files in the subdirectories
$solutionFiles = Get-ChildItem -Path $rootPath -Recurse -Include *.sln

# Analyze and clean up each solution
foreach ($solutionFile in $solutionFiles) {
  if (Test-Path $solutionFile) {
    Write-Host "Running Code Format on $solutionFile"
    dotnet format $solutionFile.FullName
  } else {
    Write-Host "Solution file not found for $($projectFile.FullName)"
  }
}

Write-Host "Completed code formatting."
Set-Location -Path $startPath

exit 0