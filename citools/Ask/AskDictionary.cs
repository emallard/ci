using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citools
{

    public class AskDictionary : IAsk
    {
        public Dictionary<string, string> dic = new Dictionary<string, string>();
        public Task<string> GetValue(string key)
        {
            var val = dic[key];
            return new Task<string>(() => val);
        }
    }
}