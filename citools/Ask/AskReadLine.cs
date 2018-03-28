using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citools
{

    public class AskReadLine : IAsk
    {
        public async Task<string> GetValue(string key)
        {
            await Task.CompletedTask;
            Console.WriteLine(key);
            var val = Console.ReadLine();
            return val;
        }
    }
}