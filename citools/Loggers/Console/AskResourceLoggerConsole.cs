using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citools
{
    public class AskResourceLoggerConsole : IAskResourceLogger 
    {
        public async Task LogAskResource(AskResource askResource)
        {
            await Task.CompletedTask;
            Console.WriteLine("[Ask] " + askResource.Name());
        }
    }
}