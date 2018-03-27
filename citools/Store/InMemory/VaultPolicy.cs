using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citools
{
    public class VaultPolicy
    {
        Dictionary<string, List<VaultCapability>> Items = new Dictionary<string, List<VaultCapability>>();
        public void CheckCapability(string path, VaultCapability capability)
        {
            List<VaultCapability> found;
            if (!Items.TryGetValue(path, out found))
                throw new VaultPolicyException(path, capability);
            
            if (!found.Contains(capability))
                throw new VaultPolicyException(path, capability);
        }
    }

}