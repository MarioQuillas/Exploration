Clear-Host

if (-not (Get-Module Exploration.Cmdlets)){
    Import-Module .\Exploration.Cmdlets\bin\Debug\Exploration.Cmdlets.dll
}

$TestCases = @(
    @{"Test"={Get-SquaredValue -Number 1}; "Expected"=1}
    @{"Test"={Get-SquaredValue -Number 2}; "Expected"=4}
    @{"Test"={Get-SquaredValue -Number 3}; "Expected"=9}
)

$testCount = 0
$passedTests = 0

foreach($testCase in $TestCases) {
    $testCount++

    $output = . $testCase["Test"]

    if ($output -eq $testCase["Expected"]) {
        $passedTests++
    } else {
        Write-Host(
            "Failed '{0}' yielded '{1}' instead of '{2}'" -f
            $testCase["Test"], $output, $testCase["Expected"]
            ) -ForegroundColor Red
    }
}

if ($passedTests -gt 0) {
    Write-Host "Passed $passedTests of $testCount" -ForegroundColor Green
} else {
    Write-Host "FAILED ALL TESTS" -ForegroundColor Red
}