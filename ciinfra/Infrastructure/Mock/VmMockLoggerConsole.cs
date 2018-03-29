using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace ciinfra
{
    public class VmMockLoggerConsole : IVmMockLogger
    {
        public async Task LogCommand(string vmName, string command)
        {
            await Task.CompletedTask;
            Console.WriteLine("Ssh " + vmName + " : " + command);
        }

        public async Task LogReboot(string vmName)
        {
            await Task.CompletedTask;
            Console.WriteLine("Reboot : " + vmName);
        }
    }
}