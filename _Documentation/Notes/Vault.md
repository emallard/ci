
https://www.vaultproject.io
https://hub.docker.com/_/vault/
https://www.vaultproject.io/downloads.html


// Tuto pur lancer Vaultet y accéder par HTTP API
https://www.katacoda.com/courses/docker-production/vault-secrets

Dev:
``` 
$ docker run --cap-add=IPC_LOCK -d -e 'VAULT_DEV_ROOT_TOKEN_ID=myroottoken' -p 8200:8200 --name=dev-vault vault
```

Accéder à Vault via l'API :
https://www.vaultproject.io/intro/getting-started/apis.html

curl http://localhost:8200/v1/sys/init | json_pp 

Prod:
```
$ docker run --cap-add=IPC_LOCK -e 'VAULT_LOCAL_CONFIG={"backend": {"file": {"path": "/vault/file"}}, "default_lease_ttl": "168h", "max_lease_ttl": "720h"}' vault server
```

Lien:

http://blog.wescale.fr/2017/09/18/tutoriel-vault-de-linfrastructure-as-code-a-lapplication/
