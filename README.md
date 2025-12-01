# BlazorBlog

Application de blog développée en Blazor Server avec authentification ASP.NET Core Identity et stockage des articles en MongoDB.

## 📋 Description

BlazorBlog est une alternative à WordPress permettant de créer et gérer un blog. Les articles sont rédigés en Markdown et stockés dans MongoDB, tandis que l'authentification et la gestion des utilisateurs utilisent ASP.NET Core Identity avec SQL Server.

## 🏗️ Architecture

- **Frontend** : Blazor Server
- **Authentification** : ASP.NET Core Identity avec SQL Server
- **Stockage des articles** : MongoDB
- **Architecture** : Vertical Slice Architecture
- **Gestion d'erreurs** : Pattern ResultOf<T>

## 📦 Prérequis

- .NET 10.0 SDK
- SQL Server 2022 (ou version supérieure) ou SQL Server LocalDB
- MongoDB 7.0 (ou version supérieure)
- Visual Studio 2022 ou VS Code

## 🗄️ Configuration de la base de données SQL Server

### Script de création de la base de données et de l'utilisateur

Exécutez le script suivant dans SQL Server Management Studio (SSMS) ou via `sqlcmd` pour créer la base de données et un utilisateur dédié avec les droits nécessaires :

```sql
/* =========================================================
   Script de création base + utilisateur (SQL Server 2022)
   Base       : BlazorBlog
   Collation  : French_100_CI_AI_SC_UTF8
   Login/User : blazorblog_app (droit db_owner sur BlazorBlog)
   ========================================================= */

/* 1. Création de la base si elle n'existe pas */
IF DB_ID(N'BlazorBlog') IS NULL
BEGIN
    PRINT 'Création de la base BlazorBlog...';
    CREATE DATABASE BlazorBlog
        COLLATE French_100_CI_AI_SC_UTF8;
END
ELSE
BEGIN
    PRINT 'La base BlazorBlog existe déjà.';
END;
GO

/* 2. Création du login serveur si absent */
IF NOT EXISTS (
    SELECT 1 FROM sys.server_principals WHERE name = N'blazorblog_app'
)
BEGIN
    PRINT 'Création du login blazorblog_app...';
    CREATE LOGIN blazorblog_app
        WITH PASSWORD = 'MyStr0ngPassW0rd!',
             CHECK_POLICY = ON,
             CHECK_EXPIRATION = OFF;
END
ELSE
BEGIN
    PRINT 'Le login blazorblog_app existe déjà.';
END;
GO

/* 3. Associer un utilisateur dans la base et lui donner db_owner */
USE BlazorBlog;
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.database_principals WHERE name = N'blazorblog_app'
)
BEGIN
    PRINT 'Création de l''utilisateur blazorblog_app dans BlazorBlog...';
    CREATE USER blazorblog_app FOR LOGIN blazorblog_app WITH DEFAULT_SCHEMA = dbo;
END
ELSE
BEGIN
    PRINT 'L''utilisateur blazorblog_app existe déjà dans BlazorBlog.';
END;
GO

/* 4. Ajouter l'utilisateur au rôle db_owner (tous les droits sur la base) */
IF NOT EXISTS (
    SELECT 1
    FROM sys.database_role_members drm
    JOIN sys.database_principals r ON drm.role_principal_id = r.principal_id
    JOIN sys.database_principals u ON drm.member_principal_id = u.principal_id
    WHERE r.name = N'db_owner' AND u.name = N'blazorblog_app'
)
BEGIN
    PRINT 'Ajout de blazorblog_app au rôle db_owner...';
    ALTER ROLE db_owner ADD MEMBER blazorblog_app;
END
ELSE
BEGIN
    PRINT 'blazorblog_app est déjà membre de db_owner.';
END;
GO

/* 5. Vérifications */
PRINT 'Vérifications :';
SELECT DB_NAME() AS CurrentDB;
SELECT name, collation_name FROM sys.databases WHERE name = N'BlazorBlog';
SELECT name AS UserName, type_desc, default_schema_name
FROM sys.database_principals
WHERE name = N'blazorblog_app';

SELECT r.name AS RoleName, m.name AS MemberName
FROM sys.database_principals r
JOIN sys.database_role_members rm ON r.principal_id = rm.role_principal_id
JOIN sys.database_principals m ON rm.member_principal_id = m.principal_id
WHERE m.name = N'blazorblog_app';
GO
```

### Droits nécessaires

L'utilisateur `blazorblog_app` a le rôle `db_owner` qui lui confère :
- Création, modification et suppression de tables
- Gestion des schémas
- Exécution des migrations Entity Framework Core
- Tous les droits nécessaires pour ASP.NET Core Identity

### Alternative : SQL Server LocalDB (Développement)

Pour le développement local, vous pouvez utiliser SQL Server LocalDB. La chaîne de connexion par défaut dans `appsettings.json` utilise déjà LocalDB :

```
Server=(localdb)\mssqllocaldb;Database=aspnet-BlazorBlog-...
```

Aucune configuration supplémentaire n'est nécessaire avec LocalDB.

## 🍃 Configuration de MongoDB

### Installation

1. **Télécharger MongoDB** : [https://www.mongodb.com/try/download/community](https://www.mongodb.com/try/download/community)

2. **Installer MongoDB** selon votre système d'exploitation

3. **Démarrer MongoDB** :
   - **Windows** : Le service MongoDB démarre automatiquement après l'installation
   - **Linux/Mac** : `sudo systemctl start mongod` ou `brew services start mongodb-community`

### Vérification

Vérifiez que MongoDB est en cours d'exécution :

```bash
# Windows
# Vérifier dans les services Windows ou via PowerShell
Get-Service MongoDB

# Linux/Mac
sudo systemctl status mongod
```

### Configuration de la base de données

MongoDB créera automatiquement la base de données `BlazorBlog` lors de la première utilisation. Aucune configuration manuelle n'est nécessaire.

Par défaut, MongoDB écoute sur `localhost:27017` sans authentification en développement local.

### Configuration avec authentification (Production)

Pour la production, il est recommandé d'activer l'authentification MongoDB :

1. **Créer un utilisateur administrateur** :
```javascript
use admin
db.createUser({
  user: "blazorblog_admin",
  pwd: "MyStr0ngPassW0rd!",
  roles: [ { role: "userAdminAnyDatabase", db: "admin" } ]
})
```

2. **Créer un utilisateur pour l'application** :
```javascript
use BlazorBlog
db.createUser({
  user: "blazorblog_app",
  pwd: "MyStr0ngPassW0rd!",
  roles: [ { role: "readWrite", db: "BlazorBlog" } ]
})
```

3. **Mettre à jour la configuration** dans `appsettings.json` :
```json
{
  "MongoDB": {
    "ConnectionString": "mongodb://blazorblog_app:MyStr0ngPassW0rd!@localhost:27017",
    "DatabaseName": "BlazorBlog"
  }
}
```

**Note** : La chaîne de connexion inclut déjà l'authentification. Le `DatabaseName` est utilisé séparément par l'application.

## ⚙️ Configuration de l'application

### 1. Mettre à jour appsettings.json

Modifiez le fichier `src/BlazorBlog/BlazorBlog/appsettings.json` :

```json
{
  "ConnectionStrings": {
    "Server": "localhost",
    "Database": "BlazorBlog",
    "Login": "blazorblog_app",
    "Password": "MyStr0ngPassW0rd!"
  },
  "MongoDB": {
    "ConnectionString": "mongodb://blazorblog_app:MyStr0ngPassW0rd!@localhost:27017",
    "DatabaseName": "BlazorBlog"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

**Remplacez** :
- `Server` : nom de votre serveur SQL Server (ex: `localhost`, `localhost\\SQLEXPRESS`, ou `(localdb)\\mssqllocaldb`)
- `Login` et `Password` : les identifiants que vous avez définis pour l'utilisateur SQL Server
- Pour MongoDB : `blazorblog_app` et `MyStr0ngPassW0rd!` dans la connection string si vous avez activé l'authentification

### 2. Créer les migrations Entity Framework

Les migrations pour ASP.NET Core Identity seront créées automatiquement au premier démarrage si vous utilisez `UseMigrationsEndPoint()` en développement.

Pour créer manuellement les migrations :

```bash
cd src/BlazorBlog/BlazorBlog
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## 🚀 Démarrage de l'application

1. **Cloner le repository** :
```bash
git clone <url-du-repo>
cd BlazorBlog
```

2. **Restaurer les packages NuGet** :
```bash
cd src/BlazorBlog/BlazorBlog
dotnet restore
```

3. **Vérifier que SQL Server et MongoDB sont démarrés**

4. **Lancer l'application** :
```bash
dotnet run
```

Ou via Visual Studio : appuyez sur `F5`

5. **Accéder à l'application** :
   - L'application sera accessible sur `https://localhost:5001` ou `http://localhost:5000`
   - Créez un compte utilisateur via `/Account/Register`
   - Connectez-vous et créez votre premier article via `/articles/create`

## 📁 Structure du projet

```
BlazorBlog/
├── Core/
│   ├── Models/          # Modèles communs (Article, ResultOf, Errors)
│   └── Logging/         # Logging personnalisé
├── Data/                # ApplicationDbContext, ApplicationUser
├── Features/            # Features en Vertical Slice Architecture
│   ├── Account/         # Authentification (Login, Register, etc.)
│   └── CreateArticle/   # Création d'articles
│       ├── Components/  # Pages Blazor
│       ├── Models/      # ViewModels et InputModels
│       ├── Services/    # Services métier et ViewServices
│       ├── Repository/  # Accès MongoDB
│       └── Configurations/ # Injection de dépendances
└── Components/          # Composants partagés (Layout, etc.)
```

## 🔐 Sécurité

- L'authentification est gérée par ASP.NET Core Identity
- Les articles sont associés à leur auteur via `AuthorId`
- Les pages de création/modification nécessitent une authentification (`[Authorize]`)
- Les mots de passe sont hashés avec les algorithmes sécurisés d'ASP.NET Core Identity

## 📝 Fonctionnalités

- ✅ Authentification et gestion des utilisateurs (ASP.NET Core Identity)
- ✅ Création d'articles en Markdown
- ✅ Stockage des articles dans MongoDB
- ✅ Architecture Vertical Slice
- ✅ Gestion d'erreurs avec pattern ResultOf<T>

## 🛠️ Technologies utilisées

- **.NET 10.0**
- **Blazor Server**
- **ASP.NET Core Identity**
- **Entity Framework Core** (pour l'authentification)
- **MongoDB.Driver** (pour les articles)
- **Bootstrap** (UI)

## 📄 Licence

Voir le fichier [LICENSE](LICENSE) pour plus d'informations.

