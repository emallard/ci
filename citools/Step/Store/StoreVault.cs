using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using VaultSharp.Backends.Authentication.Models;
using VaultSharp.Backends.Authentication.Models.Token;
using citools;

namespace citools
{
    public class StoreVault : IStore
    {
        public async Task<string> Read(Uri uri, string path)
        {
            var vaultAddress = new UriBuilder();
            vaultAddress.Scheme = uri.Scheme;
            vaultAddress.Host = uri.Host;
            var token = uri.UserInfo;

            IAuthenticationInfo tokenAuthenticationInfo = new TokenAuthenticationInfo(token);
            var vaultClient = VaultSharp.VaultClientFactory.CreateVaultClient(vaultAddress.Uri, tokenAuthenticationInfo);

            var result = await vaultClient.ReadSecretAsync("/secrets/" + path);
            return (string) result.Data["value"];
        }

        public async Task Write(Uri uri, string path, string value)
        {
            var vaultAddress = new UriBuilder();
            vaultAddress.Scheme = uri.Scheme;
            vaultAddress.Host = uri.Host;
            var token = uri.UserInfo;

            IAuthenticationInfo tokenAuthenticationInfo = new TokenAuthenticationInfo(token);
            var vaultClient = VaultSharp.VaultClientFactory.CreateVaultClient(vaultAddress.Uri, tokenAuthenticationInfo);

            await vaultClient.WriteSecretAsync("/secrets/" + path, new Dictionary<string, object>
                    {{ "value", value }});
        }
    }
}