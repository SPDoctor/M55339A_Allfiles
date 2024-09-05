# Remove all bin, obj, .vs, and TestResults folders from the current directory and all subdirectories
Get-ChildItem . -include .vs,bin,obj,TestResults -Recurse -Force | ForEach-Object ($_) { 
  Write-Host "Removing" + $_.FullName 
  Remove-Item $_.FullName -Force -Recurse 
}

# Remove all .db-wal files
Get-ChildItem . -Filter *.db-wal -Recurse -Force | ForEach-Object {
  Write-Host "Removing" + $_.FullName
  Remove-Item $_.FullName -Force
}
# Remove all .db-shm files
Get-ChildItem . -Filter *.db-shm -Recurse -Force | ForEach-Object {
  Write-Host "Removing" + $_.FullName
  Remove-Item $_.FullName -Force
}