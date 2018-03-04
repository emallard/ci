using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

public class VmPilote_1_Create : IStep {

    private readonly IInfrastructure infrastructure;

    public VmPilote_1_Create(IInfrastructure infrastructure)
    {
        this.infrastructure = infrastructure;
    }

    public void Test()
    {
        var vmPilote = infrastructure.GetVmPilote();
        using (var sshClient = vmPilote.Ssh())
        {
            var cmd = sshClient.RunCommand("echo coucou");
            Assert.IsTrue("coucou\n" == cmd.Result);
        }
    }

    public void Run()
    {
        infrastructure.CreateVmPilote();
    }

    public void Revert()
    {
        infrastructure.DeleteVmPilote();
    }
}