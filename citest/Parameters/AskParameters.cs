using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using ciinfra;
using cilib;

namespace citest
{
    public class AskParameters
    {
        private readonly IAskParametersSource source;

        public AskParameters(IAskParametersSource source)
        {
            this.source = source;
        }

        public InfrastructureKey InfrastructureKey => new InfrastructureKey(GetValue("InfrastructureKey"));
        public string PiloteVmName => GetValue("PiloteVmName");
        public string PiloteRootPassword => GetValue("PiloteRootPassword");
        public string PiloteAdminUser => GetValue("PiloteAdminUser");
        public string PiloteAdminPassword => GetValue("PiloteAdminPassword");
        public Uri PiloteSshUri => new Uri(GetValue("PiloteSshUri"));

        public VaultToken PiloteCiVaultToken => new VaultToken(GetValue("PiloteCiVaultToken"));
        public Uri PrivateRegistryUri => new Uri(GetValue("PrivateRegistryUri"));

        public SshConnection PiloteSshConnection()
        {
            return new SshConnection()
            {
                SshUri = PiloteSshUri,
                User = PiloteAdminUser,
                Password = PiloteAdminPassword
            };
        }

        Dictionary<string,string> dic = new Dictionary<string, string>();

        private string GetValue(string key)
        {
            string val;
            if (!dic.TryGetValue(key, out val))
            {
                val = source.GetValue(key);
                dic[key] = val;
            }
            return val;
        }
    }
}