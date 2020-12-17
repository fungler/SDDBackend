Param(
[string]$scd
)
Write-Output "sauce.userName value from ADO was passed as an Argument in the ADO Task called `$env:GitHubPAT`
to sauceUserName variable in the Posh. This is the value found=>$GitHubPAT"

[Environment]::SetEnvironmentVariable("GitHubPAT", "$scd", "User")