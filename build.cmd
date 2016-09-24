@echo Off
cls
set config=%1
if "%config%" == "" (
   set config=release
)

msbuild .\.build\Build.proj /p:Configuration="%config%" /t:RunAll /v:M /fl /flp:LogFile=msbuild.log;Verbosity=Diagnostic /nr:false