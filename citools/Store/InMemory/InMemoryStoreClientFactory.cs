using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citools
{
    public class InMemoryStoreClientFactory : IStoreClientFactory
    {
        private readonly Func<InMemoryStoreClient> createClient;

        public InMemoryStoreClientFactory(
           Func<InMemoryStoreClient> createClient
        )
        {
            this.createClient = createClient;
        }

        public IStoreClient CreateClient(Uri uri, IAuthenticationInfo authenticationInfo)
        {
            return this.createClient().SetAuthentication(authenticationInfo);
        }
    }
}
