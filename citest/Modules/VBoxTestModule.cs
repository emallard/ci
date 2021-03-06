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
    public class VBoxTestModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<StoreResolverInMemory>().As<IStoreResolver>();
            builder.RegisterType<AskMock>().As<IAsk>();

            builder.RegisterType<VBoxInfrastructure>().As<IInfrastructure>();
            builder.RegisterType<RenciSshClient>().As<ISshClient>();
        }
    }
}