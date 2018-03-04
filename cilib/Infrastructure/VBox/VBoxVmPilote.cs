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

    public void RunCiContainer()
    {
        this.SshCommand("docker run --name ciexe ciexe");
    }

    public void CreateBuildContainer()
    {
        throw new NotImplementedException();
    }

    public void SetSourcesInBuildContainer()
    {
        throw new NotImplementedException();
    }

    public void RunBuildContainer()
    {
        throw new NotImplementedException();
    }

    public void CreateAppContainer()
    {
        throw new NotImplementedException();
    }

    public void PublishToAppRegistry()
    {
        throw new NotImplementedException();
    }

    
}