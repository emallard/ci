using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using citools;

namespace citools
{
    public class StepRunnerRunOnly : IStepRunner
    {
        public async Task Run(IStep step)
        {
            await step.Run();
        }
    }
}