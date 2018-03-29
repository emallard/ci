using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace ciinfra
{
    public interface IVmMockLogger
    {
        Task LogCommand(string vmName, string command);
        
        Task LogReboot(string vmName);
    }
}