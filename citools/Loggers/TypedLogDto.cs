using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Dynamic;

namespace citools
{
    public class TypedLogDto
    {
        public TypedLogDto()
        {

        }

        public TypedLogDto(object inner)
        {
            Type = inner.GetType().Name;
            Inner = inner;
        }

        public string Type {get; set;}
        public object Inner {get; set;}
    }
}