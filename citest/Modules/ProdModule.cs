using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Autofac.Core;
using Autofac;
using citools;
using ciinfra;

namespace citest
{
    public class ProdModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<StoreResolverVault>().As<IStoreResolver>();
            builder.RegisterType<AskReadLine>().As<IAsk>();

            builder.RegisterType<GandiInfrastructure>().As<IInfrastructure>();
            builder.RegisterType<RenciSshClient>().As<ISshClient>();
        }
    }
}