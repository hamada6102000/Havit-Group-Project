# PowerShell script to run the SQL migration
$connectionString = "Server=HAMADA-MOSTAFA-\SQLEXPRESS;Database=HavitGroupDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
$sqlScript = Get-Content "add_homeimages_table.sql" -Raw

# You'll need sqlcmd utility for this
# sqlcmd -S "HAMADA-MOSTAFA-\SQLEXPRESS" -d "HavitGroupDb" -E -i "add_homeimages_table.sql"

Write-Host "Please run the SQL script manually in SQL Server Management Studio:"
Write-Host "File: add_homeimages_table.sql"
Write-Host ""
Write-Host "Or use sqlcmd:"
Write-Host 'sqlcmd -S "HAMADA-MOSTAFA-\SQLEXPRESS" -d "HavitGroupDb" -E -i "add_homeimages_table.sql"'

