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
  -d "{\"Title\":\"Information\",\"Message\":\"Ceci est une alerte d'information\",\"Level\":0}"
echo.
echo.

timeout /t 2 /nobreak >nul

echo [2/3] Envoi d'une alerte de niveau Avertissement...
curl -X POST http://localhost:5177/api/Alertes ^
  -H "Content-Type: application/json" ^
  -d "{\"Title\":\"Avertissement\",\"Message\":\"Attention, ceci est un avertissement important\",\"Level\":1}"
echo.
echo.

timeout /t 2 /nobreak >nul

echo [3/3] Envoi d'une alerte de niveau Critique...
curl -X POST http://localhost:5177/api/Alertes ^
  -H "Content-Type: application/json" ^
  -d "{\"Title\":\"CRITIQUE\",\"Message\":\"ALERTE CRITIQUE - Action immédiate requise!\",\"Level\":2}"
echo.
echo.

echo ========================================
echo Toutes les alertes ont été envoyées!
echo ========================================
echo.
pause
