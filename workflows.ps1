Workflow Get-BiosWFR{
    Get-CimInstance Win32_Bios
}

Workflow Get-CimClassWF {
    [CmdletBinding()]
    Param($CimClass = "Win32_Bios", $Comp = 'DC1')

    Get-CimInstance $CimClass -PSComputerName $Comp #-PSCredential $credrk
}

Get-CimClassWF

Get-CimClassWF -CimClass "Win32_ComputerSystem" -Comp srv2

Workflow Get-Bios {
    Get-CimInstance -ClassName Win32_Bios
}

Get-Bios -PSComputerName DC1 -PSAuthentication Kerberos -Verbose

#$PSParameterCollection ,,,, Get-Bios -PSParameterCollection $PSParameterCollection -AsJob -JobName 'Job3-5'

Get-Module PSWorkflow -ListAvailable | Import-Module
Get-Module PSWorkflowUtility -ListAvailable | Import-Module

Get-Command -Module PSWorkflow
Get-Command -Module PSWorkflowUtility

Explorer $pshome\Modules\psworkflow

Workflow Invoke-Preference {
    Param($pref)
    $PSPersistancePreference = $pref

    $i = Get-CimInstance win32_bios
}

$s = Get-Date ; Invoke-Preference -pref $true
$e = Get-Date ; "Total time: {0}" -f ($e - $s).TotalMilliseconds

$s = Get-Date ; Invoke-Preference -pref $false
$e = Get-Date ; "Total time: {0}" -f ($e - $s).TotalMilliseconds

Workflow Chkpt-wf {
    Get-Process

    foreach ($x in 1..100)
        {$x; Checkpoint-Workflow; Start-Sleep -Seconds 2}
}

Chkpt-wf -AsJob

Get-Job | Receive-Job -Keep

Get-Job | Resume-Job

Get-Job | Suspend-Job


