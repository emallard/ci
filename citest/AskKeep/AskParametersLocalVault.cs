using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using ciinfra;
using VaultSharp.Backends.Authentication.Models;
using VaultSharp.Backends.Authentication.Models.Token;
using ciexecommands;

namespace citest
{
    public class AskParametersLocalVault : IAskParametersSource
    {
        public string GetValue(string key)
        {
            var vaultAddress = "http://127.0.0.1:8200";

            IAuthenticationInfo tokenAuthenticationInfo = new TokenAuthenticationInfo("6bc5838b-7d23-4c30-a293-b43933f4c0f5");
            var vaultClient = VaultSharp.VaultClientFactory.CreateVaultClient(new System.Uri(vaultAddress), tokenAuthenticationInfo);

            var task = vaultClient.ReadSecretAsync("/secrets/ci/" + key);
            task.Wait();
            var result = task.Result;
            return (string) result.Data["value"];
        }
    }
}