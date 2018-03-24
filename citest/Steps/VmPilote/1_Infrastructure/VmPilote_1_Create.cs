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
        var infrastructureKey = new InfrastructureKey(askParameters.InfrastructureKey);
        var sshUri = infrastructure.GetVmSshUri(infrastructureKey, askParameters.PiloteVmName);
        using (var client = infrastructure.Ssh(
            infrastructureKey, 
            askParameters.PiloteVmName, 
            askParameters.PiloteAdminUser, 
            askParameters.PiloteAdminPassword))
        {
            var result = client.RunCommand("echo coucou");
            Assert.IsTrue("coucou\n" == result.Result);
        }
        
    }

    public void Run()
    {
        var infrastructureKey = new InfrastructureKey(askParameters.InfrastructureKey);
        infrastructure.CreateVm(
            infrastructureKey, 
            askParameters.PiloteRootPassword, 
            askParameters.PiloteVmName, 
            askParameters.PiloteAdminUser, 
            askParameters.PiloteAdminPassword);
    }

    public void Clean()
    {
        var infrastructureKey = new InfrastructureKey(askParameters.InfrastructureKey);
        infrastructure.DeleteVm(infrastructureKey, askParameters.PiloteVmName);
    }
}