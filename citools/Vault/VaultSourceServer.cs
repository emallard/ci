using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using VaultSharp.Backends.Authentication.Models;
using VaultSharp.Backends.Authentication.Models.Token;
using VaultSharp;

namespace citools
{
    public class VaultSourceServer : IVaultSource
    {
        public void AddPolicy(Uri uri, VaultToken token, VaultPolicy policy)
        {
            var client = this.GetClient(uri, token);

        }

        public VaultToken CreateTokenForPolicy(Uri uri, VaultToken token, VaultPolicy policy)
        {
            throw new NotImplementedException();
        }

        public string ReadSecret(Uri uri, VaultToken token, string path)
        {
            throw new NotImplementedException();
        }

        public void SetSecret(Uri uri, VaultToken token, string path, string secret)
        {
            throw new NotImplementedException();
        }

        private IVaultClient GetClient(Uri uri, VaultToken token)
        {
            IAuthenticationInfo tokenAuthenticationInfo = new TokenAuthenticationInfo(token.Content);
            return VaultSharp.VaultClientFactory.CreateVaultClient(uri, tokenAuthenticationInfo);
        }
    }
}
