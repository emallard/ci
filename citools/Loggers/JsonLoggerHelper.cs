using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Dynamic;

namespace citools
{
    public class JsonLoggerHelper
    {
        public string Serialize(object o)
        {
            var jsonLogDto = new TypedLogDto(o);
            var jsonData = JsonConvert.SerializeObject(jsonLogDto);
            return jsonData;
        }
    }
}