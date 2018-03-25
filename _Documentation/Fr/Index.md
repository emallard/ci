# Introduction à CI

Bienvenue sur le guide d'introduction à CI. Ce guide est le meilleur endroit pour commencer avec CI.

## Qu'est-ce que CI ?

CI est une base de code permettant l'organisation d'une entreprise pour délivrer du code à ses clients.
Elle s'attaque aux problème suivants :

- Hébergement :
    La configuration de base possède un serveur pour les données de votre entreprise, et un serveur qui contiendra les
    applications des clients.
    Le serveur "pilote" permet de builder les projets clients et de proposer des images docker à installer.
    Le serveur "webserver" contient un serveur web et toutes vos applications clients, sous plusieurs versions si besoin:
    recette interne, recette technique, recette utilisateur, homologation, production
    L'hébergement est entièrement simulable sur des vms créées avec virtualbox.
    Les serveurs peuvent aussi être créés sur un cloud réel, tel Gandi.net

- Configuration et mot de passe
    Propose une méthodologie pour crypter les configurations de production du système
    Crypte toutes les configurations des applications clientes: exemples : 
        - user/pass pour envoyer des mails
        - clefs privées d'API : Stripe, GoogleAnalytics, Facebook

- Integration et Deploiement continus
    - build : clone d'un repo git, et lancement d'un docker file de build, publication de l'image sur un dockerRepository privé
    - deploiement: 
        - le serveur web télécharge l'image docker
        - les configurations sont utilisés en demandant au serveur Vault grac à un token unique.

- Gestion de la relation cliente
    Gestion des bugs de recette à l'aide de Mantis
    Si vous êtes l'hébergeur de production, gestion des bugs de production aussi à l'aide de mantis.


## Suite

Voir les cas d'utilisation