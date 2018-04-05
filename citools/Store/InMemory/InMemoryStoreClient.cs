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
        private readonly InMemoryStore store;

        public InMemoryStoreClient(InMemoryStore store)
        {
            this.store = store;
        }
        
        public InMemoryStoreClient SetAuthentication(IAuthenticationInfo auth)
        {
            this.auth = auth;
            return this;
        }

        public async Task EnableUsernamePassword()
        {
            await store.EnableUsernamePassword(this.auth);
        }

        public async Task<Policy> GetPolicyAsync(string name)
        {
            return await store.GetPolicyAsync(this.auth, name);
        }

        public async Task<string> ReadSecretAsync(string path)
        {
            return await store.ReadSecretAsync(this.auth, path);
        }

        public async Task WritePolicyAsync(Policy policy)
        {
            await store.WritePolicyAsync(this.auth, policy);
        }

        public async Task WriteSecretAsync(string path, string value)
        {
            await store.WriteSecretAsync(this.auth, path, value);
        }

        public async Task WriteUser(string user, string password, string policy)
        {
            await store.WriteUser(this.auth, user, password, policy);
        }

        public async Task DeleteSecretAsync(string path)
        {
            await store.DeleteSecretAsync(this.auth, path);
        }
    }
}
