using System;
using System.Threading.Tasks;
using Autofac;

namespace ciexe
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0) {
                Console.WriteLine("--help");
                return;
            }
                

            /*
            var builder = new ContainerBuilder();
            var injection = new Injection();
            injection.ConfigureProd(builder);
            var container = builder.Build();
            */
            if (args[0] == "hello")
            {
                Console.WriteLine("hello");
                return;
            }
            if (args[0] == "InitPilote")
            {
                Task.WaitAll(new Lanceur().InitPilote());
            }
        }
    }
}
