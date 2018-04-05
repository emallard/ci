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
        
        public const string RootToken = "inmemory-root-token";

        public InMemoryStore()
        {
            var rootPolicy = new Policy() {
                Name = "root",
                Rules = "path \"secret/*\" { capabilities = [\"create\", \"read\", \"update\", \"delete\", \"list\"]"
            };
            policies.Add(rootPolicy.Name, rootPolicy);
            tokens.Add(RootToken, "root");
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
                
            throw new Exception("MemoryStore Policy not found");
        }

        public async Task<string> ReadSecretAsync(IAuthenticationInfo auth, string path)
        {
            CheckPathAndCapability(auth, path, "read");
            await Task.CompletedTask;
            string found;
            if (secrets.TryGetValue(path, out found))
                return found;
            
            throw new Exception("MemoryStore Read not found");
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

        public async Task DeleteSecretAsync(IAuthenticationInfo auth, string path)
        {
            await Task.CompletedTask;
            CheckPathAndCapability(auth, path, "delete");
            secrets.Remove(path);
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
                var userPassword = (UserPasswordAuthenticationInfo) auth;
                string policyName;
                if (usernamePasswords.TryGetValue(userPassword.User + ":" + userPassword.Password, out policyName))
                {
                    policies.TryGetValue(policyName, out found);
                    return found;
                }
            }
            if (auth is TokenAuthenticationInfo)
            {
                var token = (TokenAuthenticationInfo) auth;
                string policyName;
                if (tokens.TryGetValue(token.Token, out policyName))
                {
                    policies.TryGetValue(policyName, out found);
                    return found;
                }
            }

            throw new Exception("InMemoryStore notSupported IAuthenticationInfo");
            
        }

        private void CheckPathAndCapability(Policy policy, string path, string capability)
        {
            if (policy == null)
                throw new CapabilityException(path, capability); 
            
            var rules = policy.Rules;
            
            // TODO check rules with Regexp
        }
    }
}
