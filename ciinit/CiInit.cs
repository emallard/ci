using System;
using System.Threading.Tasks;
using citools;
using VaultSharp;
using VaultSharp.Backends.Authentication.Models;
using VaultSharp.Backends.Authentication.Models.Token;
using VaultSharp.Backends.System.Models;

/*
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
*/

namespace ciinit
{
    public class CiInit
    {

        public string rootToken;
        public Uri vaultUri;

        public CiInit()
        {
        }

        public async Task TestRunOk()
        {
            var client = Client();
            var policy = await client.GetPolicyAsync("devop-infra");
            if (policy == null)
                throw new TestRunException(this);
        }

        public async Task TestAlreadyRun()
        {
            var client = Client();
            var policy = await client.GetPolicyAsync("devop-infra");
            if (policy != null)
                throw new AlreadyRunException(this);
        }

        public void Ask()
        {   
            // devop vault uri
            // root-token
        }

        public void Keep()
        {
            
        }

        public async Task Run()
        {
            var client = Client();
            var authenticationBackend = new AuthenticationBackend()
            {
                AuthenticationPath = "/sys/auth/userpass",
                BackendType = AuthenticationBackendType.UsernamePassword,
                Description = "userpass"
            };
            await client.EnableAuthenticationBackendAsync(authenticationBackend);

            var devopInfraPolicy = new Policy()
            {
                Name = "devop-infra-policy",
                Rules = "path \"secret/infra/*\" { capabilities = [\"create\", \"read\", \"update\", \"delete\", \"list\"]"
            };
            await client.WritePolicyAsync(devopInfraPolicy);

            var devopAdminPolicy = new Policy()
            {
                Name = "devop-admin-policy",
                Rules = "path \"secret/admin/*\" { capabilities = [\"create\", \"read\", \"update\", \"delete\", \"list\"]"
            };
            await client.WritePolicyAsync(devopAdminPolicy);
        }

        public IVaultClient Client()
        {
            IAuthenticationInfo tokenAuthenticationInfo = new TokenAuthenticationInfo(rootToken);
            return VaultSharp.VaultClientFactory.CreateVaultClient(vaultUri, tokenAuthenticationInfo);
        }
    }
}
