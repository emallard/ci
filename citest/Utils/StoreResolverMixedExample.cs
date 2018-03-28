
using System;
using System.Collections.Generic;
using citools;

namespace citest
{
    public class StoreResolverMixedExample : IStoreResolver
    {
        private readonly InMemoryStoreClientFactory inMemoryMemoryStoreClientFactory;
        private readonly VaultStoreClientFactory vaultStoreClientFactory;

        public StoreResolverMixedExample(
            InMemoryStoreClientFactory inMemoryMemoryStoreClientFactory,
            VaultStoreClientFactory vaultStoreClientFactory)
        {
            this.inMemoryMemoryStoreClientFactory = inMemoryMemoryStoreClientFactory;
            this.vaultStoreClientFactory = vaultStoreClientFactory;
        }

        public IStoreClient CreateClient(string path, IAuthenticationInfo authenticationInfo)
        {
            var p1 = path.Split('/')[0];
            if (p1 == "vault")
                return inMemoryMemoryStoreClientFactory.CreateClient(new Uri("http://localhost:8200"), authenticationInfo);
            if (p1 == "files")
                return vaultStoreClientFactory.CreateClient(new Uri("http://localhost:8200"), authenticationInfo);
                
            throw new Exception("unknown path to create client"); 
        }

    }
} 