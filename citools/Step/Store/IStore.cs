using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citools
{
    public interface IStore
    {
        Task<string> Read(Uri uri, string path);

        Task Write(Uri uri, string path, string value);
    }
}