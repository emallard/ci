
using System;
using System.Collections.Generic;
using citools;

namespace citest
{
    public class StoreResolverVault : IStoreResolver
    {
        private readonly VaultStoreClientFactory vaultStoreClientFactory;

        public StoreResolverVault(
            VaultStoreClientFactory vaultStoreClientFactory)
        {
            this.vaultStoreClientFactory = vaultStoreClientFactory;
        }

        public IStoreClient CreateClient(string path, IAuthenticationInfo authenticationInfo)
        {
            var p1 = path.Split('/')[0];
            if (p1 == "vault")
                return vaultStoreClientFactory.CreateClient(new Uri("http://localhost:8200"), authenticationInfo);
                
            throw new Exception("unknown path to create client"); 
        }

    }
} 