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
        private readonly ListAsk listAsk;

        public AskMock(
            ListAsk listAsk
        )
        {
            this.listAsk = listAsk;
        }

        public Task<string> GetValue(string key)
        {
            return new Task<string>(() =>
            {
                if (key == listAsk.RootToken.Name()) return "unused";
                if (key == listAsk.VaultUri.Name()) return "unused";
                if (key == listAsk.DevopPassword.Name()) return "devoppass";

                throw new Exception("unknown ask in mock : " + key);
            });
        }
    }
}