using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using citools;

namespace cisteps
{
    public class AskHelper
    {
        private readonly IAsk ask;

        public AskHelper(
            IAsk ask)
        {
            this.ask = ask;
        }

        Dictionary<string,string> dicAsk = new Dictionary<string, string>();

        public async Task<string> Ask(string key)
        {
            string val;
            if (!dicAsk.TryGetValue(key, out val))
            {
                try { val = await ask.GetValue(key); }
                catch (Exception e) {throw new AskException(e);}
                dicAsk[key] = val;
            }
            return val;
        }
    }
}