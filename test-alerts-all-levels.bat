@echo off
REM Script pour envoyer différents types d'alertes de test
echo ========================================
echo Envoi de plusieurs alertes de test
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

echo [1/3] Envoi d'une alerte de niveau Info...
curl -X POST http://localhost:5177/api/Alertes ^
  -H "Content-Type: application/json" ^
  -d "{\"titre\":\"Information\",\"message\":\"Ceci est une alerte d'information\",\"niveau\":\"Info\"}"
echo.
echo.

timeout /t 2 /nobreak >nul

echo [2/3] Envoi d'une alerte de niveau Avertissement...
curl -X POST http://localhost:5177/api/Alertes ^
  -H "Content-Type: application/json" ^
  -d "{\"titre\":\"Avertissement\",\"message\":\"Attention, ceci est un avertissement important\",\"niveau\":\"Avertissement\"}"
echo.
echo.

timeout /t 2 /nobreak >nul

echo [3/3] Envoi d'une alerte de niveau Critique...
curl -X POST http://localhost:5177/api/Alertes ^
  -H "Content-Type: application/json" ^
  -d "{\"titre\":\"CRITIQUE\",\"message\":\"ALERTE CRITIQUE - Action immédiate requise!\",\"niveau\":\"Critique\"}"
echo.
echo.

echo ========================================
echo Toutes les alertes ont été envoyées!
echo ========================================
echo.
pause
