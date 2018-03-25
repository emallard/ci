using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citools
{
    public interface IStep 
    {
        Task Need();

        Task Ask();

        Task Run();
        
        Task Keep();

        Task TestRunOk();

        Task TestAlreadyRun();

        Task Clean();        
    }
}