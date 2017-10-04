Function Start-RScript {
    # [CmdletBinding()]
    Param([Parameter(ValueFromPipeline, Mandatory)]$RScript)
    $RScriptExePath = "C:\Program Files\R\R-3.1.2\bin\Rscript.exe"
    & $RScriptExePath $RScript
}

Function Start-LocalRScript {
    Param([Parameter(ValueFromPipeline, Mandatory)]$Name)
    $ScriptPath = "C:\MyGithub\Exploration\Exploration.RScripts"
    $RScriptExePath = "C:\Program Files\R\R-3.1.2\bin\Rscript.exe"
    $scriptPath = Join-Path -Path $ScriptPath -ChildPath $Name
    & $RScriptExePath $scriptPath
}

#Start-LocalRScript script1.R
#scriptblock = {} Start-job -scriptblock

Workflow Get-RScriptWorkflow {
    InlineScript {
        C:\MyGithub\Exploration\script1.ps1
    }
    

    #Start-LocalRScript script1.R

    #Parallel {
    #    Start-LocalRScript script2.R
    #    Start-LocalRScript script3.R
    #}
}

Trace-Command -Name ParameterBinding -Expression  { 

  #Get-Process  -Name powershell  | Stop-Process  -WhatIf
  #Start-LocalRScript script1.R
  Get-RScriptWorkflow
}  -PSHost
