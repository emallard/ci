using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace ciinfra
{
    public class InfrastructureKey
    {
        public InfrastructureKey()
        {
        }

        public InfrastructureKey(string content)
        {
            Content = content;
        }

        public string Content;
    }
}