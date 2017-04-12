@echo Off
cls
set config=%1
if "%config%" == "" (
   set config=release
)

REM msbuild .\.build\Build.proj /p:Configuration="%config%" /t:RunAll /v:M /fl /flp:LogFile=msbuild.log;Verbosity=Diagnostic /nr:false
msbuild .\.build\Build.proj /p:Configuration="%config%" /t:Package /v:M /fl /flp:LogFile=msbuild.log;Verbosity=Diagnostic /nr:false