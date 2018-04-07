using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using ciexecommands;
using ciinfra;
using cilib;
using cisteps;
using citools;

namespace citest
{
    class CommandLineRunCheck
    {
        public async Task Run(Type pipelineType)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<CommonModule>();

            builder.RegisterType<ConsoleStepLogger>().As<ILogger>().SingleInstance();

            builder.RegisterModule<MockModule>();
            builder.RegisterType<StepRunnerCheck>().As<IStepRunner>();
            var container = builder.Build();

            var pipeline = (IPipeline) container.Resolve(pipelineType);
            await pipeline.Run();
        }
    }
}
