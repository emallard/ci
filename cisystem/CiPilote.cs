using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace cisystem
{
    public class CiPilote : CiVm 
    {
        public Uri PrivateRegistryUri;
        public string PrivateRegistryUser;
        public string PrivateRegistryPassword;
    }
}