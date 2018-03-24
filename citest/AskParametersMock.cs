using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citest
{
    public class AskParametersMock : IAskParameters
    {
        public string Ask(string key)
        {
            if (key == "piloteAdminUser")
                return "test";
            if (key == "piloteAdminPassword")
                return "test";
            throw new Exception("Unkown key");
        }
    }
}