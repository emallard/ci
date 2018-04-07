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
        public string RootPassword {get; set;}
        public string AdminUser {get; set;}
        public string AdminPassword {get; set;}


        public List<string> commands = new List<string>();
        private readonly IVmMockLogger vmMockLogger;

        public VmMock(IVmMockLogger vmMockLogger)
        {
            this.vmMockLogger = vmMockLogger;
        }

        public void Command(string command)
        {
            vmMockLogger.LogCommand(Name, command);
            this.commands.Add(command);
        }

        public void CommandWithStdIn(string command, string stdIn)
        {
            vmMockLogger.LogCommand(Name, command + stdIn);
            this.commands.Add(command + stdIn);
        }

        public void Reboot()
        {
            vmMockLogger.LogReboot(Name);
            this.commands.Add("Reboot");
        }
    }
}