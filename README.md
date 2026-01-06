# TierList - Projet d'Architecture Hexagonale en C# .NET

Application Web de classement de logos d'entreprises en tier liste, développée en architecture hexagonale (Clean Architecture) avec C# .NET 9.

## 📋 Description du Projet

Cette application permet aux utilisateurs de :
- S'inscrire et se connecter (avec JWT)
- Classer des logos d'entreprise dans 5 catégories (S, A, B, C, D)
- Exporter leur tier liste en PDF (stocké dans MinIO S3)
- Paiement via Stripe Sandbox (optionnel - bonus)

Les administrateurs peuvent ajouter des logos d'entreprise via l'API REST (max 10 logos).

## 🏗️ Architecture

Le projet suit les principes de **Clean Architecture / Architecture Hexagonale** :

```
TierList.Solution/
├── src/
│   ├── TierList.Domain/              # Couche Domaine
│   │   ├── Entities/                 # Entités métier
│   │   ├── ValueObjects/             # Value Objects
│   │   ├── Enums/                    # Enumerations
│   │   └── Common/                   # Classes de base
│   │
│   ├── TierList.Application/         # Couche Application
│   │   ├── UseCases/                 # Cas d'utilisation
│   │   ├── DTOs/                     # Data Transfer Objects
│   │   └── Ports/                    # Interfaces (Ports)
│   │       ├── Repositories/         # Interfaces des repositories
│   │       └── Services/             # Interfaces des services
│   │
│   ├── TierList.Infrastructure/      # Couche Infrastructure
│   │   ├── Persistence/              # Configuration EF Core
│   │   ├── Repositories/             # Implémentations des repositories
│   │   └── Services/                 # Implémentations des services
│   │
│   └── TierList.WebAPI/              # Couche Présentation
│       ├── Controllers/              # Contrôleurs REST
│       └── Program.cs                # Configuration de l'application
│
└── tests/                            # Tests unitaires et d'intégration
```

### Dépendances entre les couches

```
WebAPI → Infrastructure → Application → Domain
        ↓
    (Adapters)
```

- **Domain** : Aucune dépendance externe (Pure Business Logic)
- **Application** : Dépend uniquement du Domain
- **Infrastructure** : Implémente les ports définis dans Application
- **WebAPI** : Orchestre tout et configure l'injection de dépendances

## 🚀 Technologies Utilisées

- **.NET 9** (Framework principal)
- **ASP.NET Core Web API** (API REST)
- **Entity Framework Core 9** (ORM)
- **SQLite** (Base de données)
- **JWT Bearer** (Authentification)
- **BCrypt.Net** (Hachage de mots de passe)
- **MinIO** (Stockage S3 pour les PDFs)
- **QuestPDF** (Génération de PDFs)
- **Swashbuckle** (Documentation Swagger)
- **Logo.dev API** (Récupération des logos d'entreprises)

## ⚙️ Configuration

### 1. Prérequis

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Docker](https://www.docker.com/) (pour MinIO)
- Un compte [Logo.dev](https://www.logo.dev/signup) pour l'API Key

### 2. Configuration MinIO

Lancer MinIO avec Docker :

```bash
docker run -p 9000:9000 -p 9001:9001 \
  -e "MINIO_ROOT_USER=minioadmin" \
  -e "MINIO_ROOT_PASSWORD=minioadmin" \
  quay.io/minio/minio server /data --console-address ":9001"
```

Interface MinIO disponible sur : http://localhost:9001

### 3. Configuration de l'application

Modifier le fichier `src/TierList.WebAPI/appsettings.json` :

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=tierlist.db"
  },
  "Jwt": {
    "Secret": "VotreClefSecreteDe32CaracteresMinimum!",
    "Issuer": "TierListAPI",
    "Audience": "TierListClient",
    "ExpirationMinutes": 60
  },
  "LogoApi": {
    "ApiKey": "votre-cle-api-logo-dev"
  },
  "Minio": {
    "Endpoint": "localhost:9000",
    "AccessKey": "minioadmin",
    "SecretKey": "minioadmin",
    "BucketName": "tierlist-pdfs"
  }
}
```

## 🏃 Démarrage de l'application

### 1. Restaurer les packages

```bash
dotnet restore
```

### 2. Lancer l'application

```bash
dotnet run --project src/TierList.WebAPI/TierList.WebAPI.csproj
```

L'API sera disponible sur :
- HTTPS : https://localhost:7000
- HTTP : http://localhost:5000

### 3. Accéder à Swagger

Documentation Swagger disponible sur : https://localhost:7000/swagger

## 📡 API Endpoints

### Authentification

#### POST /api/auth/register
Inscription d'un nouvel utilisateur

**Request Body:**
```json
{
  "email": "user@example.com",
  "password": "MotDePasse123!",
  "username": "username"
}
```

**Response:** `200 OK`
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "email": "user@example.com",
  "username": "username",
  "isAdmin": false
}
```

#### POST /api/auth/login
Connexion d'un utilisateur

**Request Body:**
```json
{
  "email": "user@example.com",
  "password": "MotDePasse123!"
}
```

**Response:** `200 OK` (même structure que register)

### Logos d'entreprise

#### POST /api/companylogo
Ajouter un logo d'entreprise (Admin uniquement)

**Request Body:**
```json
{
  "companyName": "Google",
  "domain": "google.com"
}
```

**Responses:**
- `201 Created` : Logo ajouté avec succès
- `400 Bad Request` : Maximum de 10 logos atteint
- `409 Conflict` : Logo déjà existant

#### GET /api/companylogo
Récupérer tous les logos (Authentification requise)

**Response:** `200 OK`
```json
[
  {
    "id": "guid",
    "companyName": "Google",
    "domain": "google.com",
    "logoUrl": "https://img.logo.dev/google.com?token=..."
  }
]
```

### Tier Liste

#### PUT /api/tierlist
Mettre à jour sa tier liste (Authentification requise)

**Request Body:**
```json
{
  "items": {
    "logo-guid-1": 1,
    "logo-guid-2": 2,
    "logo-guid-3": 5
  }
}
```

**Response:** `200 OK`
```json
{
  "id": "guid",
  "userId": "guid",
  "isPaid": false,
  "pdfUrl": null,
  "items": [
    {
      "id": "guid",
      "companyLogo": { },
      "rank": 1
    }
  ]
}
```

#### POST /api/tierlist/export
Exporter sa tier liste en PDF (Authentification requise + Paiement)

**Response:** `200 OK`
```json
{
  "pdfUrl": "tierlist-pdfs/tierlist-userid-20260106123456.pdf"
}
```

## 🎨 Catégories de Tier List

| Rank | Description | Couleur (PDF) |
|------|-------------|---------------|
| S | Les chefs-d'œuvre du branding | Rouge |
| A | Très bons logos | Orange |
| B | Ça passe | Jaune |
| C | Médiocres | Vert |
| D | Les flops visuels | Bleu |

## 🏛️ Principes de Clean Architecture Appliqués

### 1. Indépendance des frameworks
Le domaine métier ne dépend d'aucun framework externe.

### 2. Testabilité
Chaque couche peut être testée indépendamment grâce aux interfaces (ports).

### 3. Indépendance de la UI
L'API peut être remplacée par une autre interface sans toucher au métier.

### 4. Indépendance de la base de données
Changement de SQLite vers PostgreSQL ? Seule l'Infrastructure change.

### 5. Règle de dépendance
Les dépendances pointent toujours vers l'intérieur (vers le Domain).

## 📦 Diagramme de Classes UML

Les entités principales :

```
┌─────────────┐
│    User     │
├─────────────┤
│ + Id        │
│ + Email     │
│ + Username  │
│ + Password  │
│ + IsAdmin   │
└─────────────┘
       │ 1
       │
       │ *
┌──────────────┐
│UserTierList  │
├──────────────┤
│ + Id         │
│ + UserId     │
│ + IsPaid     │
│ + PdfUrl     │
└──────────────┘
       │ 1
       │
       │ *
┌──────────────┐       *  ┌──────────────┐
│TierListItem  ├──────────│ CompanyLogo  │
├──────────────┤       1  ├──────────────┤
│ + Id         │          │ + Id         │
│ + Rank       │          │ + Name       │
│              │          │ + Domain     │
└──────────────┘          │ + LogoUrl    │
                          └──────────────┘
```

## 🔐 Sécurité

- Les mots de passe sont hachés avec BCrypt
- Authentification JWT avec token expiration
- Validation des données d'entrée
- CORS configuré (à adapter en production)

## 📝 Notes pour le Développement

### Créer une migration EF Core

```bash
dotnet ef migrations add InitialCreate --project src/TierList.Infrastructure --startup-project src/TierList.WebAPI
```

### Appliquer les migrations

```bash
dotnet ef database update --project src/TierList.Infrastructure --startup-project src/TierList.WebAPI
```

### Ajouter un nouveau Use Case

1. Créer l'interface dans `Application/Ports` si nécessaire
2. Créer le Use Case dans `Application/UseCases`
3. Implémenter les adapters dans `Infrastructure`
4. Enregistrer dans `WebAPI/Program.cs`
5. Créer le controller dans `WebAPI/Controllers`

## 🎯 Fonctionnalités Bonus (À implémenter)

### Intégration Stripe Sandbox

Pour activer le paiement Stripe :

1. Créer un compte [Stripe](https://stripe.com/)
2. Récupérer les clés API de test
3. Implémenter `IPaymentService` dans Infrastructure
4. Ajouter l'endpoint de paiement dans le controller

## 📚 Ressources

- [Clean Architecture par Robert Martin](https://blog.cleancoder.com/)
- [Architecture Hexagonale par Alistair Cockburn](https://alistair.cockburn.us/hexagonal-architecture/)
- [Documentation .NET](https://docs.microsoft.com/dotnet/)
- [Entity Framework Core](https://docs.microsoft.com/ef/core/)
- [Logo.dev API](https://www.logo.dev/)

## 👥 Contributeurs

Projet réalisé dans le cadre du TP "Tier Listes" - Architecture Hexagonale

## 📄 Licence

Ce projet est à but éducatif.
