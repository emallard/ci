using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace citools
{
    public interface IEmbeddedResource
    {
        string Name { get; }
        Stream Stream();
        string ReadAsText();
    }
}