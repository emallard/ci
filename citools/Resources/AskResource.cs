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
        private readonly IAskResourceLogger logger;
        string name;

        public AskResource(IAsk ask, IAskResourceLogger logger)
        {
            this.ask = ask;
            this.logger = logger;
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
            logger.LogAskResource(this);
            return await this.ask.GetValue(this.name);
        }
    }
}