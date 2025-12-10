@echo off
REM Script de compilation de la solution T-Tron-Alert
echo ========================================
echo Compilation de T-Tron-Alert
echo ========================================
echo.

REM Restaurer les outils dotnet si nécessaire
echo Restauration des outils dotnet...
dotnet tool restore
echo.

dotnet build TTronAlert.sln

if %ERRORLEVEL% EQU 0 (
    echo.
    echo ========================================
    echo Compilation réussie!
    echo ========================================
) else (
    echo.
    echo ========================================
    echo Erreur lors de la compilation!
    echo ========================================
    exit /b 1
)

pause
