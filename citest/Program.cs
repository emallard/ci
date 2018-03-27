using System;
using System.IO;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using ciinfra;
using cisteps;
using citools;

namespace citest
{
    class Program
    {
        static void Main(string[] args)
        {
            string moduleTypeStr = typeof(TestModule).Name;
            string pipelineTypeStr = typeof(PipelineInit).Name;
            string pipelineRunnerTypeStr = typeof(PipelineRunnerCheck).Name;

            var fileName = "bin/lastRun";
            if (File.Exists(fileName))
            {
                var lastRun = File.ReadAllLines(fileName);
                moduleTypeStr = lastRun[0];
                pipelineTypeStr = lastRun[1];
                pipelineRunnerTypeStr = lastRun[2];
            }

            var module = typeof(TestModule).Assembly.GetType(moduleTypeStr);
            var pipelineType = typeof(PipelineInit).Assembly.GetType(pipelineTypeStr);
            var pipelineRunnerType = typeof(PipelineRunnerTest).Assembly.GetType(pipelineRunnerTypeStr);

            File.WriteAllLines(fileName, new string[] {
                moduleTypeStr,
                pipelineTypeStr,
                pipelineRunnerTypeStr
            });

            Run(module, pipelineType, pipelineRunnerType).Wait();
        }   


        static void Main1(string[] args)
        {
            var module = typeof(TestModule);
            var pipelineType = typeof(PipelineInit);
            var pipelineRunnerType = typeof(PipelineRunnerTest);
            Run(module, pipelineType, pipelineRunnerType).Wait();
        }   

        static async Task Run<TModule, TPipeline, TPipelineRunner>() 
            where TModule : IModule, new()
            where TPipeline : IPipeline
            where TPipelineRunner : IPipelineRunner
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<TModule>();

            var container = builder.Build();
            var pipeline = container.Resolve<TPipeline>();
            var pipelineRunner = container.Resolve<TPipelineRunner>();
            await pipeline.Run(pipelineRunner);
        }


        static async Task Run(Type moduleType, Type pipelineType, Type pipelineRunnerType)
        {
            var builder = new ContainerBuilder();

            var module =  (IModule)Activator.CreateInstance(moduleType);
            var container = builder.Build();
            var pipeline = (IPipeline) container.Resolve(pipelineType);
            var pipelineRunner = (IPipelineRunner) container.Resolve(pipelineRunnerType);
            await pipeline.Run(pipelineRunner);
        }

    }
}
