@echo off
REM Script pour démarrer l'API et le client simultanément
echo ========================================
echo Démarrage de T-Tron-Alert
echo ========================================
echo Lancement de l'API et du client WPF...
echo.

REM Démarrer l'API dans une nouvelle fenêtre
start "API AlertesApi" cmd /k "cd AlertesApi && dotnet run"

REM Attendre quelques secondes pour que l'API démarre
echo Attente du démarrage de l'API (10 secondes)...
timeout /t 10 /nobreak

REM Démarrer le client WPF dans une nouvelle fenêtre
start "Client WPF" cmd /k "cd ClientAlertesWPF && dotnet run"

echo.
echo ========================================
echo Les deux applications sont en cours de démarrage
echo API: http://localhost:5177
echo Swagger: http://localhost:5177/swagger
echo ========================================
echo.
echo Appuyez sur une touche pour fermer cette fenêtre...
pause > nul
