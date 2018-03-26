using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Autofac;

namespace citest
{
    
    
    public class TestRunner {

        IContainer container;
        public bool TestOnly;

        public void SetContainer(IContainer container)
        {
            this.container = container;
        }

        public void Run<S>() where S : IStep
        {
            var s = container.Resolve<S>();
            Run(s);
        }

        public void Run(IStep step)
        {
            Console.WriteLine("==== " + step.GetType().Name);

            if (TestOnly)
            {
                try {
                    step.Test();
                    Console.WriteLine("- OK");
                }
                catch (Exception)
                {
                    Console.WriteLine("- FAIL");
                }
                return;
            }

            
            try {
                step.Test();
                Console.WriteLine("- Nothing to do");
            }
            catch (Exception)
            {
                Console.WriteLine("- Clean");
                step.Clean();
                Console.WriteLine("- Run");
                step.Run();
                Console.WriteLine("- Test");
                step.Test();
            }
        }
        
        public void Clean<S>() where S : IStep
        {
            if (TestOnly)
                return;

            var s = container.Resolve<S>();
            s.Clean();;
        }

        public void ForceRun<S>() where S : IStep
        {
            if (TestOnly)
                return;
                
            var s = container.Resolve<S>();
            ForceRun(s);
        }

        public void ForceRun(IStep step)
        {
            if (TestOnly)
                return;

            Console.WriteLine("==== ForceRun Step " + step.GetType().Name);

            Console.WriteLine("- Step clean");
            step.Clean();
            Console.WriteLine("- Step run");
            step.Run();
            Console.WriteLine("- Step test");
            step.Test();
        }
    }
}