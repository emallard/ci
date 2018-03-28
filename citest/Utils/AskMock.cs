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
            if (key == listAsk.Value.RootToken.Name()) return "unused";
            if (key == listAsk.Value.VaultUri.Name()) return "http://unused";
            if (key == listAsk.Value.DevopPassword.Name()) return "devoppass";

            throw new Exception("unknown ask in mock : " + key);
        }
    }
}