using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citools
{
    public class StoreResource
    {
        private readonly IStoreResolver storeResolver;
        private string path;

        public StoreResource(IStoreResolver storeResolver)
        {
            this.storeResolver = storeResolver;
            
        }

        public StoreResource Path(string path)
        {
            this.path = path;
            return this;
        }

        public async Task<string> Read(IAuthenticationInfo authenticationInfo)
        {
            var client = storeResolver.CreateClient(path, authenticationInfo);

            var result = await client.ReadSecretAsync("/secrets/" + path);
            return result;
        }

        public async Task Write(IAuthenticationInfo authenticationInfo, string value)
        {
            var client = storeResolver.CreateClient(path, authenticationInfo);

            await client.WriteSecretAsync("/secrets/" + path, value);
        }
    }
}