using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citools
{
    public class VaultToken
    {
        public string Content;

        public VaultToken() {}

        public VaultToken(string content) 
        {
            Content = content;
        }
    }
}