
using System;

namespace citools
{
    public interface IStoreResolver
    {
        IStoreClient CreateClient(string path, IAuthenticationInfo authenticationInfo);
    }
} 