using System;
using System.Threading.Tasks;
using Autofac;
using ciinfra;

namespace citest
{
    class Program
    {
        static void Main(string[] args)
        {
            
            //var runner = new TestRunner();
            //var test = new Scenario1<VBoxInfrastructure>(runner);
            //var test = new Scenario1<GandiInfrastructure>(runner);
            //test.RunAll();
            

            var test = new TestInstallVault<VBoxInfrastructure, AskParametersSourceMock>();
            test.Run();
        }   
    }
}
