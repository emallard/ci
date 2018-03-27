using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using citools;

namespace citools
{
    public class VaultStoreClientFactory : IStoreClientFactory
    {
        public IStoreClient CreateClient(Uri uri, IAuthenticationInfo authenticationInfo)
        {
            if (authenticationInfo is TokenAuthenticationInfo)
            {
                var auth = (TokenAuthenticationInfo) authenticationInfo;
                return new VaultStoreClient(
                    uri, 
                    new VaultSharp.Backends.Authentication.Models.Token.TokenAuthenticationInfo(auth.Token));
            }
            else if (authenticationInfo is UserPasswordAuthenticationInfo)
            {
                var auth = (UserPasswordAuthenticationInfo) authenticationInfo;
                return new VaultStoreClient(
                    uri, 
                    new VaultSharp.Backends.Authentication.Models.UsernamePassword.UsernamePasswordAuthenticationInfo(auth.User, auth.Password));
            } 
            throw new Exception("Unsupported IAuthenticationInfo");
        }
    }
}