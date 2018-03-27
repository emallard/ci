using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citools
{
    public class InMemoryStoreClient : IStoreClient
    {
        private IAuthenticationInfo auth;
        private Policy authPolicy;

        Dictionary<string, Policy> policies = new Dictionary<string, Policy>();
        Dictionary<string, string> tokens = new Dictionary<string, string>();
        Dictionary<string, string> secrets = new Dictionary<string, string>();
        Dictionary<string, string> usernamePasswords = new Dictionary<string, string>();
        

        public InMemoryStoreClient(IAuthenticationInfo auth)
        {
            this.auth = auth;
            if (auth is UserPasswordAuthenticationInfo)
            {
                var auth2 = (UserPasswordAuthenticationInfo) auth;
                string policyName;
                if (usernamePasswords.TryGetValue(auth2.User + ":" + auth2.Password, out policyName))
                {
                    policies.TryGetValue(policyName, out this.authPolicy);
                }
            }
        }

        public async Task EnableUsernamePassword()
        {
            await Task.CompletedTask;
        }

        public Task<Policy> GetPolicyAsync(string name)
        {
            return new Task<Policy>(() => {
                Policy found;
                if (policies.TryGetValue(name, out found))
                    return found;
                    
                return null;
            });
        }

        public Task<string> ReadSecretAsync(string path)
        {
            CheckPathAndCapability(path, "read");
            return new Task<string>(() => {
                string found;
                if (secrets.TryGetValue(path, out found))
                    return found;
                    
                return null;
            });
        }

        public Task WritePolicyAsync(Policy policy)
        {
            return new Task(() => { policies[policy.Name] = policy ; });
        }

        public Task WriteSecretAsync(string path, string value)
        {
            CheckPathAndCapability(path, "write");
            return new Task(() => { secrets[path] = value ; });
        }

        public Task WriteUser(string user, string password, string policy)
        {
            return new Task(() => { usernamePasswords[user + ":" + password] = policy ; });
            
        }

        private void CheckPathAndCapability(string path, string capability)
        {
            if (this.authPolicy == null)
                throw new CapabilityException(path, capability); 
            
            var rules = authPolicy.Rules;
        }
    }
}
