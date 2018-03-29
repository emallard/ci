using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citools
{
    public class AskResourceLoggerConsole : IAskResourceLogger 
    {
        public async Task Log(string ask)
        {
            await Task.CompletedTask;
            Console.WriteLine("[Ask] " + ask);
        }
    }
}