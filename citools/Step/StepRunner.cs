using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Autofac;

namespace citools
{
    public class StepRunner
    {
        public async Task SafeRun(params IStep[] steps)
        {
            foreach (var s in steps)
                await this.SafeRun(s);
        }

        // Safe Run : doesn't do a step if TestRunOk is successful
        public async Task SafeRun(IStep step)
        {
            try { 
                await step.TestRunOk(); 
                return;
            }
            catch (Exception) {}

            await step.Run();
        }

        // Test Run : doesn't do a step if TestRunOk is successful
        //            and clean before running the test
        public async Task TestRun(IStep step)
        {
            try { 
                await step.TestRunOk();
                return;
            }
            catch (Exception) {}

            await this.Clean(step);
            await this.Run(step);
        }

        private async Task Run(IStep step)
        {
            try { await step.Run();}
            catch (Exception e4) {throw new StepException(step, e4);}
        }

        private async Task Clean(IStep step)
        {
            try { await step.Clean();}
            catch (Exception e4) {throw new StepException(step, e4);}
        }

        public void RunSync<T>(IContainer container) where T : IStep
        {
             citools.RunSync.Run<T>(container, c => this.Run(c));
        }
    }
}