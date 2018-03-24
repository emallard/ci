using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Renci.SshNet;
using System.Threading;
using System.Text.RegularExpressions;
using System.IO;
using ciinfra;

namespace ciinfra
{
    public class VBoxVmWebServer : VBoxVm, IVmWebServer {


        public VBoxVmWebServer()
        {
        }

        public void InstallHosts(string vmPiloteIp, string vmPilotePrivateRegistryDomain)
        {
            this.SshSudoBashCommand($"echo \"{vmPiloteIp}  {vmPilotePrivateRegistryDomain}\" >> /etc/hosts");
        }

        public void CleanHosts(string vmPilotePrivateRegistryDomain)
        {
            this.SshSudoBashCommand($"sed -i \"/ {vmPilotePrivateRegistryDomain}/d\" /etc/hosts");
        }
    }
}