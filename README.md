🖥️ Application de gestion en réseau (C# WPF & SQL Server)

Cette application de bureau a été développée en C# avec WPF pour offrir une interface moderne, fluide et réactive.
Elle fonctionne en mode réseau, avec une base de données SQL Server centralisée partagée entre plusieurs postes connectés au même réseau local.

⚙️ Fonctionnalités principales :

🔐 Authentification multi-utilisateurs avec gestion des rôles et permissions.

🧾 Gestion complète des données (ajout, modification, suppression, recherche).

🌐 Connexion réseau : chaque poste client communique avec le serveur SQL via TCP/IP.

💾 Base de données partagée (SQL Server) installée sur un serveur local ou distant.

📊 Tableaux de bord dynamiques avec statistiques et rapports.

📤 Sauvegarde et restauration de la base de données.

🧱 Stack technique :

Langage : C# (.NET Framework / .NET 6 selon la version)

Interface : WPF (Windows Presentation Foundation)

Base de données : Microsoft SQL Server

Architecture : Client-Serveur en réseau local (LAN)

ORM : Entity Framework

🚀 Points forts :

Architecture solide et facilement extensible.

Accès concurrentiel sécurisé à la base de données.

Interface utilisateur ergonomique et professionnelle.

Compatible avec les environnements Windows 10 et 11.

🧰 Utilisation :

Le serveur SQL est hébergé sur une machine principale.

Les clients se connectent via l’adresse IP du serveur dans les paramètres de connexion.

Peut être adapté à tout type de gestion (stock, facturation, point de vente, RH, etc.).
