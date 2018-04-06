using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using citools;

namespace citools
{
    public class StepRunnerCleanAll : IStepRunner
    {
        private readonly StepLogger logger;

        public StepRunnerCleanAll(StepLogger logger)
        {
            this.logger = logger;
        }
        

        public async Task Run(IStep step)
        {
            await logger.Enter(step);

            try { 
                await logger.LogClean(step);
            }
            catch (Exception) {}

            await logger.Exit(step);
        }
    }
}