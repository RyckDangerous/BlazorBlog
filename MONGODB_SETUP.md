# Configuration MongoDB pour BlazorBlog

## Problème
L'utilisateur MongoDB n'a pas les droits nécessaires pour accéder à la base de données `BlazorBlog`.

## Solution : Créer l'utilisateur avec les droits appropriés

### Option 1 : Via MongoDB Shell (mongosh)

1. **Connectez-vous à MongoDB en tant qu'administrateur** :
```bash
mongosh -u admin -p --authenticationDatabase admin
```

2. **Créez l'utilisateur avec les droits sur la base BlazorBlog** :
```javascript
use BlazorBlog

db.createUser({
  user: "blazorblog_user",
  pwd: "MyM0ng0PassW0rd!",
  roles: [
    { role: "readWrite", db: "BlazorBlog" }
  ]
})
```

**Important** : La chaîne de connexion dans `appsettings.Development.json` doit inclure `authSource=BlazorBlog` :
```json
"ConnectionString": "mongodb://blazorblog_user:MyM0ng0PassW0rd!@localhost:27017/BlazorBlog?authSource=BlazorBlog"
```

### Option 2 : Via MongoDB Compass

1. Connectez-vous à MongoDB Compass avec un compte administrateur
2. Allez dans l'onglet "Users" de la base de données `BlazorBlog`
3. Cliquez sur "Add Database User"
4. Remplissez :
   - Username: `blazorblog_user`
   - Password: `MyM0ng0PassW0rd!`
   - Database User Privileges: Sélectionnez "Read and write to any database" ou "Custom role"
   - Si Custom role, sélectionnez le rôle `readWrite` pour la base `BlazorBlog`

### Option 3 : Si MongoDB n'a pas d'authentification activée (développement local)

Si votre MongoDB local n'a pas d'authentification activée, vous pouvez :

1. **Modifier la chaîne de connexion** dans `appsettings.Development.json` :
```json
"MongoDB": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "BlazorBlog"
}
```

2. **OU activer l'authentification et créer l'utilisateur** (recommandé pour la sécurité)

## Vérification

Après avoir créé l'utilisateur, la base de données `BlazorBlog` sera automatiquement créée lors du premier insert d'un article.

Vous pouvez vérifier dans MongoDB Compass que :
- La base de données `BlazorBlog` existe
- L'utilisateur `blazorblog_user` a les droits `readWrite` sur cette base

## Note importante

MongoDB crée automatiquement les bases de données et les collections lors du premier insert. Vous n'avez pas besoin de les créer manuellement. L'important est que l'utilisateur ait les droits nécessaires.

