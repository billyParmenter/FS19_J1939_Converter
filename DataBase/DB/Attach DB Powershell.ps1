
 
#
# Get SQL Server database (MDF/LDF).
#

    $mdfFilename = 'J1939_Full.MDF'
    $ldfFilename = 'J1939_Full.LDF'
    $DBName = 'J1939_Full'
 
    #
    # Attach SQL Server database
    #
    Add-PSSnapin SqlServerCmdletSnapin* -ErrorAction SilentlyContinue
        If (!$?) {Import-Module SQLPS -WarningAction SilentlyContinue}
If (!$?) {"Error loading Microsoft SQL Server PowerShell module. Please check if it is installed."; Exit}
$attachSQLCMD = @"
USE [master]
GO
CREATE DATABASE [$DBName] ON (FILENAME = '$mdfFilename.mdf'),(FILENAME = '$ldfFilename.ldf') for ATTACH
GO
"@ 
    Invoke-Sqlcmd $attachSQLCMD -QueryTimeout 3600 -ConnectionString 'Data Source=ConnorAsus;Initial Catalog=master;User ID="J1939 Access";Password=DirtyMike'
 
