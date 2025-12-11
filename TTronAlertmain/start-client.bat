@echo off
REM Script pour démarrer le client Desktop (Avalonia)
echo ========================================
echo Démarrage du client Desktop
echo ========================================
echo Le client démarrera avec l'interface Avalonia
echo.

cd src\TTronAlert.Desktop
dotnet run

cd ..\..
