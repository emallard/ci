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

    // Vm pilotes is bound to 2 volumes
    // 1) to be able to use the docker socket ans thus the docker engine api
    // 2) a directory to store data.

    string volume1 = "--volume /var/run/docker.sock:/var/run/docker.sock";
    string volume2 = "--volume ~/cidata:/cidata";

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
        this.SshSudoCommand($"echo \"{Ip}  {PrivateRegistryDomain}\" >> /etc/hosts");
    }
    
    public void CleanHosts()
    {
        this.SshSudoCommand($"sed -i \"/ {PrivateRegistryDomain}/d\" /etc/hosts");
    }

    public string SshDockerRun(string arg)
    {
        return this.SshCommand($"docker run --name ciexe --rm {volume1} {volume2} ciexe " + arg);
    }

    public void InstallCA()
    {
        SshDockerRun("install-ca");
    }

    public void CleanCA()
    {
        SshDockerRun("clean-ca");
    }

    public void InstallPrivateRegistry()
    {
        SshDockerRun("install-privateregistry");
    }

    public void CleanPrivateRegistry()
    {
        SshDockerRun("clean-privateregistry");
    }

    public void InstallVault()
    {
        SshDockerRun("install-vault");
    }

    public void Build()
    {
        SshDockerRun("build");
    }

    public void Publish()
    {
        SshDockerRun("publish");
    }

}