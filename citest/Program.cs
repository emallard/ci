using System;
using Autofac;

namespace citest
{
    class Program
    {
        static void Main(string[] args)
        {
            var test = new Test<VBoxInfrastructure>();
            test.RunAll();
        }
    }
}
