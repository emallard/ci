using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citest
{
    public class AskParametersMock : IAskParameters
    {
        public string InfrastructureKey => "no need for virtualbox";

        public string PiloteVmName => "pilote";

        public string PiloteRootPassword => "no need for virtualbox";

        public string PiloteAdminUser => "test";

        public string PiloteAdminPassword => "test";
    }
}