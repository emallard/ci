using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citools
{
    public class JsonLogger : ILogger
    {
        private readonly JsonLoggerHelper helper;
        Func<string, Task> func;

        public JsonLogger(JsonLoggerHelper helper)
        {
            this.helper = helper;
        }

        public JsonLogger SetFunc(Func<string, Task> func)
        {
            this.func = func;
            return this;
        }

        public async Task Log(object log)
        {
            await this.func(helper.Serialize(log));
        }
    }
}