using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

public class VmCiCli {

    // Vm pilotes is bound to 2 volumes
    // 1) to be able to use the docker socket ans thus the docker engine api
    // 2) a directory to store data.
    
    IVm vm;

    string volume1 = "--volume /var/run/docker.sock:/var/run/docker.sock ";
    string volume2 = "--volume /home/test/cidata:/cidata ";

    public VmCiCli SetVm(IVm vm)
    {
        this.vm = vm;
        return this;
    }

    public string SshDockerRun(string arg)
    {
        return vm.SshCommand(DockerRun(arg));
    }

    public void InstallCA()
    {
        //SshDockerRun("install-ca");
        vm.SshScript(DockerRun("install-ca"), "installca.sh");
    }

    public void CleanCA()
    {
        vm.SshScript(DockerRun("clean-ca"), "cleanca.sh");
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

    public void BuildWebApp()
    {
        SshDockerRun("build");
    }

    public void PublishWebApp()
    {
        SshDockerRun("publish");
    }

    private string DockerRun(string arg)
    {
        return "docker run --name ciexe --rm " + volume1 + " " + volume2 + " ciexe " + arg;
    }
}