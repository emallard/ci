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
        public AskResource VaultUri;
        public AskResource RootToken;
        public AskResource DevopPassword;
        public AskResource VaultToken;

        public ListAsk(Func<AskResource> createAskResource)
        {
            VaultUri = createAskResource().Name("vault uri");
            RootToken = createAskResource().Name("root token");
            DevopPassword = createAskResource().Name("devop password");
            VaultToken = createAskResource().Name("vault token");
        }
    }
}