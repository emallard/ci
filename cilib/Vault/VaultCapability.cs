using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace cilib
{
    public enum VaultCapability
    {
        Create,
        Write,
        Read,
        Delete,
        List
    }
}