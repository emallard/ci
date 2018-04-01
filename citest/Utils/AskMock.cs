using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using citools;
using cisteps;

namespace citest
{
    public class AskMock : IAsk
    {
        private readonly Lazy<ListAsk> listAsk;

        public AskMock(
            Lazy<ListAsk> listAsk
        )
        {
            this.listAsk = listAsk;
        }

        public async Task<string> GetValue(string key)
        {
            await Task.CompletedTask;
            if (key == listAsk.Value.CADomain.Name()) return "http://maboitededev.com";
            if (key == listAsk.Value.LocalVaultRootToken.Name()) return InMemoryStore.RootToken;
            if (key == listAsk.Value.LocalVaultUri.Name()) return "http://localVaultUri";

            if (key == listAsk.Value.LocalVaultDevopUser.Name()) return "devop";
            if (key == listAsk.Value.LocalVaultDevopPassword.Name()) return "devoppass";

            if (key == listAsk.Value.InfraApiKey.Name()) return "infrapiKey";
            if (key == listAsk.Value.PiloteRootPassword.Name()) return "rootpass";
            if (key == listAsk.Value.PiloteAdminUser.Name()) return "piloteadmin";
            if (key == listAsk.Value.PiloteAdminPassword.Name()) return "piloteadminpass";

            throw new Exception("unknown ask in mock : " + key);
        }
    }
}