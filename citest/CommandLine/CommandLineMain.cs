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
    class CommandLineMain
    {
        public void Main(string[] args)
        {
            string pipelineTypeStr = typeof(PipelineDevOpOnlyFull).Name;
            string runFuncStr = "Run";

            var fileName = "ciTestLastRun";
            if (File.Exists(fileName))
            {
                var lastRun = File.ReadAllLines(fileName);
                pipelineTypeStr = lastRun[0];
                runFuncStr = lastRun[1];
            }

            var pipelineType = typeof(CiStepsModule).Assembly.GetTypes().First(t => t.Name == pipelineTypeStr);

            File.WriteAllLines(fileName, new string[] {
                pipelineTypeStr,
                runFuncStr
            });

            if (runFuncStr == "Run")
                Run(pipelineType).Wait();
            if (runFuncStr == "RunIntegration")
                RunIntegration(pipelineType).Wait();
        }   

        static async Task Run(Type pipelineType)
        {
            await new CommandLineRun().Run<ConsoleAndFileStepLogger, MockModule, StepRunnerCheckAll>(pipelineType);
            await new CommandLineRun().Run<ConsoleAndFileStepLogger, MockModule, StepRunnerRunAll>(pipelineType);
        }

        static async Task RunIntegration(Type pipelineType)
        {
            await new CommandLineRun().Run<ConsoleAndFileStepLogger, VBoxIntegrationModule, StepRunnerTest>(pipelineType);
        }

    }
}
