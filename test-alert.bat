@echo off
REM Script pour envoyer une alerte de test via l'API
echo ========================================
echo Envoi d'une alerte de test
echo ========================================
echo.

REM Vérifier que curl est disponible
where curl >nul 2>nul
if %ERRORLEVEL% NEQ 0 (
    echo ERREUR: curl n'est pas disponible sur ce système.
    echo Veuillez installer curl ou utiliser Swagger UI pour tester.
    echo Swagger UI: http://localhost:5177/swagger
    pause
    exit /b 1
)

echo Envoi d'une alerte de test a l'API...
echo.

curl -X POST http://localhost:5177/api/Alertes ^
  -H "Content-Type: application/json" ^
  -d "{\"titre\":\"Alerte de test\",\"message\":\"Ceci est une alerte de test envoyée depuis test-alert.bat\",\"niveau\":\"Info\"}"

echo.
echo.
if %ERRORLEVEL% EQU 0 (
    echo ========================================
    echo Alerte envoyée avec succès!
    echo ========================================
) else (
    echo ========================================
    echo Erreur lors de l'envoi de l'alerte!
    echo Verifiez que l'API est demarree.
    echo ========================================
)

echo.
pause
