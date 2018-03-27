using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Autofac.Core;
using Autofac;
using ciinfra;
using citools;

namespace citest
{
    public class VBoxIntegrationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterAssemblyTypes(this.GetType().Assembly);
            //builder.RegisterModule<CiLibModule>();

            builder.RegisterType<VBoxInfrastructure>().As<IInfrastructure>();
            builder.RegisterType<StoreResolverVault>().As<IStoreResolver>();

        }
    }
}