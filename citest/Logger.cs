using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citest
{
    public class Logger {

        public void Log(Action action, string logBefore, string logAfter)
        {
            Console.WriteLine(logBefore);
            action();
            Console.WriteLine(logAfter);
        }
    }
}