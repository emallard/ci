using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using cisystem;
using Autofac;
using citest;
using ciinfra;
using cicli;

public class TestInstallVault<T, U> 
    where T : IInfrastructure 
    where U : IAskParametersSource {

    CiSystemConfig ciSystemConfig;
    CiSystem ciSystem;
    
    public void Run()
    {
        
    }

    void TestOk() 
    {
        // Run http request
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
        builder.RegisterModule<CiCliModule>();
        builder.RegisterAssemblyTypes(this.GetType().Assembly);

        var container = builder.Build();
        return container;
    }
}