# citest

A test is a Program and not a XUnit test.

It's made of multiple steps. Each steps can be long so thegoal is to not do everything each time.

So each Step is made of 3 methods :
- Test : Does the step need to be done ?
- Run : Do it
- Clean : Delete what is needed, so that we can really test the step with Run.

Loop to run tests:
``` cs
try {
    step.Test();
}
catch (Exception)
{
    step.Clean();
    step.Run();
    step.Test();
}
```

# Examples of tests :

- VmPilote_1_Create
- VmPilote_1_Hosts
- VmPilote_2_Docker
- VmPilote_3_MirrorRegistry
- VmPilote_5_PiloteCi_Build
- VmPilote_4_PiloteCi_Sources

- PiloteCi_1_InstallCA
- PiloteCi_3_InstallPrivateRegistry
- PiloteCi_1_Build
- PiloteCi_2_Publish


# Passwords and settings

During this bash session, disable history:
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

Start vault locally.
docker start dev-vault

if not yet installed : install vault locally.
```
docker run --cap-add=IPC_LOCK -d -e 'VAULT_LOCAL_CONFIG={"backend": {"file": {"path": "/vault/file"}}, "default_lease_ttl": "12h", "max_lease_ttl": "12h", "listener": {"tcp": {"address": "127.0.0.1:8200", "tls_disable": "true"}}}' -e 'VAULT_ADDR=http://127.0.0.1:8200' -p 127.0.0.1:8200:8200 --name=dev-vault vault server
```
Unseal
```
docker exec dev-vault vault operator init
docker exec -it dev-vault vault operator unseal
docker exec -it dev-vault vault operator unseal
docker exec -it dev-vault vault operator unseal
docker exec dev-vault vault auth enable userpass
```

enable user/pass authentication
```
curl \
    --header "X-Vault-Token: ..." \
    --request POST \
    --data '{"type": "userpass"}' \
    http://127.0.0.1:8200//v1/sys/auth/userpass
```

add ci-policy
```
curl \
    --header "X-Vault-Token: ..." \
    --request PUT \
    --data '{"policy": "path \"secret/ci/*\" { capabilities = [\"create\", \"read\", \"update\", \"delete\", \"list\"] }"}' \
    http://127.0.0.1:8200/v1/sys/policy/ci-policy
```
add user/pass for ci

curl \
    --header "X-Vault-Token: ..." \
    --request POST \
    --data @- \
    http://127.0.0.1:8200/v1/auth/userpass/users/ci

# fill stdin with {"password": "superSecretPassword", "policies": "ci-policy,default" } Ctrl+D

curl \
    --request POST \
    --data @- \
    http://127.0.0.1:8200/v1/auth/userpass/login/ci

# fill stdin with {"password": "superSecretPassword"} Ctrl+D
``` 
Keep the "client-token" : part
And use it to create a secret :
```
curl \
    --header "X-Vault-Token: ..." \
    --request POST \
    --data '{"foo" : "bar"}'\
    http://127.0.0.1:8200/v1/secret/ci/foo
```
and readt it
```
curl \
    --header "X-Vault-Token: ..." \
    http://127.0.0.1:8200/v1/secret/ci/foo
```