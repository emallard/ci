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
        private readonly IStepLogger logger;

        public StepRunnerCleanAll(IStepLogger logger)
        {
            this.logger = logger;
        }
        

        public async Task Run(IStep step)
        {
            try { 
                await logger.LogClean(step);
            }
            catch (Exception) {}
        }
    }
}