using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using ciexecommands;
using cilib;
using ciinfra;
using citools;
using cisteps;

namespace citest
{
    public class CommonModule : Module {

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<CiLibModule>();
            builder.RegisterModule<CiToolsModule>();
            builder.RegisterModule<CiStepsModule>();
            builder.RegisterModule<CiInfraModule>();
            builder.RegisterModule<CiExeCommandsModule>();

            builder.RegisterType<ListAsk>().SingleInstance();
            builder.RegisterType<ListResources>().SingleInstance();
            builder.RegisterType<InMemoryStore>().SingleInstance();
        }
        
    }
}