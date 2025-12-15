#!/bin/bash
# Script pour démarrer l'API et le client simultanément
echo "========================================"
echo "Démarrage de T-Tron-Alert"
echo "========================================"
echo "Lancement de l'API et du client Desktop..."
echo ""

# Démarrer l'API en arrière-plan
echo "Démarrage de l'API..."
cd api/TTronAlert.Api
dotnet run &
API_PID=$!
cd ../..

# Attendre quelques secondes pour que l'API démarre
echo "Attente du démarrage de l'API (10 secondes)..."
sleep 10

# Démarrer le client Desktop en arrière-plan
echo "Démarrage du client Desktop..."
cd app/TTronAlert.Desktop
dotnet run &
CLIENT_PID=$!
cd ../..

echo ""
echo "========================================"
echo "Les deux applications sont en cours de démarrage"
echo "API: http://localhost:62051"
echo "Swagger: http://localhost:62051/swagger"
echo "========================================"
echo ""
echo "PIDs: API=$API_PID Client=$CLIENT_PID"
echo "Pour arrêter: kill $API_PID $CLIENT_PID"
echo ""

# Attendre que l'utilisateur appuie sur Ctrl+C
trap "kill $API_PID $CLIENT_PID 2>/dev/null; exit" INT TERM
wait
