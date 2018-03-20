using System;
using System.Threading.Tasks;
using Autofac;

namespace citest
{
    class Program
    {
        static void Main(string[] args)
        {

            //var t = new GandiXmlRPC();
            //t.SystemListMethods();
            //t.VersionInfo();
            //t.AccountInfo();
            //t.VmCount();
            //t.CreateVm("pilote");

            //Task.WaitAll(t.Test());

            
            var runner = new TestRunner();
            //var test = new Scenario1<VBoxInfrastructure>(runner);
            var test = new Scenario1<GandiInfrastructure>(runner);
            test.RunAll();
            
        }   
    }
}
