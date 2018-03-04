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
            
            if (args[0] == "install-registry")
                new Lanceur().RunSync<InitRegistry>(r => r.Init());
            
            else if (args[0] == "install-vault")
                new Lanceur().RunSync<InitVault>(r => r.Init());

            else if (args[0] == "build")
                new Lanceur().RunSync<PiloteExample1>(r => r.Build());
            
        }
    }
}
