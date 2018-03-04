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

    public VBoxVmPilote()
    {
    }

    public void InstallRegistry()
    {
        this.SshCommand("docker start && docker exec dotnet ciexe.dll install-registry");
    }

    public void InstallVault()
    {
        this.SshCommand("docker run ciexe install-vault");
    }

    public void RunCiContainer(string args)
    {
        this.SshCommand("docker run --rm --name ciexe ciexe " + args);
    }

    public void Build()
    {
        var v1 = "-v /var/run/docker.sock:/var/run/docker.sock";
        var v2 = "";//"-v ~/sources/:/sources";
        
        this.SshCommand($"docker run --rm --name ciexe {v1} {v2} ciexe build");
    }

    public void Publish()
    {
        throw new NotImplementedException();
    }
}