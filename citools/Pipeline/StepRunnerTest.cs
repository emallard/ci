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
        public async Task Run(IStep step)
        {
            try {
                await step.CheckRunOk();
                return;
            }
            catch (Exception) {}

            try { await step.Clean();}
            catch (Exception e4) {throw new StepException(step, e4);}

            try { await step.Run();}
            catch (Exception e4) {throw new StepException(step, e4);}
        }
    }
}