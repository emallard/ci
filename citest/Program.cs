using System;
using Autofac;

namespace citest
{
    class Program
    {
        static void Main(string[] args)
        {
            var runner = new TestRunner();

            var test = new Scenario1<VBoxInfrastructure>(runner);
            test.RunAll();
        }
    }
}
