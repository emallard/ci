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
    class CommandLineRun
    {
        public async Task Run<TLogger, TModule, TRunner>(Type pipelineType) 
            where TLogger : ILogger
            where TModule : IModule, new()
            where TRunner : IStepRunner
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<CommonModule>();

            builder.RegisterType<TLogger>().As<ILogger>().SingleInstance();

            builder.RegisterModule<TModule>();
            builder.RegisterType<TRunner>().As<IStepRunner>();
            var container = builder.Build();

            var pipeline = (IPipeline) container.Resolve(pipelineType);
            await pipeline.Run();
        }
    }
}
