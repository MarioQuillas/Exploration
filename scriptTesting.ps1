& "C:\Program Files\R\R-3.1.2\bin\Rscript.exe" "C:\MyGithub\Exploration\Exploration.RScripts\script1.R"

$conn = New-Object System.Data.SqlClient.SqlConnection # System.Data.OleDb.OleDbConnection
$conn.ConnectionString = "Data Source=SQLLFISDEV,1460;Initial Catalog=TardisFlow;Persist Security Info=True;User ID=TardisFlowUsrRW;Password=6Sk&>3R-" # whatever you are testing

$command = New-Object System.Data.SqlClient.SqlCommand # $conn.CreateCommand() 
$command.CommandText = "SELECT * FROM TEST_TABLE"
$command.Connection = $conn

$conn.Open()

$sqlReader = $command.ExecuteReader()

while($sqlReader.Read())
{
    Write-Host ("Test Column : "+$sqlReader["TestColumn"]) -ForegroundColor Green
}

$conn.Close()

$azureEmulatorExe = "C:\Program Files (x86)\Microsoft SDKs\Azure\Storage Emulator\AzureStorageEmulator.exe"

#Account name: devstoreaccount1
#Account key: Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==




