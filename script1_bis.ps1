$ps = new-object System.Diagnostics.Process
$ps.StartInfo.Filename = $RScriptExePath
$ps.StartInfo.Arguments = $singleScriptPath
$ps.StartInfo.RedirectStandardOutput = $True
$ps.StartInfo.UseShellExecute = $false
$ps.StartInfo.CreateNoWindow = $true
$ps.StartInfo.WorkingDirectory = $powershellWorkingDirectory
$ps.start()
$ps.WaitForExit()
[string] $Out = $ps.StandardOutput.ReadToEnd();
$Out

$params = @{
    FilePath = $RScriptExePath
    ArgumentList = $singleScriptPath
    WorkingDirectory = $powershellWorkingDirectory
    #Wait = $true
    NoNewWindow = $true
    #PassThru = $true
    RedirectStandardOutput = Join-Path -Path $powershellWorkingDirectory -ChildPath toto.out
    }
$result = Start-Process @params