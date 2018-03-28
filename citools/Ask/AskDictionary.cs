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
        public async Task<string> GetValue(string key)
        {
            await Task.CompletedTask;
            var val = dic[key];
            return val;
        }
    }
}