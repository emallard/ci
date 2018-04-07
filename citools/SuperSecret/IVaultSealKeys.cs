using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citools
{
    public interface IVaultSealKeys 
    {
        Task SetSealKeys(IEnumerable<string> keys);
        Task<IEnumerable<string>> GetSealKeys();

        Task SetRootToken(string rootToken);
        Task<string> GetRootToken();
    }
}