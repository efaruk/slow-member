@echo off

call "C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\Tools\VsDevCmd.bat"

C:\Windows\Microsoft.NET\Framework64\v4.0.30319\msbuild log4net.Appender.Extended.sln /m /p:Configuration=Release

.nuget\nuget pack log4net.Appender.Extended\log4net.Appender.Extended.csproj -Prop Configuration=Release -IncludeReferencedProjects

@echo on
pause