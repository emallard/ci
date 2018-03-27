using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citools
{
    public class AlreadyRunException : Exception
    {
        public AlreadyRunException(Exception inner) :
            base("", inner)
        {

        }
    }
}