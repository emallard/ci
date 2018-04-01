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
            // Resources
            builder.RegisterType<StoreResolverInMemory>().As<IStoreResolver>().SingleInstance();            
            builder.RegisterType<AskMock>().As<IAsk>().SingleInstance();
            
            // Infrastructure
            builder.RegisterType<InfrastructureMock>().As<IInfrastructure>().SingleInstance();
            builder.RegisterType<SshClientMock>().As<ISshClient>();
            builder.RegisterType<VmMockLoggerConsole>().As<IVmMockLogger>().SingleInstance();
        }
        
    }
}