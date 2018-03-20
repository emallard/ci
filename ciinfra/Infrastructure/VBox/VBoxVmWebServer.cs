using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Renci.SshNet;
using System.Threading;
using System.Text.RegularExpressions;
using System.IO;

public class VBoxVmWebServer : VBoxVm, IVmWebServer {

    public string VmName => "webserver";
    public IPAddress Ip => new IPAddress(new byte[]{10,0,2,6});
    public int PortForward => 22006;
    
    IVmPilote vmPilote;

    public VBoxVmWebServer()
    {
    }

    public void SetVmPilote(IVmPilote vmPilote)
    {
        this.vmPilote = vmPilote;
    }

    public void InstallHosts()
    {
        this.SshSudoBashCommand($"echo \"{vmPilote.Ip}  {vmPilote.PrivateRegistryDomain}\" >> /etc/hosts");
    }

    public void CleanHosts()
    {
        this.SshSudoBashCommand($"sed -i \"/ {vmPilote.PrivateRegistryDomain}/d\" /etc/hosts");
    }
}