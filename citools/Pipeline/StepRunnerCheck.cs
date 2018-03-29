using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using citools;

namespace citools
{
    public class StepRunnerCheck : IStepRunner
    {
        private readonly IStepLogger logger;

        public StepRunnerCheck(IStepLogger logger)
        {
            this.logger = logger;
        }
        

        public async Task Run(IStep step)
        {
            await logger.LogCheckOk(step);
        }
    }
}