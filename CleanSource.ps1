# Remove all bin, obj, .vs, and TestResults folders from the current directory and all subdirectories
Get-ChildItem . -include .vs,bin,obj,TestResults -Recurse -Force | ForEach-Object ($_) { 
  Write-Host "Removing" + $_.FullName 
  Remove-Item $_.FullName -Force -Recurse 
}

# Remove all school.db-wal files
Get-ChildItem . -Filter school.db-wal -Recurse -Force | ForEach-Object {
  Write-Host "Removing" + $_.FullName
  Remove-Item $_.FullName -Force
}

# Remove all school.db-shm files
Get-ChildItem . -Filter school.db-shm -Recurse -Force | ForEach-Object {
  Write-Host "Removing" + $_.FullName
  Remove-Item $_.FullName -Force
}

# Remove all school.db files
Get-ChildItem . -Filter school.db -Recurse -Force | ForEach-Object {
  Write-Host "Removing" + $_.FullName
  Remove-Item $_.FullName -Force
}