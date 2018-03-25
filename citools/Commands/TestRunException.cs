using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citools
{
    public class TestRunException : Exception
    {
        public TestRunException(Object obj) :
            base(obj.GetType().Name)
        {

        }
    }
}