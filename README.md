# T-Tron Alert

Système d'alertes temps réel avec .NET 8, SignalR et Avalonia UI.

## Installation

```bash
git clone https://github.com/SulivanM/T-Tron-Alert.git
cd T-Tron-Alert
dotnet build
```

## Configuration

### API (Backend)

Modifier `api/TTronAlert.Api/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=systeme_alertes;User=root;Password=votre_mdp;"
  }
}
```

### Client Desktop

Modifier `app/TTronAlert.Desktop/appsettings.json`:

```json
{
  "AlertSystem": {
    "WorkstationId": "poste-1",
    "ApiBaseUrl": "http://localhost:62051",
    "HubPath": "/alerthub",
    "AutoReconnect": true
  }
}
```

**Note:** Chaque poste client doit avoir un `WorkstationId` unique (ex: "poste-1", "poste-2", etc.)

## Démarrage

### Démarrage rapide

**Windows:**
```bash
start-all.bat      # Démarre API + Client
```

**Linux/macOS:**
```bash
./start-all.sh     # Démarre API + Client
```

### Démarrage manuel

**1. Appliquer les migrations de base de données** (première fois uniquement) :
```bash
# Windows
migrate-db.bat

# Linux/macOS
./migrate-db.sh
```

**2. Démarrer l'API** (Terminal 1) :
```bash
cd api/TTronAlert.Api
dotnet run
```

**3. Démarrer le client** (Terminal 2, attendre ~10s que l'API démarre) :
```bash
cd app/TTronAlert.Desktop
dotnet run
```

**URLs:**
- API: `http://localhost:62051` (HTTP) ou `https://localhost:62050` (HTTPS)
- Swagger: `http://localhost:62051/swagger`

## Tester l'API

Envoyer une alerte de test :

```bash
curl -X POST http://localhost:62051/api/alerts \
  -H "Content-Type: application/json" \
  -d '{"title":"Test","message":"Message test","level":0,"targetWorkstation":"poste-1"}'
```

**Niveaux d'alerte disponibles :**
- `0` = Info (bleu)
- `1` = Warning (orange)
- `2` = Critical (rouge)

## Structure du projet

```
T-Tron-Alert/
├── api/                             # 📡 Backend
│   ├── TTronAlert.Api/              # API REST (.NET 8)
│   │   ├── Controllers/             # Contrôleurs REST
│   │   ├── Hubs/                    # Hubs SignalR
│   │   ├── Services/                # Services métier
│   │   ├── Data/                    # Contexte EF Core
│   │   └── Migrations/              # Migrations de DB
│   │
│   └── TTronAlert.Shared/           # Bibliothèque partagée
│       ├── Models/                  # Modèles de domaine
│       ├── DTOs/                    # Objets de transfert
│       └── Extensions/              # Extensions et helpers
│
├── app/                             # 💻 Client Desktop
│   └── TTronAlert.Desktop/          # Application Avalonia
│       ├── Configuration/           # Classes de configuration
│       ├── Services/                # Services client
│       ├── ViewModels/              # ViewModels MVVM
│       ├── Views/                   # Vues Avalonia
│       ├── Converters/              # Convertisseurs de valeurs
│       ├── Assets/                  # Ressources (icônes, images)
│       └── appsettings.json         # Configuration client
│
├── start-all.bat / start-all.sh     # Démarrage rapide
├── migrate-db.bat / migrate-db.sh   # Migration de DB
└── TTronAlert.sln                   # Solution .NET
```

## Fonctionnalités

- ✅ **Alertes temps réel** via SignalR
- ✅ **Multi-postes** avec ciblage par workstation ID
- ✅ **3 niveaux d'alerte** (Info, Warning, Critical)
- ✅ **Interface moderne** avec Avalonia UI
- ✅ **Notifications toast** pour les alertes
- ✅ **Système de configuration** flexible (appsettings.json)
- ✅ **Reconnexion automatique** en cas de déconnexion
- ✅ **Cross-platform** (Windows, Linux, macOS)
- ✅ **API REST** documentée avec Swagger

## Configuration avancée

### Variables d'environnement

Le client Desktop supporte les variables d'environnement:

```bash
export DOTNET_ENVIRONMENT=Development
export AlertSystem__WorkstationId=poste-2
export AlertSystem__ApiBaseUrl=http://192.168.1.100:62051
```

### Configuration multi-postes

Pour déployer sur plusieurs postes, créez un fichier `appsettings.json` unique pour chaque poste:

**Poste 1:**
```json
{
  "AlertSystem": {
    "WorkstationId": "poste-1",
    "ApiBaseUrl": "http://server-ip:62051"
  }
}
```

**Poste 2:**
```json
{
  "AlertSystem": {
    "WorkstationId": "poste-2",
    "ApiBaseUrl": "http://server-ip:62051"
  }
}
```
