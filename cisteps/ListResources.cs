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

        public StoreResource WebServerRootPassword;
        public StoreResource WebServerSshUri;
        public StoreResource WebServerUser;
        public StoreResource WebServerPassword;
        public ReadResource<SshConnection> WebServerSshConnection;

        public StoreResource GitUri;
        public StoreResource GitDirectory;

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

            WebServerRootPassword = createStoreResource().Path( "vault/secret/devop/webserverRootPassword");
            WebServerSshUri = createStoreResource().Path("vault/secret/devop/webserver/sshuri");
            WebServerUser = createStoreResource().Path("vault/secret/devop/webserver/user");
            WebServerPassword = createStoreResource().Path("vault/secret/devop/webserver/password");
            WebServerSshConnection = new ReadResource<SshConnection>(GetWebServerSshConnection);

            GitUri = createStoreResource().Path("vault/secret/devop/pilote/gitUri");
            GitDirectory = createStoreResource().Path("vault/secret/devop/pilote/gitDirectory");

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

        private async Task<SshConnection> GetWebServerSshConnection(IAuthenticationInfo authenticationInfo)
        {
            var sshUri = await WebServerSshUri.Read(authenticationInfo);
            var user = await WebServerUser.Read(authenticationInfo);
            var password = await WebServerPassword.Read(authenticationInfo);
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