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
            builder.RegisterType<StoreResolverInMemory>().As<IStoreResolver>().SingleInstance();
            builder.RegisterType<StoreResourceLoggerConsole>().As<IStoreResourceLogger>().SingleInstance();

            builder.RegisterType<AskMock>().As<IAsk>().SingleInstance();
            builder.RegisterType<AskResourceLoggerConsole>().As<IAskResourceLogger>().SingleInstance();

            builder.RegisterType<InfrastructureMock>().As<IInfrastructure>().SingleInstance();
            builder.RegisterType<SshClientMock>().As<ISshClient>();

            builder.RegisterType<StepLoggerAllConsole>().As<IStepLogger>().SingleInstance();
        }
        
    }
}