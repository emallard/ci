using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace cilib
{
    public class Vault
    {
        private readonly IVaultBackend backend;
        private VaultToken token;

        public Vault(IVaultBackend backend)
        {
            this.backend = backend;
        }

        public void SetToken(VaultToken token)
        {
            this.token = token;
        }

        public string ReadSecret(string path)
        {
            return this.backend.ReadSecret(token, path);
        }

        public void SetSecret(string path, string secret)
        {
            this.backend.SetSecret(token, path, secret);
        }

        public void AddPolicy(VaultPolicy policy)
        {
            this.backend.AddPolicy(token, policy);
        }

        public VaultToken CreateTokenForPolicy(VaultPolicy policy)
        {
            return this.backend.CreateTokenForPolicy(token, policy);
        }
    }
}
