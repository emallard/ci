using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citools
{

    public class AskReadLine : IAsk
    {
        public Task<string> GetValue(string key)
        {
            Console.WriteLine(key);
            var val = Console.ReadLine();
            return new Task<string>(() => val);
        }
    }
}