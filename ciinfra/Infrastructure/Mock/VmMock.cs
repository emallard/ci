using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace ciinfra
{
    public class VmMock
    {
        public string Name {get; set;}
        public Uri SshUri {get; set;}
        public List<string> commands = new List<string>();

        public void Command(string command)
        {
            this.commands.Add(command);
        }
    }
}