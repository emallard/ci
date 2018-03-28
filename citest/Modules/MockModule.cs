using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using ciexecommands;
using cilib;
using ciinfra;
using citools;

namespace citest
{
    public class MockModule : Module {

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<StoreResolverInMemory>().As<IStoreResolver>();
            builder.RegisterType<AskMock>().As<IAsk>();

            builder.RegisterType<InfrastructureMock>().As<IInfrastructure>();
            builder.RegisterType<SshClientMock>().As<ISshClient>();
            
        }
        
    }
}