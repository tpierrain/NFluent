@echo Off
cls
set config=%1
if "%config%" == "" (
   set config=debug
)

%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild %~dp0\.build\Build.proj /p:Configuration="%config%" /t:RunAll /v:M /fl /flp:LogFile=.\.build\msbuild.log;Verbosity=Normal /nr:false