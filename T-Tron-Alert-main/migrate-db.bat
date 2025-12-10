@echo off
REM Script pour appliquer les migrations de base de données
echo ========================================
echo Migration de la base de données
echo ========================================
echo.

REM Restaurer les outils dotnet si nécessaire
echo Restauration des outils dotnet...
dotnet tool restore
echo.

cd src\TTronAlert.Api
dotnet ef database update

if %ERRORLEVEL% EQU 0 (
    echo.
    echo ========================================
    echo Migration réussie!
    echo ========================================
) else (
    echo.
    echo ========================================
    echo Erreur lors de la migration!
    echo ========================================
    cd ..
    exit /b 1
)

cd ..
pause
