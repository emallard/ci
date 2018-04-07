using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using citools;

namespace cisteps
{
    public class ListAsk
    {
        public AskResource LocalVaultUri;
        public AskResource LocalVaultDevopUser;
        public AskResource LocalVaultDevopPassword;
        //public AskResource LocalVaultToken;

        public AskResource CADomain;
        
        public AskResource InfraApiKey;
        public AskResource PiloteRootPassword;
        public AskResource PiloteAdminUser;
        public AskResource PiloteAdminPassword;

        public AskResource WebServerRootPassword;
        public AskResource WebServerAdminUser;
        public AskResource WebServerAdminPassword;

        public AskResource GitUri;
        public AskResource GitDirectory;

        public ListAsk(Func<AskResource> createAskResource)
        {
            LocalVaultUri = createAskResource().Name("vault uri");
            LocalVaultDevopUser = createAskResource().Name("devop user");
            LocalVaultDevopPassword = createAskResource().Name("devop password");
            //LocalVaultToken = createAskResource().Name("vault token");

            CADomain = createAskResource().Name("CA domain");

            InfraApiKey = createAskResource().Name("infrastructure apikey");
            PiloteRootPassword = createAskResource().Name("pilote root password");
            PiloteAdminUser = createAskResource().Name("pilote admin user");
            PiloteAdminPassword = createAskResource().Name("pilote admin password");

            WebServerRootPassword = createAskResource().Name("webserver root password");
            WebServerAdminUser = createAskResource().Name("webserver admin user");
            WebServerAdminPassword = createAskResource().Name("webserver admin password");

            GitUri = createAskResource().Name("git uri");
            GitDirectory = createAskResource().Name("git directory");

            this.CheckNoNullField();

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