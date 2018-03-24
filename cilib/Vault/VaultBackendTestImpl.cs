using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace cilib
{
    public class VaultBackendTestImpl : IVaultBackend
    {
        List<VaultPolicy> policies = new List<VaultPolicy>();
        Dictionary<string, VaultPolicy> tokens = new Dictionary<string, VaultPolicy>();
        Dictionary<string, string> secrets = new Dictionary<string, string>();


        public void AddPolicy(VaultToken token, VaultPolicy policy)
        {
            tokens.Add(token.Content, policy);
        }

        public string ReadSecret(VaultToken token, string path)
        {
            var policy = GetPolicy(token);
            policy.CheckCapability(path, VaultCapability.Read);
            return secrets[path];
        }

        public void SetSecret(VaultToken token, string path, string secret)
        {
            var policy = GetPolicy(token);
            policy.CheckCapability(path, VaultCapability.Write);
            secrets[path] = secret;
        }

        public VaultToken CreateTokenForPolicy(VaultToken token, VaultPolicy policy)
        {
            var tokenPolicy = GetPolicy(token);
            tokenPolicy.CheckCapability("auth/token", VaultCapability.Create);

            var generatedToken = new VaultToken();
            generatedToken.Content = Guid.NewGuid().ToString();
            tokens.Add(generatedToken.Content, policy);
            return generatedToken;
        }

        private VaultPolicy GetPolicy(VaultToken token)
        {
            return tokens[token.Content];
        }
    }
}