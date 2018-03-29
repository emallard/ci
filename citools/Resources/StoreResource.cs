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
        private readonly IStoreResourceLogger logger;
        private string path;

        public StoreResource(
            IStoreResolver storeResolver,
            IStoreResourceLogger logger)
        {
            this.storeResolver = storeResolver;
            this.logger = logger;
        }

        public StoreResource Path(string path)
        {
            this.path = path;
            return this;
        }

        public async Task<string> Read(IAuthenticationInfo authenticationInfo)
        {
            await logger.LogRead(path);
            var client = storeResolver.CreateClient(path, authenticationInfo);

            var result = await client.ReadSecretAsync(GetRelativePath(path));
            return result;
        }

        public async Task Write(IAuthenticationInfo authenticationInfo, string value)
        {
            await logger.LogWrite(path);
            var client = storeResolver.CreateClient(path, authenticationInfo);

            await client.WriteSecretAsync(GetRelativePath(path), value);
        }

        private string GetRelativePath(string p)
        {
            var slash = p.IndexOf('/');
            var store = p.Substring(slash);
            return store;
        }
    }
}