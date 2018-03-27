
using System;

namespace citools
{
    public class StoreResolverDictionary : IStoreResolver
    {
  /*      
        public void AddStoreClientFactory(string host, Uri realUri, IStoreClientFactory storeClientFactory)
        {
            
        }
*/
        public IStoreClient CreateClient(string path, IAuthenticationInfo authenticationInfo)
        {
            if (path.Split('/')[0] == "vault")
                return new VaultStoreClientFactory().CreateClient(new Uri("http://localhost:8200"), authenticationInfo);

            throw new Exception("unknown uri"); 
        }

    }
} 