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
        public StoreResource CAKey;
        public StoreResource CAPem;

        public StoreResource InfrastructureApiKey;
        public StoreResource PiloteRootPassword;
        
        public StoreResource PiloteSshUri;
        public StoreResource PiloteUser;
        public StoreResource PilotePassword;

        public ReadResource<SshConnection> PiloteSshConnection;


        public ListResources(Func<StoreResource> createStoreResource)
        {
            CAKey = createStoreResource().Path("vault/secret/devop/CAKey");
            CAPem = createStoreResource().Path( "vault/secret/devop/CAPem");
           
            InfrastructureApiKey = createStoreResource().Path("vault/secret/devop/apikey");
            PiloteRootPassword = createStoreResource().Path( "vault/secret/devop/piloteRootPassword");
            
            PiloteSshUri = createStoreResource().Path("vault/secret/devop/pilote/sshuri");
            PiloteUser = createStoreResource().Path("vault/secret/devop/pilote/user");
            PilotePassword = createStoreResource().Path("vault/secret/devop/pilote/password");

            PiloteSshConnection = new ReadResource<SshConnection>(GetPiloteSshConnection);

            this.CheckNoNullField();
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

        void CheckNoNullField()
        {
            foreach (var fi in this.GetType().GetFields())
            {
                if (fi.GetValue(this) == null)
                {
                    throw new Exception("You forget to assign field " + fi.Name);
                }
            }
        }

    }
}