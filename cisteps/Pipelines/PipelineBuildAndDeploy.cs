using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using citools;

namespace cisteps
{   
    public class PipelineBuildAndDeploy : IPipeline
    {
        public async Task Run()
        {
            await Task.CompletedTask;
        }
    }
}