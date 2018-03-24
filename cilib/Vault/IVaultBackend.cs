using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace cilib
{
    public interface IVaultBackend
    {
        string ReadSecret(VaultToken token, string path);

        void SetSecret(VaultToken token, string path, string secret);

        void AddPolicy(VaultToken token, VaultPolicy policy);

        VaultToken CreateTokenForPolicy(VaultToken token, VaultPolicy policy);
    }
}
