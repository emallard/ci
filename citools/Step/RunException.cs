using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citools
{
    public class RunException : Exception
    {
        public RunException(Exception inner) :
            base("", inner)
        {

        }
    }
}