using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace cisystem
{
    public class CiBuild {
        public string name;

        public string gitUrl;
        public string branch;
        
        public string image;
        public string tag;
    }

    public class CiBuildResult {
        
        public string image;
        public string tag;
        
    }
}