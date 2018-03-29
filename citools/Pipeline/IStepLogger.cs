using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citools
{
    public interface IStepLogger
    {
        Task LogRun(IStep step);

        Task LogCheckOk(IStep step);

        Task LogClean(IStep step);
    }
}