using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using citools;

namespace citools
{
    public class PipelineRunnerCheck : IPipelineRunner
    {
        public async Task Run(IStep step)
        {
            await step.CheckRunOk();
        }
    }
}