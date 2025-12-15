param (
    [string]$rootPath = "."
)

# Print instructions if no arguments are provided
if ($rootPath -eq ".") {
  Write-Host "Recursively removes all database files from the specified directory."
  Write-Host "Usage: CleanDB.ps1 <rootPath>"
  Write-Host "Example: .\CleanDB.ps1 C:\M55340A_Allfiles\Mod01"
  exit 1
}

# prompt for confirmation:
Write-Host "About to recursively delete all databases in $rootPath"
$confirmation = Read-Host "Are you SURE you want to continue? (Y/N)"
if ($confirmation -ne "Y") {
  Write-Host "Operation cancelled."
  exit 1
}

Get-ChildItem $rootPath -Include *.db,*.db-wal,*.db-shm,*.ldf,*.mdf -Recurse -Force | ForEach-Object {
  Write-Host "Removing" + $_.FullName
  Remove-Item $_.FullName -Force
}