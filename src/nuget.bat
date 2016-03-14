@echo off

call environment.bat

C:\Windows\Microsoft.NET\Framework64\v4.0.30319\msbuild SlowMember.sln /m /p:Configuration=Release

.nuget\nuget pack SlowMember\SlowMember.csproj -Prop Configuration=Release -IncludeReferencedProjects

@echo on
pause