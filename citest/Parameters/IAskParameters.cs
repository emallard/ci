using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citest
{
    public interface IAskParameters
    {
        string InfrastructureKey {get;}
        string PiloteVmName {get;}
        string PiloteRootPassword {get;}
        string PiloteAdminUser {get;}
        string PiloteAdminPassword {get;}
    }
}