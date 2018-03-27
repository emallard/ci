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
        public Task<string> GetValue(string key)
        {
            return new Task<string>(() =>
            {
                if (key == ListAsk.RootToken) return "unused";
                if (key == ListAsk.VaultUri) return "unused";
                if (key == ListAsk.DevopPassword) return "devoppass";

                throw new Exception("unknown ask in mock : " + key);
            });
        }
    }
}