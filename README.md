## 📦 Gestion de stocks – Back Office

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=.net)
![Angular](https://img.shields.io/badge/Angular-17-DD0031?logo=angular)
![SQL Server](https://img.shields.io/badge/SQL%20Server-2019-CC2927?logo=microsoft-sql-server)
![License](https://img.shields.io/badge/license-MIT-green)

> Application complète de gestion de stocks avec architecture hexagonale, CQRS, DDD, notifications temps réel et interface moderne.

---

## 📌 Choix techniques et hypothèses

- **Architecture** : Clean Architecture (hexagonale) avec séparation Domain / Application / Infrastructure / API.
- **CQRS** : implémenté via **MediatR** (séparation commandes/requêtes).
- **DDD** : utilisation d’agrégats (`Article`), de value objects (`ReferenceEAN13`, `Prix`), d’événements de domaine (`StockUpdatedEvent`).
- **Base de données** : SQL Server (Code First avec EF Core) – index unique sur la référence EAN13.
- **Frontend** : Angular 17 (standalone components) + Angular Material pour une UI moderne et responsive.
- **Temps réel** : SignalR pour notifier tous les clients lors d’une mise à jour de stock.
- **Messagerie asynchrone** : Kafka (producteur) – les événements de mise à jour de stock sont publiés (consommateur non implémenté mais possible).
- **Hypothèses** :
  - Une référence EAN13 est unique et au format 13 chiffres.
  - Le prix HT doit être strictement positif.
  - Un approvisionnement se fait avec une quantité ≥ 1.
  - Un inventaire peut ramener la quantité à zéro (mais pas négative).
  - La DLC est obligatoire pour les articles alimentaires.
  - Le stock vendable est simplement la quantité en stock actuelle (pas de notion de seuil ou de réservation).

---

## 🤖 IA utilisée

**ChatGPT (GPT-5 mini)** 

## 💬 Usages de l’IA


- Assistance pour l’écriture des handlers, des commandes/queries MediatR.
- Débogage et correction des erreurs (ex: `HasConversion`).
- Rédaction du README .


---

## ⏱️ Temps passé

Environ **12 heures** réparties sur plusieurs sessions (conception, développement, tests, rédaction de la documentation).

---

## ✨ Aperçu

| Module | Fonctionnalités |
|--------|----------------|
| 📋 **Articles** | CRUD complet, pagination, recherche, validation (référence unique, prix >0, DLC obligatoire) |
| 📥 **Approvisionnement** | Ajout de quantité positive, historique tracé |
| 🔄 **Inventaire** | Ajustement du stock réel, enregistrement des écarts |
| 📜 **Historique** | Mouvements paginés, triables, filtrables (référence/commentaire) |
| 📎 **Export CSV** | Export des inventaires (avec détails article) |
| 🔔 **Notifications** | Mise à jour temps réel via SignalR |
| 🐘 **Kafka** | Événements asynchrones (producteur) |

---

## 🧱 Architecture


┌─────────────────────────────────────────────────────────┐
│ API (ASP.NET Core) │
│ Controllers + Middlewares + CORS + Swagger │
└─────────────────────────────────────────────────────────┘
│
┌─────────────────────────────────────────────────────────┐
│ Application (CQRS + MediatR) │
│ Commands / Queries / Handlers / DTOs / Behaviors │
└─────────────────────────────────────────────────────────┘
│
┌─────────────────────────────────────────────────────────┐
│ Domain (DDD) │
│ Entities / Value Objects / Enums / Events / Interfaces │
└─────────────────────────────────────────────────────────┘
│
┌─────────────────────────────────────────────────────────┐
│ Infrastructure │
│ EF Core (SQL Server) / Kafka Producer / SignalR Hub │
└─────────────────────────────────────────────────────────┘


- **Clean Architecture** : séparation stricte des couches.
- **CQRS** : via MediatR (commandes/requêtes).
- **DDD** : agrégats, value objects, événements de domaine.
- **SignalR** : notifications temps réel.
- **Kafka** : événements asynchrones (optionnel).

---

## 🛠️ Technologies

### Backend
- ![.NET](https://img.shields.io/badge/.NET-8.0-512BD4) – Framework principal
- ![EF Core](https://img.shields.io/badge/EF%20Core-8.0-512BD4) – ORM
- ![MediatR](https://img.shields.io/badge/MediatR-12.2.0-blue) – CQRS
- ![SignalR](https://img.shields.io/badge/SignalR-8.0-512BD4) – Temps réel
- ![Kafka](https://img.shields.io/badge/Kafka-2.3.0-black) – Messagerie
- ![Swagger](https://img.shields.io/badge/Swagger-OpenAPI-green) – Documentation

### Frontend
- ![Angular](https://img.shields.io/badge/Angular-17-DD0031) – Framework
- ![Angular Material](https://img.shields.io/badge/Material-17-3F51B5) – UI
- ![TypeScript](https://img.shields.io/badge/TypeScript-5.2-3178C6) – Langage
- ![RxJS](https://img.shields.io/badge/RxJS-7.8-B7178C) – Réactivité

### Base de données
- ![SQL Server](https://img.shields.io/badge/SQL%20Server-2019-CC2927) – Persistance
- Index unique sur `Reference` (EAN13)

---

## 🚀 Installation & Exécution

### Prérequis
- [.NET 8 SDK](https://dotnet.microsoft.com/)
- [Node.js 18+](https://nodejs.org/)
- [SQL Server](https://www.microsoft.com/sql-server) (LocalDB / Express / full)
- [Git](https://git-scm.com/) (optionnel)

1️⃣ Cloner le dépôt

git clone https://github.com/seif2018/StockManagement.git
cd StockManagement

2️⃣ Configurer la base de données
Modifiez src/API/appsettings.json :

json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=StockManagementDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}

3️⃣ Appliquer les migrations
bash
dotnet restore
dotnet ef database update --startup-project src/API --project src/Infrastructure

4️⃣ Lancer l’API
bash
cd src/API
dotnet run
L’API est disponible sur https://localhost:5001 – Swagger

5️⃣ Lancer le frontend
Ouvrez un nouveau terminal :

bash
cd frontend
npm install
ng serve   # ou npx ng serve
L’application Angular tourne sur http://localhost:4200

6️⃣ (Optionnel) Démarrer Kafka
bash
docker run -p 9092:9092 apache/kafka:latest

---

## 📖 Utilisation

Étape	Action
1️⃣	Créer un article : formulaire « Nouvel article » (référence 13 chiffres, nom, prix HT >0). Pour les articles alimentaires, la DLC est obligatoire.
2️⃣	Approvisionner : saisir une référence existante et une quantité positive.
3️⃣	Inventaire : saisir une référence et la nouvelle quantité réelle (≥0). L’écart est enregistré.
4️⃣	Liste des articles : affichage paginé, possibilité de modifier/supprimer.
5️⃣	Historique : consulter tous les mouvements (tri, filtre, export CSV).
6️⃣	Export inventaires : télécharger un fichier CSV détaillé (ancienne/nouvelle quantité, écart, infos article).

---

## 📂 Structure du projet (abrégée)

StockManagement/
├── src/
│   ├── Domain/               # Entités, Value Objects, Events
│   ├── Application/          # Commandes, Queries, Handlers, DTOs
│   ├── Infrastructure/       # DbContext, Repositories, Kafka, SignalR
│   └── API/                  # Controllers, Program.cs
└── frontend/
    ├── src/app/
    │   ├── article/          # Composant de gestion des articles
    │   ├── history/          # Composant d’historique
    │   ├── services/         # Services HTTP & SignalR
    │   └── edit-article-dialog/
    └── ...

---

## 🔌 Endpoints API principaux

Méthode	URL	Description
GET	/api/articles/paged	Liste paginée des articles
POST	/api/articles	Créer un article
PUT	/api/articles/{reference}	Modifier un article
DELETE	/api/articles/{reference}	Supprimer un article
POST	/api/articles/approvisionner	Approvisionner
POST	/api/articles/inventaire	Faire un inventaire
GET	/api/mouvements/paged	Historique paginé
GET	/api/inventaires/export	Export CSV des inventaires

---

## 🧪 Tests & Qualité

Swagger : https://localhost:5001/swagger

Logs : console de l’API (niveau Information)

Validations : FluentValidation (backend) + Reactive Forms (frontend)

## 📌 Règles métier clés
✅ La référence EAN13 est unique (contrainte base + validation).

✅ Prix HT > 0.

✅ Quantité d’approvisionnement ≥ 1.

✅ Nouvelle quantité (inventaire) ≥ 0.

✅ DLC obligatoire pour les articles alimentaires.

✅ Le stock vendable = quantité en stock.

---

## 🔮 Améliorations possibles


Authentification JWT.

Dashboard statistique.

Mode hors ligne / PWA.

---

## 📄 Licence
MIT © 2026 – Projet de démonstration Développé dans le cadre d’un exercice technique – Architecture hexagonale, CQRS, DDD.

---

##👥 Auteurs

**Seifeddine Trabelsi**  
Email : seifeddin.trabelsi@gmail.com


