@echo Off
rem Trigger build process for NFluent
rem usage build [Target] [Configuration]. Configuration is Release by default
Rem target is either 'nightly' (default) for nightly build, 'RC' for release candidate, 'release' for release package
Rem those targets build a tested packaged and pushed it on nuget (assuming a publishing key has been setup)
Rem other targets (e.G. CI) build 'alpha package'
cls
set target=%1
if "%target%"=="" (
	set target=%NFLUENT_STREAM%
)
if "%target%"=="" (
	set target=CI
)
set config=%2
if "%config%" == "" (
   set config=Release
)
msbuild .build\Build.proj /t:%target% /p:Configuration="%config%" /fl /v:diag /flp:LogFile=msbuild.log;Verbosity=Diagnostic /nr:false