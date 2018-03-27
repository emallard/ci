using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Autofac.Core;
using Autofac;

namespace citest
{
    public class ProdModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterAssemblyTypes(this.GetType().Assembly);
            //builder.RegisterModule<CiLibModule>();
        }
    }
}