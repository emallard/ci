using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using ciinfra;
using ciexecommands;

namespace citest
{
    public class AskParametersFromSecrets : IAskParametersSource
    {
        public string GetValue(string key)
        {
            return SecretStore.GetSecret(key);
        }
    }
}