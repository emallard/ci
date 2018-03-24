using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace cilib
{
    public class Vault
    {
        private readonly IVaultSource backend;
        private VaultToken token;
        private Uri uri;

        public Vault(IVaultSource backend)
        {
            this.backend = backend;
        }

        public void SetUriAndToken(Uri uri, VaultToken token)
        {
            this.uri = uri;
            this.token = token;
        }

        public string ReadSecret(string path)
        {
            return this.backend.ReadSecret(uri, token, path);
        }

        public void SetSecret(string path, string secret)
        {
            this.backend.SetSecret(uri, token, path, secret);
        }

        public void AddPolicy(VaultPolicy policy)
        {
            this.backend.AddPolicy(uri, token, policy);
        }

        public VaultToken CreateTokenForPolicy(VaultPolicy policy)
        {
            return this.backend.CreateTokenForPolicy(uri, token, policy);
        }
    }
}
