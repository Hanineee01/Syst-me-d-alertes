# T-Tron Alert

Système d'alertes temps réel avec .NET 8, SignalR et Avalonia UI.

## Installation

```bash
git clone https://github.com/SulivanM/T-Tron-Alert.git
cd T-Tron-Alert
dotnet build
```

## Configuration

Modifier `src/TTronAlert.Api/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=ttronalert;User=root;Password=votre_mdp;"
  }
}
```

## Démarrage

Lancer l'API:
```bash
cd src/TTronAlert.Api
dotnet run
```

Lancer le client:
```bash
cd src/TTronAlert.Desktop
dotnet run
```

API: `http://localhost:5177`

Swagger: `http://localhost:5177/swagger`

## Tester

```bash
curl -X POST http://localhost:5177/api/alerts \
  -H "Content-Type: application/json" \
  -d '{"title":"Test","message":"Message test","level":0}'
```

Niveaux: 0=Info, 1=Warning, 2=Critical

## Structure

```
src/
├── TTronAlert.Shared/   # Modèles
├── TTronAlert.Api/      # Backend
└── TTronAlert.Desktop/  # Client
```
