

using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace cisystem
{
    public class CiSystemConfigJson {
        
        public CiSystemConfig Deserialize(string contents)
        {
            return JsonConvert.DeserializeObject<CiSystemConfig>(contents);
        }

        public string Serialize(CiSystemConfig config)
        {
            return JsonConvert.SerializeObject(config);
        }

    }
}