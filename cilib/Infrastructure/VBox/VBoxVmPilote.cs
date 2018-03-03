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
        using (var client = Connect())
        {
            var cmd = client.RunCommand("docker run ciexe install-registry");
            var wait = cmd.Result;
        }
    }

    public void InstallVault()
    {
        using (var client = Connect())
        {
            var cmd = client.RunCommand("docker run ciexe install-vault");
            var wait = cmd.Result;
        }
    }

    public void ConfigureDotNetBuildContainer(string name)
    {

    }

}