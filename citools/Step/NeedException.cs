using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citools
{
    public class NeedException : Exception
    {
        public NeedException(Exception inner) :
            base("", inner)
        {

        }
    }
}