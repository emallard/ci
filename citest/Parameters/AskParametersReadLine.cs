using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using ciinfra;
using cilib;

namespace citest
{
    public class AskParametersReadLine : IAskParametersSource
    {

        Dictionary<string,string> dic = new Dictionary<string, string>();

        public string GetValue(string key)
        {
            Console.WriteLine(key);
            var val = Console.ReadLine();
            return val;
        }
    }
}