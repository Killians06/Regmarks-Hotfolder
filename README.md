# Hotfolder RegMarks

## Description

**Hotfolder RegMarks** est une application Windows en C# conçue pour surveiller un dossier spécifique (hotfolder) et traiter automatiquement les fichiers qui y sont ajoutés. Les fonctionnalités incluent la possibilité de configurer des chaînes de recherche et de remplacement, de choisir les types de fichiers à surveiller, et de gérer les paramètres via une interface utilisateur intuitive.

---

## Fonctionnalités principales

- **Surveillance de dossiers** : Détection automatique des fichiers ajoutés dans un dossier spécifié.
- **Configuration flexible** : Gestion des chemins d'entrée/sortie, type de fichier surveillé, et règles de remplacement via une interface utilisateur.
- **Traitement automatique** : Application de règles de remplacement aux fichiers détectés et sauvegarde dans le dossier de sortie.
- **Interface intuitive** : Interface graphique conviviale pour sélectionner les dossiers, configurer les règles et afficher les journaux.
- **Persistance des paramètres** : Enregistrement et chargement automatique de la configuration utilisateur via un fichier JSON.

---

## Prérequis

- **Système d'exploitation** : Windows
- **Framework** : .NET Framework 4.8 ou version ultérieure
- **Dépendances** : Aucune dépendance externe

---

## Installation

1. Téléchargez l'installeur.
2. Installez l'application dans le répertoire de votre choix
3. Exécutez l'application.

---

## Utilisation

1. Lancez l'application `Hotfolder RegMarks`.
2. **Configurer les dossiers** :
   - Cliquez sur `Choisir le dossier d'entrée` pour sélectionner le dossier à surveiller.
   - Cliquez sur `Choisir le dossier de sortie` pour spécifier où sauvegarder les fichiers traités.
3. **Configurer le type de fichier** :
   - Sélectionnez l'extension des fichiers à surveiller dans le menu déroulant.
4. **Gérer les chaînes de remplacement** :
   - Cliquez sur `Gérer les remplacements` pour ajouter ou modifier les paires de recherche et de remplacement.
5. **Démarrer la surveillance** :
   - Cliquez sur `Démarrer la surveillance` pour surveiller les fichiers.
   - Les fichiers ajoutés seront automatiquement traités et sauvegardés dans le dossier de sortie.
6. **Arrêter la surveillance** :
   - Cliquez sur `Arrêter la surveillance` pour désactiver la surveillance.
7. **Arrêter la surveillance** :
   - Cliquez sur `Surveillance au lancement` pour activer la surveillance automatiquement au prochain lancement de l'application.

---

## Structure du projet

- **MainForm** : La classe principale gérant l'interface utilisateur et la logique de surveillance.
- **ManageWindow** : Fenêtre secondaire pour gérer les chaînes de remplacement.
- **Config** : Classe pour la gestion des paramètres utilisateur (fichiers JSON).

---

## Fonctionnalités à venir

- Support pour des types de fichiers supplémentaires.
- Notifications système lors de la détection et du traitement des fichiers.
- Historique détaillé des opérations.

---

## Auteurs

Ce projet a été développé par Killians Streibel. 

---

## Licence

Ce projet est distribué sous la licence [MIT](LICENSE).

---
