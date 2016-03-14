if exist "C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\Tools\VsDevCmd.bat" (
	echo VS 2015
	set SdkToolsPath="C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\"
	call "C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\Tools\VsDevCmd.bat"
) else (
	echo VS 2013
	set SdkToolsPath="C:\Program Files (x86)\Microsoft SDKs\Windows\v8.0A\bin\NETFX 4.5.1 Tools\"	
	call "C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\Tools\VsDevCmd.bat"
)
echo SdkToolsPath = %SdkToolsPath%