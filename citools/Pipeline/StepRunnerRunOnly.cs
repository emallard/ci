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
        private readonly StepLogger logger;

        public StepRunnerRunOnly(StepLogger logger)
        {
            this.logger = logger;
        }

        public async Task Run(IStep step)
        {
            await logger.LogRun(step);
        }
    }
}