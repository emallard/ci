using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using ciinfra;
using ciexecommands;

namespace citest
{
    public class AskParametersReadLine : IAskParametersSource
    {
        public string GetValue(string key)
        {
            Console.WriteLine(key);
            var val = Console.ReadLine();
            return val;
        }
    }
}