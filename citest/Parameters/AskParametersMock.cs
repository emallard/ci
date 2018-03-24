using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using ciinfra;
using cilib;

namespace citest
{
    public class AskParametersSourceMock : IAskParametersSource
    {
        public string InfrastructureKey => "no need for virtualbox";

        public string PiloteVmName => "pilote";

        public string PiloteRootPassword => "no need for virtualbox";

        public string PiloteAdminUser => "test";

        public string PiloteAdminPassword => "test";

        public string PiloteSshUri => "ssh://127.0.0.1:22005";

        public string PiloteCiVaultToken => throw new NotImplementedException();

        public string PrivateRegistryUri => "https://registry.mycompany.com:5443";

        public string GetValue(string key)
        {
            return (string) this.GetType().GetProperty(key).GetMethod.Invoke(this, new object[0]);
        }
    }
}