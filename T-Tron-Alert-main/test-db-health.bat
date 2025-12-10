@echo off
echo ========================================
echo Test du Health Check - Base de donnees
echo ========================================
echo.

echo Test de la connexion rapide (ping)...
curl -X GET "http://localhost:5177/api/health/database/ping" -H "accept: */*"
echo.
echo.

echo Test du statut detaille de la base de donnees...
curl -X GET "http://localhost:5177/api/health/database" -H "accept: application/json"
echo.
echo.

echo ========================================
echo Tests termines
echo ========================================
pause
