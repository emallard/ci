using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citest
{
    public interface IKeepResult
    {
        void Keep(string key, string value);
    }
}