# Cas d'utilisation

Vous savez coder, mais vous devez proposer votre travail à un client, voire héberger son site web.

## Créez un serveur de build

Créez une machine chez un hébergeur cloud, et utilisez des Dockerfile pour builder des images prêtes à être installées

## Créez un serveur de recette

Deployez votre image docker sur une machine munie d'un reverse proxy web.
Enregistrez un domaine ou sous-domaine et le https, à l'aide d'un fournisseur de certificat ou créez votre propre authorité de certification.

## Cryptez vos configurations

Aucune configuration n'est jamais enregistrées en clair.
Celles-ci sont toutes cryptées sur votre serveur de build, et sont accessibles lors du déploiement à l'aide d'un token à utilisation unique.

## Gérez votre recette

Avec la création d'un projet d'intégration continue, créez automatiquement des projets/utilisateurs sur Mantis pour
suivre la recette.

## Répetez le processus pour chaque client, et hébergez tout le code que gère votre entreprise.

Vous pouvez créer autant de configuration de build et d'environnement de deploiement. Installez-les sur le serveur pilote et laissez faire l'intégration continu.

## Reconstituez une infrastructure complète automatiquement.

Toute la configuration est cryptée sur votre machine, ou sur votre intranet. Même envisageant que toute votre cloud disparaisse,
Vous pourriez tout recréer facilement chez un autre hébergeur.