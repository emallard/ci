using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using ciinfra;
using cilib;
using cisteps;
using citools;

namespace citest
{
    class Program
    {
        static void Main(string[] args)
        {
            string moduleTypeStr = typeof(MockModule).Name;
            string pipelineTypeStr = typeof(PipelineInit).Name;
            string pipelineRunnerTypeStr = typeof(StepRunnerCheck).Name;

            var fileName = "bin/lastRun";
            if (File.Exists(fileName))
            {
                var lastRun = File.ReadAllLines(fileName);
                moduleTypeStr = lastRun[0];
                pipelineTypeStr = lastRun[1];
                pipelineRunnerTypeStr = lastRun[2];
            }

            var module = typeof(MockModule).Assembly.GetTypes().First(t => t.Name == moduleTypeStr);
            var pipelineType = typeof(PipelineInit).Assembly.GetTypes().First(t => t.Name == pipelineTypeStr);
            var pipelineRunnerType = typeof(StepRunnerTest).Assembly.GetTypes().First(t => t.Name == pipelineRunnerTypeStr);

            File.WriteAllLines(fileName, new string[] {
                moduleTypeStr,
                pipelineTypeStr,
                pipelineRunnerTypeStr
            });

            Run(module, pipelineType, pipelineRunnerType).Wait();
        }   


        static void Main1(string[] args)
        {
            var module = typeof(MockModule);
            var pipelineType = typeof(PipelineInit);
            var pipelineRunnerType = typeof(StepRunnerTest);
            Run(module, pipelineType, pipelineRunnerType).Wait();
        }   



        static async Task Run(Type moduleType, Type pipelineType, Type pipelineRunnerType)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<CiLibModule>();
            builder.RegisterModule<CiToolsModule>();
            builder.RegisterModule<CiStepsModule>();
            builder.RegisterModule<CiInfraModule>();

            var module =  (IModule)Activator.CreateInstance(moduleType);
            builder.RegisterModule(module);
            builder.RegisterType(pipelineRunnerType).As(typeof(IStepRunner));
            var container = builder.Build();
            var pipeline = (IPipeline) container.Resolve(pipelineType);
            await pipeline.Run();
        }

    }
}
