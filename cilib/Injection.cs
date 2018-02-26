using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Autofac;

public class Injection {

    public void Configure(ContainerBuilder builder)
    {
        builder.RegisterType<ConfigDev>().As<IConfig>();
        builder.RegisterType<ConfigInitDev>().As<IConfigInit>();

        builder.RegisterAssemblyTypes(this.GetType().Assembly);
    }
}