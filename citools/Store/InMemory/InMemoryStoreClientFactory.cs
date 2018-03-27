using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citools
{
    public class InMemoryStoreClientFactory : IStoreClientFactory
    {
        public IStoreClient CreateClient(Uri uri, IAuthenticationInfo authenticationInfo)
        {
            throw new NotImplementedException();
        }
    }
}
