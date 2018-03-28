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
        private readonly Func<IAuthenticationInfo, Task<T>> func;

        public ReadResource(Func<IAuthenticationInfo, Task<T>> func)
        {
            this.func = func;
        }
        
        public async Task<T> Read(IAuthenticationInfo auth)
        {
            return await this.func(auth);
        }
    }
}