@echo off
REM Script pour démarrer l'API TTronAlert
echo ========================================
echo Démarrage de l'API TTronAlert
echo ========================================
echo L'API sera accessible sur http://localhost:5177
echo Swagger UI: http://localhost:5177/swagger
echo.

cd src\TTronAlert.Api
dotnet run

cd ..\..
