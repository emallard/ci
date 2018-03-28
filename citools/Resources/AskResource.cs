using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citools
{
    public class AskResource
    {
        IAsk ask;
        string name;

        public AskResource(IAsk ask)
        {
            this.ask = ask;
        }

        public AskResource Name(string name)
        {
            this.name = name;
            return this;
        }

        public string Name()
        {
            return this.name;
        }

        public async Task<string> Ask()
        {
            return await this.ask.GetValue(this.name);
        }
    }
}