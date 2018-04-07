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
    public class DevOpConfigureVault : IStep
    {
        private readonly ListAsk listAsk;
        private readonly IStoreResolver storeResolver;
        private readonly Installer installer;
        private readonly IVaultSealKeys vaultSealKeys;

        public DevOpConfigureVault(
            ListAsk listAsk,
            IStoreResolver storeResolver,
            Installer installer,
            IVaultSealKeys vaultSealKeys
        )
        {
            this.listAsk = listAsk;
            this.storeResolver = storeResolver;
            this.installer = installer;
            this.vaultSealKeys = vaultSealKeys;
        }

        public async Task Check()
        {
            // Check that devopuser can write and read a secret
            var devopUser = await listAsk.LocalVaultDevopUser.Ask();
            var devopPass = await listAsk.LocalVaultDevopPassword.Ask();

            var client = storeResolver.CreateClient("vault", new UserPasswordAuthenticationInfo(devopUser, devopPass));
            var guid = Guid.NewGuid().ToString();
            await client.WriteSecretAsync("secret/devop/test", guid);
            StepAssert.AreEqual(guid, await client.ReadSecretAsync("secret/devop/test"));
        }

        public async Task Run()
        {   
            var vaultUri = new Uri(await listAsk.LocalVaultUri.Ask());
            var rootToken = await vaultSealKeys.GetRootToken();
            
            var devopUser = await listAsk.LocalVaultDevopUser.Ask();
            var devopPass = await listAsk.LocalVaultDevopPassword.Ask();
        
            // install vault
            installer.Install("Vault");

            // log with root token
            var client = storeResolver.CreateClient("vault", new TokenAuthenticationInfo(rootToken));
            await client.EnableUsernamePassword();

            // create devop-policy
            var devopAdminPolicy = new Policy()
            {
                Name = "devop-policy",
                Rules = "path \"secret/devop/*\" { capabilities = [\"create\", \"read\", \"update\", \"delete\", \"list\"] }"
            };
            await client.WritePolicyAsync(devopAdminPolicy);

            // create user devop
            await client.WriteUser(devopUser, devopPass,"devop-policy");
        }

        public async Task Clean()
        {
            await Task.CompletedTask;
        }
    }
}
