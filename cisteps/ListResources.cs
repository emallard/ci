using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using citools;

namespace cisteps
{
    public class ListResources
    {
        
        public StoreResource InfrastructureApiKey;
        public StoreResource PiloteRootPassword;
        
        public StoreResource PiloteSshUri;
        public StoreResource PiloteUser;
        public StoreResource PilotePassword;

        public ReadResource<SshConnection> PiloteSshConnection;


        public ListResources(Func<StoreResource> createStoreResource)
        {
            InfrastructureApiKey = createStoreResource().Path("vault/infra/apikey");
            PiloteRootPassword = createStoreResource().Path("vault/infra/piloteRootPassword");
            PiloteSshUri = createStoreResource().Path("vault/admin/pilote/sshuri");
            PiloteUser = createStoreResource().Path("vault/admin/pilote/user");
            PilotePassword = createStoreResource().Path("vault/admin/pilote/password");

            PiloteSshConnection = new ReadResource<SshConnection>(GetPiloteSshConnection);
        }
        

        private async Task<SshConnection> GetPiloteSshConnection(IAuthenticationInfo authenticationInfo)
        {
            var sshUri = await PiloteSshUri.Read(authenticationInfo);
            var user = await PiloteUser.Read(authenticationInfo);
            var password = await PilotePassword.Read(authenticationInfo);
            return new SshConnection() {
                SshUri = new Uri(sshUri),
                User = user,
                Password = password,
            };
        }



    }
}