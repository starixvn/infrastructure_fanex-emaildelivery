@ECHO OFF

REM The following directory is for .NET 2.0
set DOTNETFX2=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319
set PATH=%PATH%;%DOTNETFX2%

echo Installing Broadcast Command Service...
echo ---------------------------------------------------
InstallUtil /i Fanex.MailDelivery.AdminWinService.exe /servicename="Mail Station Admin Service"
echo ---------------------------------------------------
echo Done.