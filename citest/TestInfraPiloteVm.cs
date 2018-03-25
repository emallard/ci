using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using cisystem;
using Autofac;
using citest;
using ciinfra;
using ciexecommands;

public class TestInfraPiloteVm<T, U> 
    where T : IInfrastructure 
    where U : IAskParametersSource {

    CiSystemConfig ciSystemConfig;
    CiSystem ciSystem;
    
    public void Run()
    {
        var container = Init();
        var infrastructure = container.Resolve<IInfrastructure>();
        var askParameters = container.Resolve<AskParameters>();

        infrastructure.CreateVm(
            askParameters.InfrastructureKey, 
            askParameters.PiloteRootPassword, 
            askParameters.PiloteVmName, 
            askParameters.PiloteAdminUser, 
            askParameters.PiloteAdminPassword);

        var vmPilote = infrastructure.GetVmPilote(askParameters.PiloteSshConnection());
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
        builder.RegisterType<U>().As<IAskParametersSource>();
        

        //builder.RegisterModule<CiInfraModule>();
        builder.RegisterModule<CiExeCommandsModule>();
        builder.RegisterAssemblyTypes(this.GetType().Assembly);

        var container = builder.Build();
        return container;
    }
}