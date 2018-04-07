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
    class CommandLineMain_Old
    {
        public void Main(string[] args)
        {
            string moduleTypeStr = typeof(MockModule).Name;
            string pipelineTypeStr = typeof(PipelineFull).Name;
            string stepRunnerTypeStr = typeof(StepRunnerRunOnly).Name;

            var fileName = "ciTestLastRun";
            if (File.Exists(fileName))
            {
                var lastRun = File.ReadAllLines(fileName);
                moduleTypeStr = lastRun[0];
                pipelineTypeStr = lastRun[1];
                stepRunnerTypeStr = lastRun[2];
            }

            var moduleType = typeof(MockModule).Assembly.GetTypes().First(t => t.Name == moduleTypeStr);
            var pipelineType = typeof(CiStepsModule).Assembly.GetTypes().First(t => t.Name == pipelineTypeStr);
            var stepRunnerType = typeof(CiToolsModule).Assembly.GetTypes().First(t => t.Name == stepRunnerTypeStr);

            File.WriteAllLines(fileName, new string[] {
                moduleTypeStr,
                pipelineTypeStr,
                stepRunnerTypeStr
            });

            Run(moduleType, pipelineType, stepRunnerType).Wait();
        }   


        static void Main1(string[] args)
        {
            var module = typeof(MockModule);
            var pipelineType = typeof(PipelineDevopInit);
            var stepRunnerType = typeof(StepRunnerTest);
            Run(module, pipelineType, stepRunnerType).Wait();
        }   



        static async Task Run(Type moduleType, Type pipelineType, Type stepRunnerType)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<CommonModule>();

            //builder.RegisterType<ConsoleLogger>().As<ILogger>();
            builder.RegisterType<ConsoleStepLogger>().As<ILogger>().SingleInstance();

            //builder.RegisterModule<LogRequestsModule>();

            var module =  (IModule)Activator.CreateInstance(moduleType);
            builder.RegisterModule(module);
            builder.RegisterType(stepRunnerType).As<IStepRunner>();
            var container = builder.Build();
            
            //container.Resolve<PipelineInstallPiloteVm>();
            //container.Resolve<IStepRunner>();


            var pipeline = (IPipeline) container.Resolve(pipelineType);
            await pipeline.Run();
        }

    }
}
