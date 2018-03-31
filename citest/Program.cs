using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using ciexecommands;
using ciinfra;
using cilib;
using cisteps;
using citools;

namespace citest
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                new ApiMain().Main(args);
                return;
            }
            else
            {    
                new CommandLineMain().Main(args);
                return;
            }
        }

    }
}
