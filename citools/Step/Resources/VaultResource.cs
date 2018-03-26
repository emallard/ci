using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using VaultSharp.Backends.Authentication.Models.Token;
using VaultSharp.Backends.Authentication.Models;

namespace citools
{
    public class VaultResource
    {
        private readonly string path;

        public VaultResource(string path)
        {
            this.path = path;
        }

        public async Task<string> Read(Uri uri, IAuthenticationInfo authenticationInfo)
        {
            var vaultAddress = new UriBuilder();
            vaultAddress.Scheme = uri.Scheme;
            vaultAddress.Host = uri.Host;
            var token = uri.UserInfo;

            var vaultClient = VaultSharp.VaultClientFactory.CreateVaultClient(vaultAddress.Uri, authenticationInfo);

            var result = await vaultClient.ReadSecretAsync("/secrets/" + path);
            return (string) result.Data["value"];
        }

        public async Task Write(Uri uri, IAuthenticationInfo authenticationInfo, string value)
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