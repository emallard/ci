using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citools
{
    public class InMemoryStore
    {
        Dictionary<string, Policy> policies = new Dictionary<string, Policy>();
        Dictionary<string, string> tokens = new Dictionary<string, string>();
        Dictionary<string, string> secrets = new Dictionary<string, string>();
        Dictionary<string, string> usernamePasswords = new Dictionary<string, string>();
        
        public InMemoryStore()
        {
            
        }

        public async Task EnableUsernamePassword(IAuthenticationInfo auth)
        {
            await Task.CompletedTask;
        }

        public async Task<Policy> GetPolicyAsync(IAuthenticationInfo auth, string name)
        {
            await Task.CompletedTask;
            Policy found;
            if (policies.TryGetValue(name, out found))
                return found;
                
            return null;
        }

        public async Task<string> ReadSecretAsync(IAuthenticationInfo auth, string path)
        {
            CheckPathAndCapability(auth, path, "read");
            await Task.CompletedTask;
            string found;
            if (secrets.TryGetValue(path, out found))
                return found;
                
            return null;
        }

        public async Task WritePolicyAsync(IAuthenticationInfo auth, Policy policy)
        {
            await Task.CompletedTask;
            policies[policy.Name] = policy ;
        }

        public async Task WriteSecretAsync(IAuthenticationInfo auth, string path, string value)
        {
            await Task.CompletedTask;
            CheckPathAndCapability(auth, path, "write");
            secrets[path] = value ;
        }

        public async Task WriteUser(IAuthenticationInfo auth, string user, string password, string policy)
        {
            await Task.CompletedTask;
            usernamePasswords[user + ":" + password] = policy ;
        }

        private void CheckPathAndCapability(IAuthenticationInfo auth, string path, string capability)
        {
            var policy = GetPolicy(auth);
            CheckPathAndCapability(policy, path, "write");
        }

        private Policy GetPolicy(IAuthenticationInfo auth)
        {
            Policy found;
            if (auth is UserPasswordAuthenticationInfo)
            {
                var auth2 = (UserPasswordAuthenticationInfo) auth;
                string policyName;
                if (usernamePasswords.TryGetValue(auth2.User + ":" + auth2.Password, out policyName))
                {
                    policies.TryGetValue(policyName, out found);
                }
            }
            throw new Exception("InMemoryStore notSupported IAuthenticationInfo");
        }

        private void CheckPathAndCapability(Policy policy, string path, string capability)
        {
            if (policy == null)
                throw new CapabilityException(path, capability); 
            
            var rules = policy.Rules;
        }
    }
}
