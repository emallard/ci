# Etapes manuelles pour installer un serveur web

Installer sur son propre poste en local Vault.

Créer dans l'infrastructure le serveur WebServer.

Enregistre l'IP / le user et le password dans Vault /secret/devop/webserver/ip (/user , /password)

Configurer le firewall du serveur
https://www.digitalocean.com/community/tutorials/how-to-setup-a-firewall-with-ufw-on-an-ubuntu-and-debian-cloud-server
sudo ufw default deny incoming
sudo ufw default allow outgoing
sudo ufw allow ssh
sudo ufw allow www 
(or sudo ufw allow 80/tcp)
sudo ufw allow ftp 
(or sudo ufw allow 21/tcp)

sudo ufw status verbose


Installer Docker sur le serveur

Crer un repertoire /cidata avec les permissions pour user

Installer Traefik, avec un volume vers /etc/cidata/traefik
docker run -d -p 8080:8080 -p 80:80 -v /etc/cidata/traefik:/etc/traefik --name=dev-traefik traefik:1.5;

Ecrire dans /etc/cidata/traefik/rules.toml traefik.toml

Builder mantisbt
Génerer un root password, user / password pour mysql,
Configurer mysql pour utiliser le fichier de données dans cidata/mantisbt/mysql
Lancer le dockercompose avec les bonnes variables
Enregistrer ces variables dans le vault :

/secret/mantisbt/mysql-root-password
/secret/mantisbt/mysql-user
/secret/mantisbt/mysql-password
/secret/mantisbt/mantis-admin-user
/secret/mantisbt/mantis-admin-password
/secret/mantisbt/mantis-admin-password
/secret/mantisbt/mantis-smtp-username
/secret/mantisbt/mantis-smtp-password

Puis lancer docker-compose-vault X-token:... docker-compose.yml

$ firefox http://localhost:8989/admin/install.php
>>> username: administrator
>>> password: root
==================================================================================
Installation Options
==================================================================================
Type of Database                                        MySQL/MySQLi
Hostname (for Database Server)                          mysql
Username (for Database)                                 mantisbt
Password (for Database)                                 mantisbt
Database name (for Database)                            bugtracker
Admin Username (to create Database if required)         root
Admin Password (to create Database if required)         root
Print SQL Queries instead of Writing to the Database    [ ]
Attempt Installation                                    [Install/Upgrade Database]
==================================================================================

$g_phpMailer_method = PHPMAILER_METHOD_SMTP;
$g_administrator_email = 'admin@example.org';
$g_webmaster_email = 'webmaster@example.org';
$g_return_path_email = 'mantisbt@example.org';
$g_from_email = 'mantisbt@example.org';
$g_smtp_host = 'smtp.example.org';
$g_smtp_port = 25;
$g_smtp_connection_mode = 'tls';
$g_smtp_username = 'mantisbt';
$g_smtp_password = '********';
