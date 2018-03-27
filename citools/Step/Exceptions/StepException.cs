using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citools
{
    public class StepException : Exception
    {
        public IStep step;

        public StepException(IStep step, Exception inner) :
            base(step.GetType().Name, inner)
        {
            this.step = step;
        }
    }
}