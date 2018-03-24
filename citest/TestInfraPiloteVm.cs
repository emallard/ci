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

        var infrastructureKey = new InfrastructureKey(askParameters.Ask("infrastructureKey"));
        infrastructure.CreateVm(infrastructureKey, "pilote", askParameters.Ask("piloteAdminUser"), askParameters.Ask("piloteAdminPassword"));
        
        var piloteSshConnection = new SshConnection();
        piloteSshConnection.SshUri = infrastructure.GetVmSshUri("pilote");
        piloteSshConnection.user = askParameters.Ask("piloteAdminUser");
        piloteSshConnection.password = askParameters.Ask("piloteAdminPassword");

        var vmPilote = infrastructure.GetVmPilote(piloteSshConnection);
        vmPilote.InstallHosts();
        vmPilote.InstallDocker();
        vmPilote.InstallMirrorRegistry();
        vmPilote.InstallDotNetCoreSdk();
        vmPilote.CloneOrPullCiSources();
        vmPilote.BuildCiUsingSdk();
    }

    void TestOk() 
    {
        // Run http request
    }

    void Prerequisite() 
    {
        // Check that VM call "webserver is setup
        ciSystem.CheckVmSsh(ciSystem.Pilote);
        ciSystem.CheckVmSsh(ciSystem.WebServer);

        ciSystem.CheckCiexe(ciSystem.Pilote);
        ciSystem.CheckCiexe(ciSystem.WebServer);

        ciSystem.CheckPrivateRegistry(ciSystem.Pilote);
        ciSystem.CheckPrivateRegistryConnection(ciSystem.WebServer, ciSystem.Pilote);

        ciSystem.CheckTraefik(ciSystem.WebServer);
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