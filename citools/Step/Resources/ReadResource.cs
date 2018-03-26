using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using VaultSharp.Backends.Authentication.Models;

namespace citools
{
    public class ReadResource<T>
    {
        private readonly Func<Uri, IAuthenticationInfo, Task<T>> func;

        public ReadResource(Func<Uri, IAuthenticationInfo, Task<T>> func)
        {
            this.func = func;
        }
        
        public async Task<T> Read(Uri uri, IAuthenticationInfo auth)
        {
            return await this.func(uri, auth);
        }
    }
}