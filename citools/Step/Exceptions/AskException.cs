using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citools
{
    public class AskException : Exception
    {
        public AskException(Exception inner) :
            base("", inner)
        {

        }
    }
}