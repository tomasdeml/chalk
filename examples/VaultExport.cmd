REM Execute chalk.exe without parameters for help
REM Argument names have shortcuts available
..\src\Chalk\bin\Release\chalk.exe -action VaultExport -workspacePath C:\Temp\VaultWorkspaces -vaultCommandLineClientPath ..\externals\VaultProClientAPI_7_2_1_30265\vault.exe -vaultHost "test-host" -vaultUserName "scott" -vaultPassword "tiger" -vaultRepositoryName "Foo" -vaultRepositoryPath "$/Foo/trunk"
pause