
set H=R:\KSP_1.3.1_dev
echo %H%

copy /Y "bin\Debug\VaporVent.dll" "..\GameData\VaporVent\Plugins"
copy /Y VaporVent.version ..\GameData\VaporVent

cd ..\GameData
mkdir "%H%\GameData\VaporVent"
xcopy /y /s VaporVent "%H%\GameData\VaporVent"
