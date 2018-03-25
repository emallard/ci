using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citools
{
    public interface IStep 
    {
        void Need();

        void Ask();

        void Run();
        
        void Keep();

        void TestRunOk();

        void TestAlreadyRun();

        void Clean();        
    }
}