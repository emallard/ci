using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using cisystem;
using Autofac;
using citest;
using ciinfra;

public class TestInfraPiloteVm<T, U> 
    where T : IInfrastructure 
    where U : IAskParameters {

    CiSystemConfig ciSystemConfig;
    CiSystem ciSystem;
    
    public void Run()
    {
        var container = Init();
        var infrastructure = container.Resolve<IInfrastructure>();
        var askParameters = container.Resolve<IAskParameters>();

        var infrastructureKey = new InfrastructureKey(askParameters.InfrastructureKey);
        infrastructure.CreateVm(
            infrastructureKey, 
            askParameters.PiloteRootPassword, 
            askParameters.PiloteVmName, 
            askParameters.PiloteAdminUser, 
            askParameters.PiloteAdminPassword);

        var sshUri = infrastructure.GetVmSshUri(infrastructureKey, askParameters.PiloteVmName);
        var sshConnection = new SshConnection() {
            SshUri = sshUri,
            User = askParameters.PiloteAdminUser,
            Password = askParameters.PiloteAdminPassword
        };

        var vmPilote = infrastructure.GetVmPilote(sshConnection);
        vmPilote.InstallHosts();
        vmPilote.InstallDocker();
        vmPilote.InstallMirrorRegistry();
        vmPilote.InstallDotNetCoreSdk();
        vmPilote.CloneOrPullCiSources();
        vmPilote.BuildCiUsingSdk();
    }

    void TestOk() 
    {
    }

    void Prerequisite() 
    {
    }

    private IContainer Init()
    {
        var builder = new ContainerBuilder();
        
        builder.RegisterType<T>().As<IInfrastructure>();
        builder.RegisterType<U>().As<IAskParameters>();
        

        //builder.RegisterModule<CiInfraModule>();
        builder.RegisterModule<CiCliModule>();
        builder.RegisterAssemblyTypes(this.GetType().Assembly);

        var container = builder.Build();
        return container;
    }
}