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
        
        public async Task Run(IStep step)
        {
            try { await step.TestAlreadyRun();}
            catch (Exception e1) {throw new AlreadyRunException(step, e1);}
/*
            try { await step.Need();}
            catch (Exception e2) {throw new NeedException(step, e2);}

            try { await step.Ask();}
            catch (Exception e3) {throw new AskException(step, e3);}
*/
            try { await step.Run();}
            catch (Exception e4) {throw new RunException(step, e4);}
/*
            try { await step.Keep();}
            catch (Exception e5) {throw new KeepException(step, e5);}
*/
            try { await step.TestRunOk();}
            catch (Exception e6) {throw new RunOkException(step, e6);}


        }

        public void RunSync<T>(IContainer container) where T : IStep
        {
             citools.RunSync.Run<T>(container, c => this.Run(c));
        }
    }
}