using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citools
{
    public class FuncLogger : ILogger
    {
        Func<object, Task> func;

        public FuncLogger()
        {

        }

        public FuncLogger SetFunc(Func<object, Task> func)
        {
            this.func = func;
            return this;
        }

        public async Task Log(object log)
        {
            await this.func(log);
        }
    }
}