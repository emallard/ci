using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citools
{
    public class RunOkException : Exception
    {
        public RunOkException(Exception inner) :
            base("", inner)
        {

        }
    }
}