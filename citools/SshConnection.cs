using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citools
{
    public class SshConnection
    {
        public Uri SshUri {get; set;}
        public string User  {get; set;}
        public string Password  {get; set;}
    }
}