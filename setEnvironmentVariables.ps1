Param(
[string]$SCD_Access
)
Write-Output "sauce.userName value from ADO was passed as an Argument in the ADO Task called `$env:SCD_Access`
to sauceUserName variable in the Posh. This is the value found=>$SCD_Access"

[Environment]::SetEnvironmentVariable("SCD_Access", "$SCD_Access", "User")