$ScriptPath = "C:\MyGithub\Exploration\Exploration.RScripts"
$RScriptExePath = "C:\Program Files\R\R-3.1.2\bin\Rscript.exe"
$scriptPath = Join-Path -Path $ScriptPath -ChildPath script2.R
& $RScriptExePath $scriptPath