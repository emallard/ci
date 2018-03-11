using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Autofac;
using cicli;

namespace citest
{
    public class Scenario1<T> where T : IInfrastructure {
        private readonly TestRunner runner;
        IContainer container;
        

        public Scenario1(TestRunner runner) 
        {
            this.runner = runner;
            this.container = Init();
            runner.SetContainer(container);

            runner.TestOnly = false;
        }

        public void RunAll()
        {
            
            var infrastructure = container.Resolve<IInfrastructure>();
            infrastructure.TryToStartVmPilote();

            // From : no VM 
            // To   : Image with CI installed
            runner.Run<VmPilote_1_Create>();
            runner.Run<VmPilote_1_Hosts>();
            runner.Run<VmPilote_2_Docker>();
            runner.Run<VmPilote_3_MirrorRegistry>();
            
            bool forceBuildCI = true;
            bool alternativeBuild = true;
            if (alternativeBuild)
            {
                if (forceBuildCI)
                    runner.Clean<VmPilote_5_PiloteCi_Build>();
                runner.Run<VmPilote_5b_PiloteCi_BuildUsingSdk>();
            }
            else
            {
                if (forceBuildCI)
                {
                    runner.Clean<VmPilote_5_PiloteCi_Build>();
                    runner.Clean<VmPilote_4_PiloteCi_Sources>();
                }
                runner.Run<VmPilote_4_PiloteCi_Sources>();
                runner.Run<VmPilote_5_PiloteCi_Build>();
            }

            
            
            // From : Image with CI installed
            // To   : Vm with other software installed
            runner.Run<PiloteCi_1_InstallCA>();
            //Run<PiloteCi_2_InstallVault>();
            runner.Run<PiloteCi_3_InstallPrivateRegistry>();

            // From : Vm with CI Installed and other software installed
            // To   : Container with production webapp 
            runner.Run<PiloteCi_1_Build>();
            runner.ForceRun<PiloteCi_2_Publish>();



            // Part 2 install app on webserver
            infrastructure.TryToStartVmWebServer();

            // From : no VM 
            // To   : Image with CI installed
            runner.Run<VmWebServer_1_Create>();
            runner.Run<VmWebServer_1_Hosts>();
            runner.Run<VmWebServer_2_Docker>();
            runner.Run<VmWebServer_3_MirrorRegistry>();
        }

        private IContainer Init()
        {
            var builder = new ContainerBuilder();
            
            builder.RegisterType<T>().As<IInfrastructure>();

            builder.RegisterType<ConfigVBox>().As<IConfig>();
            builder.RegisterAssemblyTypes(typeof(CiLib).Assembly);
            builder.RegisterAssemblyTypes(typeof(CiCli).Assembly);
            builder.RegisterAssemblyTypes(this.GetType().Assembly);

            var container = builder.Build();
            return container;
        }
    }
}