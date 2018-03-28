
using System;
using System.Collections.Generic;
using citools;

namespace citest
{
    public class StoreResolverInMemory : IStoreResolver
    {
        private readonly InMemoryStoreClientFactory inMemoryMemoryStoreClientFactory;
        Dictionary<string, string> mountingPoints = new Dictionary<string, string> ();

        public StoreResolverInMemory(
            InMemoryStoreClientFactory inMemoryMemoryStoreClientFactory)
        {
            this.inMemoryMemoryStoreClientFactory = inMemoryMemoryStoreClientFactory;
        }

        public IStoreClient CreateClient(string path, IAuthenticationInfo authenticationInfo)
        {
            var p1 = path.Split('/')[0];
            if (p1 == "vault")
                return inMemoryMemoryStoreClientFactory.CreateClient(new Uri("http://localhost:8200"), authenticationInfo);
                
            throw new Exception("unknown path to create client"); 
        }

    }
} 