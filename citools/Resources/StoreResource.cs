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
        private readonly ILogger logger;
        private string path;

        public StoreResource(
            IStoreResolver storeResolver,
            ILogger logger)
        {
            this.storeResolver = storeResolver;
            this.logger = logger;
        }

        public StoreResource Path(string path)
        {
            this.path = path;
            return this;
        }

        public string Path()
        {
            return this.path;
        }

        public async Task<string> Read(IAuthenticationInfo authenticationInfo)
        {
            await logger.Log(new StoreResourceLogDto(this, StoreResourceLogDtoState.Read));

            var client = storeResolver.CreateClient(path, authenticationInfo);

            var result = await client.ReadSecretAsync(GetRelativePath(path));
            return result;
        }

        public async Task Write(IAuthenticationInfo authenticationInfo, string value)
        {
            await logger.Log(new StoreResourceLogDto(this, StoreResourceLogDtoState.Write));

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