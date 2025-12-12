@echo off
REM Script pour nettoyer les fichiers de compilation
echo ========================================
echo Nettoyage des fichiers de compilation
echo ========================================
echo.

echo Nettoyage en cours...
dotnet clean T-Tron-Alert.sln

if %ERRORLEVEL% EQU 0 (
    echo.
    echo ========================================
    echo Nettoyage réussi!
    echo ========================================
) else (
    echo.
    echo ========================================
    echo Erreur lors du nettoyage!
    echo ========================================
    exit /b 1
)

echo.
pause
