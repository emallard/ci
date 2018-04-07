using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using citools;

namespace citools
{
    public class VaultStoreClient : IStoreClient
    {
        private readonly Uri uri;
        private readonly VaultSharp.Backends.Authentication.Models.IAuthenticationInfo authenticationInfo;

        public VaultStoreClient(
            Uri uri,
            VaultSharp.Backends.Authentication.Models.IAuthenticationInfo authenticationInfo)
        {
            this.uri = uri;
            this.authenticationInfo = authenticationInfo;
        }

        public async Task WritePolicyAsync(Policy policy)
        {
            var vaultClient = VaultSharp.VaultClientFactory.CreateVaultClient(uri, this.authenticationInfo);
            await vaultClient.WritePolicyAsync(new VaultSharp.Backends.System.Models.Policy()
            {
                Name = policy.Name,
                Rules = policy.Rules
            });
        }

        public async Task<Policy> GetPolicyAsync(string name)
        {
            var vaultClient = VaultSharp.VaultClientFactory.CreateVaultClient(uri, this.authenticationInfo);
            var vaultPolicy = await vaultClient.GetPolicyAsync(name);
            return new Policy()
            {
                Name = vaultPolicy.Name,
                Rules = vaultPolicy.Rules
            };
        }

        public async Task<string> ReadSecretAsync(string path)
        {
            var vaultClient = VaultSharp.VaultClientFactory.CreateVaultClient(uri, this.authenticationInfo);

            var result = await vaultClient.ReadSecretAsync(path);
            return (string) result.Data["value"];
        }


        public async Task WriteSecretAsync(string path, string value)
        {
            var vaultClient = VaultSharp.VaultClientFactory.CreateVaultClient(uri, this.authenticationInfo);

            await vaultClient.WriteSecretAsync(path, new Dictionary<string, object>
                    {{ "value", value }});
        }

        public async Task DeleteSecretAsync(string path)
        {
            var vaultClient = VaultSharp.VaultClientFactory.CreateVaultClient(uri, this.authenticationInfo);

            await vaultClient.DeleteSecretAsync(path);
        }

        public async Task EnableUsernamePassword()
        {
            var vaultClient = VaultSharp.VaultClientFactory.CreateVaultClient(uri, this.authenticationInfo);

            var authenticationBackend = new VaultSharp.Backends.Authentication.Models.AuthenticationBackend()
            {
                AuthenticationPath = "auth/userpass",
                BackendType = VaultSharp.Backends.Authentication.Models.AuthenticationBackendType.UsernamePassword,
                Description = "userpass"
            };
            await vaultClient.EnableAuthenticationBackendAsync(authenticationBackend);
        }

        public async Task WriteUser(string user, string password, string policy)
        {
            var vaultClient = VaultSharp.VaultClientFactory.CreateVaultClient(uri, this.authenticationInfo);

            await vaultClient.WriteSecretAsync("auth/userpass/users/" + user, new Dictionary<string, object>
                    {
                        { "password", password },
                        { "policies", policy }
                    });
        }
    }
}