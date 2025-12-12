# T-Tron-Alert

Système d'alertes en temps réel permettant d'envoyer des alertes à tous les postes connectés via une API et SignalR.

## Architecture

Le projet est composé de deux parties :

### 1. AlertesApi (Backend)
- **Framework**: ASP.NET Core 8.0 Web API
- **Base de données**: MariaDB/MySQL avec Entity Framework Core
- **Communication temps réel**: SignalR
- **Documentation API**: Swagger/OpenAPI

### 2. ClientAlertesWPF (Client Windows)
- **Framework**: WPF .NET 8.0
- **Architecture**: MVVM avec CommunityToolkit.Mvvm
- **Icône système**: Hardcodet.NotifyIcon.Wpf
- **Connexion API**: SignalR Client

## Fonctionnalités

- ✅ Envoi d'alertes via API REST
- ✅ Diffusion en temps réel sur tous les postes connectés via SignalR
- ✅ Client WPF avec icône dans la barre système
- ✅ Notifications Windows (balloon tips)
- ✅ Alertes sonores
- ✅ Support de différents niveaux d'alerte (Info, Avertissement, Critique)

## 🚀 Démarrage Rapide (Windows)

Pour lancer rapidement le projet :
1. Configurer la base de données dans `AlertesApi/appsettings.json`
2. Exécuter `migrate-db.bat` pour créer la base de données
3. Exécuter `start-all.bat` pour lancer l'API et le client
4. Exécuter `test-alert.bat` pour envoyer une alerte de test

Voir [LANCEMENT.md](LANCEMENT.md) pour plus de détails sur tous les fichiers batch disponibles.

## Prérequis

- .NET 8.0 SDK
- MariaDB ou MySQL (pour l'API)
- Windows 10/11 (pour le client WPF)

**Note**: Les outils Entity Framework Core seront automatiquement installés lors de la première compilation ou migration grâce au manifeste d'outils local (`.config/dotnet-tools.json`).

## Configuration

### Base de données

1. Configurer la chaîne de connexion dans `AlertesApi/appsettings.Development.json`:
```json
{
  "ConnectionStrings": {
    "MariaDB": "Server=localhost;Port=3306;Database=systeme_alertes;User=votre_utilisateur;Password=votre_mot_de_passe;"
  }
}
```

2. Appliquer les migrations :
```bash
# Les outils Entity Framework seront restaurés automatiquement
cd AlertesApi
dotnet ef database update
```

Ou utilisez le fichier batch Windows :
```cmd
migrate-db.bat
```

### Client WPF

Modifier l'URL du serveur dans `ClientAlertesWPF/ViewModels/MainViewModel.cs` (ligne 33) si nécessaire :
```csharp
.WithUrl("http://localhost:5177/hubs/alertes")
```

## Compilation

### Compiler toute la solution :
```bash
dotnet build T-Tron-Alert.sln
```

### Compiler uniquement l'API :
```bash
cd AlertesApi
dotnet build
```

### Compiler uniquement le client :
```bash
cd ClientAlertesWPF
dotnet build
```

## Exécution

### 🚀 Méthode rapide (Windows)

Des fichiers batch (.bat) sont disponibles pour simplifier le lancement et les tests :

- **`start-all.bat`** - Lance l'API et le client automatiquement
- **`start-api.bat`** - Lance uniquement l'API
- **`start-client.bat`** - Lance uniquement le client WPF
- **`test-alert.bat`** - Envoie une alerte de test
- **`test-alerts-all-levels.bat`** - Teste tous les niveaux d'alerte
- **`build.bat`** - Compile la solution
- **`clean.bat`** - Nettoie les fichiers de compilation
- **`migrate-db.bat`** - Applique les migrations de base de données

📖 Consultez [LANCEMENT.md](LANCEMENT.md) pour le guide complet d'utilisation des fichiers batch.

### Démarrer l'API (manuel) :
```bash
cd AlertesApi
dotnet run
```

L'API sera disponible sur `http://localhost:5177` (ou le port configuré).
Swagger UI : `http://localhost:5177/swagger`

### Démarrer le client (manuel) :
```bash
cd ClientAlertesWPF
dotnet run
```

Le client démarrera en mode réduit avec une icône dans la barre système.

## Utilisation de l'API

### Envoyer une alerte :
```http
POST http://localhost:5177/api/Alertes
Content-Type: application/json

{
  "titre": "Alerte importante",
  "message": "Ceci est un test d'alerte",
  "niveau": "Critique"
}
```

### Lister toutes les alertes :
```http
GET http://localhost:5177/api/Alertes
```

### Récupérer une alerte spécifique :
```http
GET http://localhost:5177/api/Alertes/1
```

## Structure du projet

```
T-Tron-Alert/
├── AlertesApi/                    # API Backend
│   ├── Controllers/               # Contrôleurs API
│   ├── Data/                      # Contexte Entity Framework
│   ├── Hubs/                      # SignalR Hubs
│   ├── Migrations/                # Migrations EF Core
│   ├── Models/                    # Modèles de données
│   └── Program.cs                 # Point d'entrée
├── ClientAlertesWPF/              # Client WPF
│   ├── ViewModels/                # ViewModels MVVM
│   ├── MainWindow.xaml            # Fenêtre principale
│   └── App.xaml                   # Application WPF
└── T-Tron-Alert.sln              # Solution Visual Studio
```

## Modèles de données

### Alerte
- `Id`: Identifiant unique
- `Titre`: Titre de l'alerte
- `Message`: Message de l'alerte
- `Niveau`: Info / Avertissement / Critique
- `DateCreation`: Date de création
- `EstLue`: Indicateur de lecture
- `EstArchivee`: Indicateur d'archivage
- `PosteIdDestinataire`: ID du poste destinataire (null = tous les postes)

### Poste
- `Id`: Identifiant unique
- `Nom`: Nom du poste
- `TokenUnique`: Token d'identification
- `DerniereConnexion`: Date de dernière connexion

## Sécurité

⚠️ **Important** : Ne pas commiter les fichiers `appsettings.Development.json` contenant des credentials réels en production.

## Licence

Ce projet est un système d'alertes interne.
