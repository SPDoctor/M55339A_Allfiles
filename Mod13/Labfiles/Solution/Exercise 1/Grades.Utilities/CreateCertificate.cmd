:: TODO: Exercise 1: Task 1a: Create an asymmetric certificate
:: Open a PowerShell window running as Administrator and run this command file.
New-SelfSignedCertificate -Subject "CN=Grades" -HashAlgorithm sha1 -KeyExportPolicy Exportable -CertStoreLocation "Cert:\CurrentUser\My" -KeySpec KeyExchange