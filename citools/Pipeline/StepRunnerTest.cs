using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using citools;

namespace citools
{
    public class StepRunnerTest : IStepRunner
    {
        private readonly StepLogger logger;

        public StepRunnerTest(StepLogger logger)
        {
            this.logger = logger;
        }

        public async Task Run(IStep step)
        {
            try {
                await logger.LogCheck(step);
                return;
            }
            catch (Exception) {}

            try { await logger.LogClean(step);}
            catch (Exception e4) {throw new StepException(step, e4);}

            try { await logger.LogRun(step);}
            catch (Exception e4) {throw new StepException(step, e4);}
        }
    }
}