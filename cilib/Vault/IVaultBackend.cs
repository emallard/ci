using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace cilib
{
    public interface IVaultSource
    {
        string ReadSecret(Uri uri, VaultToken token, string path);

        void SetSecret(Uri uri, VaultToken token, string path, string secret);

        void AddPolicy(Uri uri, VaultToken token, VaultPolicy policy);

        VaultToken CreateTokenForPolicy(Uri uri, VaultToken token, VaultPolicy policy);
    }
}
