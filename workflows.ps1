Workflow Get-BiosWFR{
    Get-CimInstance Win32_Bios
}

Workflow Get-CimClassWF {
    [CmdletBinding()]
    Param($CimClass = "Win32_Bios")

    Get-CimInstance $CimClass
}

Get-CimClassWF

Get-CimClassWF -CimClass "Win32_ComputerSystem"

Workflow Get-Bios {
    Get-CimInstance -ClassName Win32_Bios
}

Get-Bios -PSComputerName DC1 -PSAuthentication Kerberos -Verbose

#$PSParameterCollection ,,,, Get-Bios -PSParameterCollection $PSParameterCollection -AsJob -JobName 'Job3-5'
