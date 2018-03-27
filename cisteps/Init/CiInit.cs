using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using citools;

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

namespace cisteps
{
    public class CiInit : IStep
    {
        private readonly IAsk ask;
        private readonly IStoreResolver storeResolver;

        public CiInit(
            IAsk ask,
            IStoreResolver storeResolver
        )
        {
            this.ask = ask;
            this.storeResolver = storeResolver;
        }

        public async Task CheckRunOk()
        {
            // log with root token
            var rootToken = await this.ask.GetValue(ListAsk.RootToken);
            var client = storeResolver.CreateClient("vault", new TokenAuthenticationInfo(rootToken));
            var policy = await client.GetPolicyAsync("devop");
            StepAssert.IsTrue(policy != null);
        }

        public async Task TestAlreadyRun()
        {
            // log with root token
            var rootToken = await this.ask.GetValue(ListAsk.RootToken);
            var client = storeResolver.CreateClient("vault", new TokenAuthenticationInfo(rootToken));
            var policy = await client.GetPolicyAsync("devop");
            StepAssert.IsTrue(policy == null);
        }

        public async Task Run()
        {   
            var vaultUri = new Uri(await this.ask.GetValue(ListAsk.VaultUri));
            var rootToken = await this.ask.GetValue(ListAsk.RootToken);
            var devopPass = await this.ask.GetValue(ListAsk.DevopPassword);
        

            // log with root token
            var client = storeResolver.CreateClient("vault", new TokenAuthenticationInfo(rootToken));
            await client.EnableUsernamePassword();

            // create devop-policy
            var devopAdminPolicy = new Policy()
            {
                Name = "devop-policy",
                Rules = "path \"secret/devop/*\" { capabilities = [\"create\", \"read\", \"update\", \"delete\", \"list\"]"
            };
            await client.WritePolicyAsync(devopAdminPolicy);

            // create user devop
            await client.WriteUser("devop", devopPass,"devop-policy");
        }

        public Task Clean()
        {
            throw new NotImplementedException();
        }
    }
}
