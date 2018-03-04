using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

public class VmPilote_1_Hosts : IStep {

    private readonly IInfrastructure infrastructure;
    private readonly IVmPilote vmPilote;

    public VmPilote_1_Hosts(IInfrastructure infrastructure)
    {
        this.infrastructure = infrastructure;
        this.vmPilote = infrastructure.GetVmPilote();
    }

    public void Test()
    {
        var result = vmPilote.SshCommand($"getent hosts {vmPilote.PrivateRegistryDomain}");
        Assert.Contains(vmPilote.PrivateRegistryDomain, result);
    }

    public void Run()
    {
        vmPilote.InstallHosts();
    }

    public void Clean()
    {
        vmPilote.CleanHosts();
    }
}