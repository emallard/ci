using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace citools
{
    public class VaultSealKeysMessageBox : IVaultSealKeys
    {

        public Task SetSealKeys(IEnumerable<string> keys)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> GetSealKeys()
        {
            throw new NotImplementedException();
        }

        public Task SetRootToken(string rootToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRootToken()
        {
            throw new NotImplementedException();
        }
    }
}