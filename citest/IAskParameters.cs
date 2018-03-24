using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citest
{
    public interface IAskParameters
    {
        string Ask(string key);
    }
}