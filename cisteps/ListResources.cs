using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using citools;
using VaultSharp.Backends.Authentication.Models;

namespace cisteps
{
    public class ListResources
    {
        public VaultResource InfrastructureApiKey = new VaultResource("infra/apikey");
        public VaultResource PiloteRootPassword = new VaultResource("infra/piloteRootPassword");
        
        public VaultResource PiloteSshUri = new VaultResource("admin/pilote/sshuri");
        public VaultResource PiloteUser = new VaultResource("admin/pilote/user");
        public VaultResource PilotePassword = new VaultResource("admin/pilote/password");

        public ReadResource<SshConnection> PiloteSshConnection;


        public ListResources()
        {
            PiloteSshConnection = new ReadResource<SshConnection>(GetPiloteSshConnection);
        }
        

        private async Task<SshConnection> GetPiloteSshConnection(Uri uri, IAuthenticationInfo authenticationInfo)
        {
            var sshUri = await PiloteSshUri.Read(uri, authenticationInfo);
            var user = await PiloteUser.Read(uri, authenticationInfo);
            var password = await PilotePassword.Read(uri, authenticationInfo);
            return new SshConnection() {
                SshUri = new Uri(sshUri),
                User = user,
                Password = password,
            };
        }



    }
}