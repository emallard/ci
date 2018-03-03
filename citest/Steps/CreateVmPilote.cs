using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

public class CreateVmPilote : IStep {

    private readonly IInfrastructure infrastructure;

    public CreateVmPilote(IInfrastructure infrastructure)
    {
        this.infrastructure = infrastructure;
    }

    public void Test()
    {
        var vmPilote = infrastructure.GetVmPilote();
        using (var sshClient = vmPilote.Connect())
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