using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Renci.SshNet;
using System.Threading;
using System.Text.RegularExpressions;
using System.IO;

public class VBoxVmPilote : VBoxVmCommon, IVmPilote {

    public VBoxVmPilote()
    {
    }

    public void InstallRegistry()
    {
        using (var client = Ssh())
        {
            var cmd = client.RunCommand("docker run ciexe install-registry");
            var wait = cmd.Result;
        }
    }

    public void InstallVault()
    {
        using (var client = Ssh())
        {
            var cmd = client.RunCommand("docker run ciexe install-vault");
            var wait = cmd.Result;
        }
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