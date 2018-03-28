using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Autofac;

namespace cisystem
{
    public class CiSystemModule : Module {

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(this.GetType().Assembly);
        }
    }
}