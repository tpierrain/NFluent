@echo Off
cls
echo Launching mutation tests
cd tests\NFluent.NetStandard.20.Tests
dotnet stryker
cd ..\..
