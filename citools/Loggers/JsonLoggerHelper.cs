using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Dynamic;

namespace citools
{

    public class JsonLogDto
    {
        public string Type {get; set;}
        public object Inner {get; set;}
    }
    
    public class JsonLoggerHelper
    {
        public string Serialize(object o)
        {
            var jsonLogDto = new JsonLogDto();
            jsonLogDto.Type = o.GetType().Name;
            jsonLogDto.Inner = o;
            
            var jsonData = JsonConvert.SerializeObject(jsonLogDto);
            return jsonData;
        }
    }
}