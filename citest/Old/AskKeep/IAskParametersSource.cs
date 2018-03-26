using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citest
{
    public interface IAskParametersSource
    {
        string GetValue(string key);
    }
}