using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using ciinfra;

namespace cilib
{
    public class CiLibModule : Module {

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance<ICiLibCiDataDirectory>(new CiLibCiDataDirectory("/cidata"));
            builder.RegisterAssemblyTypes(this.GetType().Assembly);
            builder.RegisterModule<CiInfraModule>();
        }
    }

    public class CiLibModuleCiExeTest : Module {

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance<ICiLibCiDataDirectory>(new CiLibCiDataDirectory("/home/test/cidata"));
            builder.RegisterAssemblyTypes(this.GetType().Assembly);
            builder.RegisterModule<CiInfraModule>();
        }
    }
}