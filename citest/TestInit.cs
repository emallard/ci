using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Autofac;

public class TestInit {

    public IContainer Init()
    {
        var builder = new ContainerBuilder();
        
        builder.RegisterType<ConfigDev>().As<IConfig>();
        builder.RegisterType<ConfigInitDev>().As<IConfigInit>();
        builder.RegisterAssemblyTypes(new ConfigDev().GetType().Assembly);

        var container = builder.Build();
        return container;
    }
    
}