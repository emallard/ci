using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citools
{
    public class StepLoggerAllConsole : IStepLogger
    {
        public async Task LogCheckOk(IStep step)
        {
            Console.WriteLine("Step Check " + step.GetType().Name);
            await step.Check();
            Console.WriteLine("Step Check OK");
        }

        public async Task LogClean(IStep step)
        {
            Console.WriteLine("Step Clean " + step.GetType().Name);
            await step.Clean();
            Console.WriteLine("Step Clean OK");
        }

        public async Task LogRun(IStep step)
        {
            Console.WriteLine("Step Run " + step.GetType().Name);
            await step.Run();
            Console.WriteLine("Step Run OK");
        }
    }
}