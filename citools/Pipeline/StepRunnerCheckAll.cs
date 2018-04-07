using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using citools;

namespace citools
{
    public class StepRunnerCheckAll : IStepRunner
    {
        private readonly StepLogger logger;

        public StepRunnerCheckAll(StepLogger logger)
        {
            this.logger = logger;
        }
        

        public async Task Run(IStep step)
        {
            await logger.Enter(step);

            try { await logger.LogCheck(step);}
            catch (Exception) {}

            await logger.Exit(step);
        }
    }
}