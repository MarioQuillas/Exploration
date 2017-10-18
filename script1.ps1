$ScriptPath = "C:\MyGithub\Exploration\Exploration.RScripts"
$RScriptExePath = "C:\Program Files\R\R-3.4.2\bin\Rscript.exe"
$singleScriptPath = Join-Path -Path $ScriptPath -ChildPath script1.R
$powershellWorkingDirectory = "C:\PowershellWorkingDirectory"
$outputPath = Join-Path -Path $powershellWorkingDirectory -ChildPath toto.out

#& $RScriptExePath $scriptPath
$ps = new-object System.Diagnostics.Process
$ps.StartInfo.Filename = $RScriptExePath
$ps.StartInfo.Arguments = $singleScriptPath
$ps.StartInfo.RedirectStandardOutput = $True
$ps.StartInfo.UseShellExecute = $false
$ps.StartInfo.CreateNoWindow = $true
#$ps.StartInfo.WorkingDirectory = $powershellWorkingDirectory
$ps.start()
#$ps.WaitForExit()
[string] $Out = $ps.StandardOutput.ReadToEnd();
$Out

#cmd /c $RScriptExePath $scriptPath