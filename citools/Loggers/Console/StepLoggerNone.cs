using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citools
{
    public class StepLoggerNone : IStepLogger
    {
        public async Task LogCheckOk(IStep step)
        {
            await step.Check();
        }

        public async Task LogClean(IStep step)
        {
            await step.Clean();
        }

        public async Task LogRun(IStep step)
        {
            await step.Run();
        }
    }
}