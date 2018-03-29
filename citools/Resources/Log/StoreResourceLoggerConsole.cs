using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citools
{
    public class StoreResourceLoggerConsole : IStoreResourceLogger
    {
        public async Task LogRead(string path)
        {
            await Task.CompletedTask;
            Console.WriteLine("Read " + path);
        }

        public async Task LogWrite(string path)
        {
            await Task.CompletedTask;
            Console.WriteLine("Write " + path);
        }
    }
}