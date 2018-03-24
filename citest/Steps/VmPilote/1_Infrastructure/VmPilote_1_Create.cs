using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using citest;
using ciinfra;

public class VmPilote_1_Create : IStep {

    private readonly IInfrastructure infrastructure;
    private readonly IAskParameters askParameters;
    private readonly IVmPilote vmPilote;

    public VmPilote_1_Create(
        IInfrastructure infrastructure,
        IAskParameters askParameters)
    {
        this.infrastructure = infrastructure;
        this.askParameters = askParameters;
    }

    public void Test()
    {
        var infrastructureKey = new InfrastructureKey(askParameters.Ask("infrastructureKey"));
        var sshUri = infrastructure.GetVmSshUri(infrastructureKey, "pilote");
        using (var client = infrastructure.Ssh(infrastructureKey, "pilote", "test", "test"))
        {
            var result = client.RunCommand("echo coucou");
            Assert.IsTrue("coucou\n" == result.Result);
        }
        
    }

    public void Run()
    {
        var infrastructureKey = new InfrastructureKey(askParameters.Ask("infrastructureKey"));
        infrastructure.CreateVm(infrastructureKey, "pilote", "test", "test");
    }

    public void Clean()
    {
        var infrastructureKey = new InfrastructureKey(askParameters.Ask("infrastructureKey"));
        infrastructure.DeleteVm(infrastructureKey, "pilote");
    }
}