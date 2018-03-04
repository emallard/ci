using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Autofac;

namespace citest
{
    public class Test<T> where T : IInfrastructure {

        public Action RunAll;
        IContainer container;

        /*
        public Test(IInfrastructure infrastructure,
            CreateVmPilote createVmPilote,
            CreateVmWebServer createVmWebServer,
            PiloteCi piloteCi,
            PiloteDocker piloteDocker
            PiloteMirrorRegistry piloteCi)
        {*/

        public Test() 
        {
            RunAll = () =>
            {
                container = Init();
                var infrastructure = container.Resolve<IInfrastructure>();
                infrastructure.TryToStartVmPilote();

                // From : no VM 
                // To   : Container with CI installed
                Run<VmPilote_1_Create>();
                Run<VmPilote_2_Docker>();
                Run<VmPilote_3_MirrorRegistry>();
                Run<VmPilote_4_Ci>();

                // From : Container with CI installed
                // To   : Vm with other software installed
                //Run<PiloteCi_1_InstallCA>();
                //Run<PiloteCi_2_InstallVault>();
                //Run<PiloteCi_3_InstallLocalRegistry>();


                // From : Vm with CI Installed and other software installed
                // To   : Container with production webapp 
                Run<PiloteCi_1_CreateBuildContainer>();
                Run<PiloteCi_2_SetSourceInBuildContainer>();
                Run<PiloteCi_3_RunBuildContainer>();
                Run<PiloteCi_4_CreateAppContainer>();
                Run<PiloteCi_5_PublishToAppRegistry>();
                Run<PiloteCi_6_RunFromAppRegistry>();
            };
        }

        private void Run<S>() where S : IStep
        {
            Run(container.Resolve<S>());
        }
        

        private void Run(IStep step)
        {
            Console.WriteLine("==== Start Step " + step.GetType().Name);

            try {
                Console.WriteLine("- Step test");
                step.Test();
                Console.WriteLine("- OK");
            }
            catch (AssertException)
            {
                Console.WriteLine("- Step revert");
                step.Revert();
                Console.WriteLine("- Step run");
                step.Run();
                Console.WriteLine("- Step test");
                step.Test();
            }
        }

        private IContainer Init()
        {
            var builder = new ContainerBuilder();
            
            builder.RegisterType<T>().As<IInfrastructure>();

            builder.RegisterType<ConfigVBox>().As<IConfig>();
            builder.RegisterAssemblyTypes(typeof(Lanceur).Assembly);
            builder.RegisterAssemblyTypes(this.GetType().Assembly);

            var container = builder.Build();
            return container;
        }
    }
}