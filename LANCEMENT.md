# Guide de Lancement - T-Tron-Alert

Ce document explique comment utiliser les fichiers batch (.bat) pour lancer et tester l'application T-Tron-Alert.

## Prérequis

- .NET 8.0 SDK installé
- MariaDB ou MySQL installé et en cours d'exécution
- Windows 10/11

## Fichiers Batch Disponibles

### 1. `build.bat` - Compilation de la solution
Compile l'ensemble de la solution (API + Client WPF).

**Utilisation :**
```cmd
build.bat
```

**Quand l'utiliser :**
- Après avoir cloné le projet pour la première fois
- Après avoir modifié du code source
- Pour vérifier que tout compile correctement

---

### 2. `clean.bat` - Nettoyage des fichiers de compilation
Nettoie les fichiers de compilation (bin/, obj/) de la solution.

**Utilisation :**
```cmd
clean.bat
```

**Quand l'utiliser :**
- Avant une compilation complète depuis zéro
- Pour résoudre des problèmes de compilation
- Pour libérer de l'espace disque

---

### 3. `migrate-db.bat` - Migration de la base de données
Applique les migrations Entity Framework Core pour créer/mettre à jour la base de données.

**Utilisation :**
```cmd
migrate-db.bat
```

**Important :**
- Assurez-vous que MariaDB/MySQL est démarré
- Configurez la chaîne de connexion dans `AlertesApi/appsettings.json` ou `appsettings.Development.json`
- Exécutez ce script AVANT de démarrer l'API pour la première fois
- Les outils Entity Framework Core seront automatiquement restaurés si nécessaire

---

### 4. `start-api.bat` - Démarrage de l'API
Lance l'API AlertesApi en mode développement.

**Utilisation :**
```cmd
start-api.bat
```

**Points d'accès :**
- API : http://localhost:5177
- Swagger UI : http://localhost:5177/swagger

**Note :** La fenêtre de commande reste ouverte pendant l'exécution. Fermez-la pour arrêter l'API.

---

### 5. `start-client.bat` - Démarrage du client WPF
Lance l'application client WPF.

**Utilisation :**
```cmd
start-client.bat
```

**Note :** 
- Le client démarre en mode réduit avec une icône dans la barre système
- Assurez-vous que l'API est déjà démarrée avant de lancer le client

---

### 6. `start-all.bat` - Démarrage complet
Lance automatiquement l'API et le client dans des fenêtres séparées.

**Utilisation :**
```cmd
start-all.bat
```

**Avantages :**
- Lance l'API dans une nouvelle fenêtre
- Attend 10 secondes que l'API démarre
- Lance ensuite le client WPF dans une autre fenêtre
- Idéal pour démarrer rapidement l'environnement de développement complet

---

### 7. `test-alert.bat` - Test d'envoi d'une alerte
Envoie une alerte de test à l'API en utilisant curl.

**Utilisation :**
```cmd
test-alert.bat
```

**Prérequis :** curl doit être installé (inclus dans Windows 10+)

**Ce que fait ce script :**
- Envoie une alerte de niveau "Info" via l'API
- Affiche la réponse du serveur
- Permet de vérifier que l'API fonctionne et que les clients reçoivent l'alerte

---

### 8. `test-alerts-all-levels.bat` - Test de tous les niveaux d'alerte
Envoie trois alertes de test avec différents niveaux de priorité.

**Utilisation :**
```cmd
test-alerts-all-levels.bat
```

**Ce que fait ce script :**
- Envoie une alerte de niveau "Info"
- Envoie une alerte de niveau "Avertissement"
- Envoie une alerte de niveau "Critique"
- Permet de tester les différents types de notifications

---

## Scénarios d'Utilisation Typiques

### Première Installation

1. Cloner le projet
2. Configurer la base de données dans `AlertesApi/appsettings.json`
3. Exécuter `build.bat` pour compiler le projet (restaure automatiquement les outils nécessaires)
4. Exécuter `migrate-db.bat` pour créer la base de données
5. Exécuter `start-all.bat` pour lancer l'API et le client
6. Exécuter `test-alert.bat` pour vérifier que tout fonctionne

### Développement Quotidien

1. Exécuter `start-all.bat` pour lancer l'environnement
2. Développer et modifier le code
3. Si modifications du code : relancer `start-all.bat`
4. Tester avec `test-alert.bat` ou `test-alerts-all-levels.bat`

### Tests Rapides

1. Exécuter `start-api.bat` dans une fenêtre
2. Utiliser Swagger UI (http://localhost:5177/swagger) pour tester manuellement
3. OU utiliser `test-alert.bat` pour des tests automatisés

---

## Dépannage

### L'API ne démarre pas
- Vérifiez que MariaDB/MySQL est démarré
- Vérifiez la chaîne de connexion dans `appsettings.json`
- Exécutez `migrate-db.bat` si ce n'est pas déjà fait
- Vérifiez qu'aucune autre application n'utilise le port 5177

### Le client ne reçoit pas les alertes
- Vérifiez que l'API est démarrée
- Vérifiez l'URL du hub SignalR dans le code du client
- Regardez les logs dans la fenêtre de l'API pour détecter les erreurs

### Les scripts batch ne fonctionnent pas
- Assurez-vous d'avoir .NET 8.0 SDK installé (`dotnet --version`)
- Exécutez les scripts en tant qu'administrateur si nécessaire
- Vérifiez que vous êtes dans le bon répertoire (racine du projet)

### curl n'est pas trouvé
- Windows 10/11 inclut curl par défaut
- Si absent, téléchargez depuis https://curl.se/windows/
- Utilisez Swagger UI comme alternative pour tester l'API

---

## Notes Supplémentaires

- Les fichiers batch utilisent l'encodage Windows (CRLF)
- Tous les scripts affichent des messages en français
- Les scripts incluent une pause à la fin pour voir les résultats
- Utilisez Ctrl+C dans une fenêtre de commande pour arrêter un serveur

---

## Support

Pour plus d'informations, consultez le fichier [README.md](README.md) principal du projet.
