using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Renci.SshNet;

namespace ciinfra
{
    public interface IVmWebServer : IVm {

        void InstallHosts(string vmPiloteIp, string vmPilotePrivateRegistryDomain);
        void CleanHosts(string vmPilotePrivateRegistryDomain);
        void InstallDocker();
        void InstallMirrorRegistry();
        
    }
}