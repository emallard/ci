using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citools
{
    public class ConsoleLogger : ILogger
    {
        private readonly JsonLoggerHelper helper;

        public ConsoleLogger(JsonLoggerHelper helper)
        {
            this.helper = helper;
        }

        public async Task Log(object log)
        {
            await Task.CompletedTask;
            Console.WriteLine(helper.Serialize(log));
        }
    }
}