using System;
using System.Collections.Generic;
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
    public class CiInit : IStep
    {

        public string rootToken;
        public Uri vaultUri;
        public string devopInfraPass;
        public string devopAdminPass;

        private readonly IAsk ask;

        public CiInit(IAsk ask)
        {
            this.ask = ask;
        }

        public async Task TestRunOk()
        {
            // log with root token
            IAuthenticationInfo tokenAuthenticationInfo = new TokenAuthenticationInfo(rootToken);
            var client = VaultSharp.VaultClientFactory.CreateVaultClient(vaultUri, tokenAuthenticationInfo);
            
            var policy = await client.GetPolicyAsync("devop-infra");
            StepAssert.IsTrue(policy != null);
        }

        public async Task TestAlreadyRun()
        {
            // log with root token
            IAuthenticationInfo tokenAuthenticationInfo = new TokenAuthenticationInfo(rootToken);
            var client = VaultSharp.VaultClientFactory.CreateVaultClient(vaultUri, tokenAuthenticationInfo);

            var policy = await client.GetPolicyAsync("devop-infra");
            StepAssert.IsTrue(policy == null);
        }

        public async Task Need()
        {   
            await Task.CompletedTask;
        }

        public async Task Ask()
        {   
            this.vaultUri = new Uri(await this.ask.GetValue(CiInitAsk.VaultUri));
            this.rootToken = await this.ask.GetValue(CiInitAsk.RootToken);
            this.devopInfraPass = await this.ask.GetValue(CiInitAsk.DevInfraPassword);
            this.devopAdminPass = await this.ask.GetValue(CiInitAsk.DevAdminPassword);
        }

        public async Task Keep()
        {  
            await Task.CompletedTask;
        }

        public async Task Run()
        {
            // log with root token
            IAuthenticationInfo tokenAuthenticationInfo = new TokenAuthenticationInfo(rootToken);
            var client = VaultSharp.VaultClientFactory.CreateVaultClient(vaultUri, tokenAuthenticationInfo);

            var authenticationBackend = new AuthenticationBackend()
            {
                AuthenticationPath = "auth/userpass",
                BackendType = AuthenticationBackendType.UsernamePassword,
                Description = "userpass"
            };
            await client.EnableAuthenticationBackendAsync(authenticationBackend);

            // create policy devop-infra
            var devopInfraPolicy = new Policy()
            {
                Name = "devop-infra-policy",
                Rules = "path \"secret/infra/*\" { capabilities = [\"create\", \"read\", \"update\", \"delete\", \"list\"]"
            };
            await client.WritePolicyAsync(devopInfraPolicy);

            // create policy devop-admin
            var devopAdminPolicy = new Policy()
            {
                Name = "devop-admin-policy",
                Rules = "path \"secret/admin/*\" { capabilities = [\"create\", \"read\", \"update\", \"delete\", \"list\"]"
            };
            await client.WritePolicyAsync(devopAdminPolicy);


            // create user devop-infra
            await client.WriteSecretAsync("auth/users/" + "devop-infra", new Dictionary<string, object>
                    {
                        { "password", this.devopInfraPass },
                        { "policies", "devop-infra" }
                    });

            // create user devop-admin
            await client.WriteSecretAsync("auth/users/" + "devop-admin", new Dictionary<string, object>
                    {
                        { "password", this.devopInfraPass },
                        { "policies", "devop-admin" }
                    });
        }

        public Task Clean()
        {
            throw new NotImplementedException();
        }
    }
}
