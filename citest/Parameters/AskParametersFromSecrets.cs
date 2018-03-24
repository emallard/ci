using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citest
{
    public class AskParametersFromSecrets : IAskParameters
    {
        public string InfrastructureKey => SecretStore.GetSecret("InfrastructureKey");

        public string PiloteVmName => SecretStore.GetSecret("PiloteVmName");

        public string PiloteRootPassword => SecretStore.GetSecret("PiloteRootPassword");

        public string PiloteAdminUser => SecretStore.GetSecret("PiloteAdminUser");

        public string PiloteAdminPassword => SecretStore.GetSecret("PiloteAdminPassword");
    }
}