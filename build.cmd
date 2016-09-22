@echo Off
cls
set config=%1
if "%config%" == "" (
   set config=release
)

msbuild %~dp0\.build\Build.proj /p:Configuration="%config%" /t:RunAll /v:M /fl /flp:LogFile=.\.build\msbuild.log;Verbosity=Diagnostic /nr:false