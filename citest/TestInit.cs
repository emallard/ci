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
        
        builder.RegisterType<ConfigInitDev>().As<IConfigInit>();
        builder.RegisterAssemblyTypes(new Lanceur().GetType().Assembly);

        var container = builder.Build();
        return container;
    }
    
}