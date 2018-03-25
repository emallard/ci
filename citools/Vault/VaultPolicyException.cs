using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citools
{
    public class VaultPolicyException : Exception
    {
        public readonly string path;
        public readonly VaultCapability capability;

        public VaultPolicyException(string path, VaultCapability capability)
        : base("Can't " + capability.ToString() + " at " + path)
        {
            this.path = path;
            this.capability = capability;
        }
        
    }
}