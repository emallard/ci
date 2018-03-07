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
                // To   : Image with CI installed
                Run<VmPilote_1_Create>();
                Run<VmPilote_1_Hosts>();
                Run<VmPilote_2_Docker>();
                Run<VmPilote_3_MirrorRegistry>();
                
                bool forceBuildCI = false;
                bool alternativeBuild = true;
                if (alternativeBuild)
                {
                    if (forceBuildCI)
                        Clean<VmPilote_5_PiloteCi_Build>();
                    Run<VmPilote_5b_PiloteCi_BuildUsingSdk>();
                }
                else
                {
                    if (forceBuildCI)
                    {
                        Clean<VmPilote_5_PiloteCi_Build>();
                        Clean<VmPilote_4_PiloteCi_Sources>();
                    }
                    Run<VmPilote_4_PiloteCi_Sources>();
                    Run<VmPilote_5_PiloteCi_Build>();
                }

                
                
                // From : Image with CI installed
                // To   : Vm with other software installed
                Run<PiloteCi_1_InstallCA>();
                //Run<PiloteCi_2_InstallVault>();
                Run<PiloteCi_3_InstallPrivateRegistry>();

                // From : Vm with CI Installed and other software installed
                // To   : Container with production webapp 
                ForceRun<PiloteCi_1_Build>();
                ForceRun<PiloteCi_2_Publish>();





                // Part 2 install app on webserver
                infrastructure.TryToStartVmWebServer();

                // From : no VM 
                // To   : Image with CI installed
                Run<VmWebServer_1_Create>();
                Run<VmWebServer_1_Hosts>();
                Run<VmWebServer_2_Docker>();
                Run<VmWebServer_3_MirrorRegistry>();

            };
        }

        private void Run<S>() where S : IStep
        {
            var s = container.Resolve<S>();
            Run(s);
        }
        

        private void Run(IStep step)
        {
            Console.WriteLine("==== " + step.GetType().Name);

            try {
                step.Test();
                Console.WriteLine("- Nothing to do");
            }
            catch (Exception)
            {
                Console.WriteLine("- Clean");
                step.Clean();
                Console.WriteLine("- Run");
                step.Run();
                Console.WriteLine("- Test");
                step.Test();
            }
        }
        
        private void Clean<S>() where S : IStep
        {
            var s = container.Resolve<S>();
            s.Clean();;
        }

        private void ForceRun<S>() where S : IStep
        {
            var s = container.Resolve<S>();
            ForceRun(s);
        }

        private void ForceRun(IStep step)
        {
            Console.WriteLine("==== ForceRun Step " + step.GetType().Name);

            Console.WriteLine("- Step clean");
            step.Clean();
            Console.WriteLine("- Step run");
            step.Run();
            Console.WriteLine("- Step test");
            step.Test();
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