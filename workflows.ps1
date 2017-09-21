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
