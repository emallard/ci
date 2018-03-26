using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citools
{
    public interface IResource
    {
        Task<object> Read();

        Task Write(object value);
    }

    public interface IResource<T>
    {
        Task<T> Read();

        Task Write(T value);
    }
}