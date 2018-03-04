using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

public class VmPilote_1_Create : IStep {

    private readonly IInfrastructure infrastructure;
    private readonly IVmPilote vmPilote;

    public VmPilote_1_Create(IInfrastructure infrastructure)
    {
        this.infrastructure = infrastructure;
        this.vmPilote = infrastructure.GetVmPilote();
    }

    public void Test()
    {
        var result = vmPilote.SshCommand("echo coucou");
        Assert.IsTrue("coucou\n" == result);
    }

    public void Run()
    {
        infrastructure.CreateVmPilote();
    }

    public void Clean()
    {
        infrastructure.DeleteVmPilote();
    }
}