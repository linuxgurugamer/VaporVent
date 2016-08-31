rem
@echo off

set DEFHOMEDRIVE=d:
set DEFHOMEDIR=%DEFHOMEDRIVE%%HOMEPATH%
set HOMEDIR=
set HOMEDRIVE=%CD:~0,2%

set RELEASEDIR=d:\Users\jbb\release
set ZIP="c:\Program Files\7-zip\7z.exe"
echo Default homedir: %DEFHOMEDIR%

set /p HOMEDIR= "Enter Home directory, or <CR> for default: "

if "%HOMEDIR%" == "" (
set HOMEDIR=%DEFHOMEDIR%
)
echo %HOMEDIR%

SET _test=%HOMEDIR:~1,1%
if "%_test%" == ":" (
set HOMEDRIVE=%HOMEDIR:~0,2%
)

d:
cd D:\Users\jbb\github\VaporVent\Source

type VaporVent.version
set /p VERSION= "Enter version: "


mkdir %HOMEDIR%\install\GameData\VaporVent
mkdir %HOMEDIR%\install\GameData\VaporVent\Plugins
mkdir %HOMEDIR%\install\GameData\VaporVent\Parts

 

del /s /q %HOMEDIR%\install\GameData\VaporVent\*
rem del /y %HOMEDIR%\install\GameData\VaporVent\Plugins
rem del /y %HOMEDIR%\install\GameData\VaporVent\Parts

copy /Y "%~dp0bin\Release\VaporVent.dll" "%HOMEDIR%\install\GameData\VaporVent\Plugins"
xcopy ..\GameData\VaporVent\parts  %HOMEDIR%\install\GameData\VaporVent\Parts

copy /Y "License.txt" "%HOMEDIR%\install\GameData\VaporVent"
copy /Y MiniAVC.dll  "%HOMEDIR%\install\GameData\VaporVent"
copy /Y VaporVent.version  "%HOMEDIR%\install\GameData\VaporVent"

%HOMEDRIVE%
cd %HOMEDIR%\install

set FILE="%RELEASEDIR%\VaporVent-%VERSION%.zip"
IF EXIST %FILE% del /F %FILE%
%ZIP% a -tzip %FILE% Gamedata\VaporVent
