using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace citools
{
    public class VaultSealKeysFile : IVaultSealKeys
    {

        string filename = "ciTestVaultSealKeys";
        public async Task SetSealKeys(IEnumerable<string> keys)
        {
            await Task.CompletedTask;
            File.WriteAllLines(filename, keys);
        }

        public async Task<IEnumerable<string>> GetSealKeys()
        {
            await Task.CompletedTask;
            return File.ReadAllLines(filename);
        }

        public async Task SetRootToken(string rootToken)
        {
            await Task.CompletedTask;
            File.WriteAllText("ciTestVaultRootToken", rootToken);
        }

        public async Task<string> GetRootToken()
        {
            await Task.CompletedTask;
            return File.ReadAllText("ciTestVaultRootToken");
        }
    }
}