using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citools
{
    public class CapabilityException : Exception
    {
        public CapabilityException(string path, string capability) :
            base("CapabilityException : " + path + " , " + capability)
        {

        }
    }
}