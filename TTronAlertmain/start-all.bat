@echo off
REM Script pour démarrer l'API et le client simultanément
echo ========================================
echo Démarrage de T-Tron-Alert
echo ========================================
echo Lancement de l'API et du client Desktop...
echo.

REM Démarrer l'API dans une nouvelle fenêtre
start "API TTronAlert" cmd /k "cd src\TTronAlert.Api && dotnet run"

REM Attendre quelques secondes pour que l'API démarre
echo Attente du démarrage de l'API (10 secondes)...
timeout /t 10 /nobreak

REM Démarrer le client Desktop dans une nouvelle fenêtre
start "Client Desktop" cmd /k "cd src\TTronAlert.Desktop && dotnet run"

echo.
echo ========================================
echo Les deux applications sont en cours de démarrage
echo API: http://localhost:5177
echo Swagger: http://localhost:5177/swagger
echo ========================================
echo.
echo Appuyez sur une touche pour fermer cette fenêtre...
pause > nul
