using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Renci.SshNet;
using System.Threading;
using System.Text.RegularExpressions;
using System.IO;

public class VBoxVmPilote : VBoxVm, IVmPilote {

    public string VmName => "pilote";
    public IPAddress Ip => new IPAddress(new byte[]{10,0,2,5});
    public int PortForward => 22005;
    public int PrivateRegistryPort => 5443;
    public string PrivateRegistryDomain => "privateregistry.mynetwork.local";

    public VBoxVmPilote()
    {
    }

    public void InstallHosts()
    {
        this.SshSudoBashCommand($"echo \"{Ip}  {PrivateRegistryDomain}\" >> /etc/hosts");
    }
    
    public void CleanHosts()
    {
        this.SshSudoBashCommand($"sed -i \"/ {PrivateRegistryDomain}/d\" /etc/hosts");
    }

}