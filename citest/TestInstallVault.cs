using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using cisystem;
using Autofac;
using citest;
using ciinfra;

public class TestInstallVault<T, U> 
    where T : IInfrastructure 
    where U : IAskParameters {

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
        builder.RegisterType<U>().As<IAskParameters>();
        

        //builder.RegisterModule<CiInfraModule>();
        builder.RegisterModule<CiCliModule>();
        builder.RegisterAssemblyTypes(this.GetType().Assembly);

        var container = builder.Build();
        return container;
    }
}