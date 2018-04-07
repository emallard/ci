# Installation du stockage crypté de votre configuration.

La configuration de votre système est enregistrée dans un serveur Vault, sur votre machine, ou déportée dans l'intranet, comme vous le souhaitez:

## But de l'installation

- installer un serveur vault
- obtenir les clés du serveur.
- installer un utilisateur avec les privilèges devop-infra qui pourra manipuler le cloud.
- installer un utilisateur avec les privilèges devop-admin qui pourra accéder en SSH aux VMs et faire toutes les installations requises

## Installer le serveur Vault à l'aide de ci-bootstrap

```
ci bootstrap
devop-infra user ?
devop-infra password ?
devop-admin user ?
devop-admin password ?
```


## Installer le serveur Vault à l'aide de commandes shell

Désactivez l'historique bash pour ne pas conservez des tokens en ligne de commande
```
export HISTFILE=/dev/null
ou
unset HISTFILE
```
Or start all commands with space : 
```
echo $HISTCONTROL
```
Check that it returns ignorespace or ignoreboth

Pour un serveur sur votre machine, et qui ne sera accessible que par 127.0.0.1:8200
```
docker run --cap-add=IPC_LOCK -d -e 'VAULT_LOCAL_CONFIG={"backend": {"file": {"path": "/vault/file"}}, "default_lease_ttl": "12h", "max_lease_ttl": "12h", "listener": {"tcp": {"address": "127.0.0.1:8200", "tls_disable": "true"}}}' -e 'VAULT_ADDR=http://127.0.0.1:8200' -p 127.0.0.1:8200:8200 --name=dev-vault vault server
```

Init
```
docker exec dev-vault vault operator init
```
=> Conservez votre root-token à l'abri et vos 5 clés. 

Unseal
```
docker exec -it dev-vault vault operator unseal
docker exec -it dev-vault vault operator unseal
docker exec -it dev-vault vault operator unseal
# docker exec dev-vault vault auth enable userpass
```

Enable user/pass authentication
```
curl \
    --header "X-Vault-Token: ..." \
    --request POST \
    --data '{"type": "userpass"}' \
    http://127.0.0.1:8200/v1/sys/auth/userpass
```

Création d'une policy devop-infra

Création d'une policy devop-admin

```
curl \
    --header "X-Vault-Token: ..." \
    --request PUT \
    --data '{"policy": "path \"secret/ci/*\" { capabilities = [\"create\", \"read\", \"update\", \"delete\", \"list\"] }"}' \
    http://127.0.0.1:8200/v1/sys/policy/ci-policy
```
add user/pass for ci

```
curl \
    --header "X-Vault-Token: ..." \
    --request POST \
    --data @- \
    http://127.0.0.1:8200/v1/auth/userpass/users/ci

# fill stdin with {"password": "superSecretPassword", "policies": "ci-policy,default" } Ctrl+D
```

```
curl \
    --request POST \
    --data @- \
    http://127.0.0.1:8200/v1/auth/userpass/login/ci

# fill stdin with {"password": "superSecretPassword"} Ctrl+D
```