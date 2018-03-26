using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using ciinfra;
using VaultSharp.Backends.Authentication.Models;
using VaultSharp.Backends.Authentication.Models.Token;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using citools;

namespace citest
{
    public class KeepResultInLocalVault : IKeepResult
    {
        private readonly ShellHelper shellHelper;

        public KeepResultInLocalVault(
            ShellHelper shellHelper
        )
        {
            this.shellHelper = shellHelper;
        }

        public void Keep(string key, string value)
        {
            //shellHelper.Bash("secret-tool")
            var vaultAddress = "http://127.0.0.1:8200";

            //
            IAuthenticationInfo tokenAuthenticationInfo = new TokenAuthenticationInfo("6bc5838b-7d23-4c30-a293-b43933f4c0f5");
            var vaultClient = VaultSharp.VaultClientFactory.CreateVaultClient(new System.Uri(vaultAddress), tokenAuthenticationInfo);

            var task = vaultClient.WriteSecretAsync("/secrets/ci/" + key, new Dictionary<string, object>() {{"value", value}});
            task.Wait();
            var result = task.Result;
        }
    }
}