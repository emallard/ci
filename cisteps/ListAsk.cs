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
        public AskResource LocalVaultRootToken;
        public AskResource LocalVaultDevopUser;
        public AskResource LocalVaultDevopPassword;
        //public AskResource LocalVaultToken;

        public AskResource CADomain;
        
        public AskResource InfraApiKey;
        public AskResource PiloteRootPassword;
        public AskResource PiloteAdminUser;
        public AskResource PiloteAdminPassword;

        public ListAsk(Func<AskResource> createAskResource)
        {
            LocalVaultUri = createAskResource().Name("vault uri");
            LocalVaultRootToken = createAskResource().Name("root token");
            LocalVaultDevopUser = createAskResource().Name("devop user");
            LocalVaultDevopPassword = createAskResource().Name("devop password");
            //LocalVaultToken = createAskResource().Name("vault token");

            CADomain = createAskResource().Name("CA domain");

            InfraApiKey = createAskResource().Name("apikey");
            PiloteRootPassword = createAskResource().Name("pilote root password");
            PiloteAdminUser = createAskResource().Name("pilote admin user");
            PiloteAdminPassword = createAskResource().Name("pilote admin password");


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