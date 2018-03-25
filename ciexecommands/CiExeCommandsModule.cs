using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using ciexecommands;
using cilib;

namespace ciexecommands
{
    public class CiExeCommandsModule : Module {

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(this.GetType().Assembly);
            builder.RegisterModule<CiLibModule>();
        }
        
    }

    public class CiExeCommandsModuleCiExeTest : Module {

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(this.GetType().Assembly);
            builder.RegisterModule<CiLibModuleCiExeTest>();
        }
        
    }
}