@echo off
REM Script pour démarrer l'API AlertesApi
echo ========================================
echo Démarrage de l'API AlertesApi
echo ========================================
echo L'API sera accessible sur http://localhost:5177
echo Swagger UI: http://localhost:5177/swagger
echo.

cd AlertesApi
dotnet run

cd ..
