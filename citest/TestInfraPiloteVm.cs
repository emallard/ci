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
using cisteps;
using citools;

public class TestInfraPiloteVm<T, U> 
    where T : IInfrastructure 
    where U : IAskParametersSource {

    CiSystemConfig ciSystemConfig;
    CiSystem ciSystem;
    
    public async Task Run()
    {
        var container = Init();
        var infrastructure = container.Resolve<InfraPiloteCreateVm>();
        var askParameters = container.Resolve<AskParameters>();

        var stepRunner = new StepRunner();

        await stepRunner.SafeRun(
            container.Resolve<InfraPiloteCreateVm>());

        await stepRunner.SafeRun(
            container.Resolve<PiloteInstallDocker>());

        await stepRunner.SafeRun(
            container.Resolve<PiloteInstallMirrorRegistry>());

        await stepRunner.SafeRun(
            container.Resolve<PiloteInstallMirrorRegistry>());
    
        await stepRunner.SafeRun(
            container.Resolve<PiloteInstallDotNetCoreSdk>());
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