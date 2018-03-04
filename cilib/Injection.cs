using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Autofac;

public class Injection {

    public void Configure(ContainerBuilder builder)
    {
        builder.RegisterType<ConfigVBox>().As<IConfig>();

        builder.RegisterAssemblyTypes(this.GetType().Assembly);
    }


    public void ConfigureProd(ContainerBuilder builder)
    {
        builder.RegisterType<ConfigVBox>().As<IConfig>();

        builder.RegisterAssemblyTypes(this.GetType().Assembly);
    }
}