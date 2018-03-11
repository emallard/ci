using System;
using Autofac;
using cicli;
using Xunit;

namespace ciexetest
{
    public class CiExeTests
    {
        [Fact] public void InstallCA()              { Run(cli => cli.InstallCA); }
        [Fact] public void CleanCA()                { Run(cli => cli.CleanCA); }
        [Fact] public void InstallRegistry()        { Run(cli => cli.InstallRegistry); }
        [Fact] public void CleanRegistry()          { Run(cli => cli.CleanRegistry); }
        [Fact] public void InstallVault()           { Run(cli => cli.InstallVault); }
        [Fact] public void BuildWebApp1()           { Run(cli => cli.BuildWebApp1); }
        [Fact] public void CleanWebApp1()           { Run(cli => cli.CleanWebApp1); }
        [Fact] public void PublishWebApp1()         { Run(cli => cli.PublishWebApp1); }
        [Fact] public void UnpublishWebApp1()       { Run(cli => cli.UnpublishWebApp1); }
        [Fact] public void InstallTraefik()         { Run(cli => cli.InstallTraefik); }
        [Fact] public void CleanTraefik()           { Run(cli => cli.CleanTraefik); }
        [Fact] public void InstallWebApp1()         { Run(cli => cli.InstallWebApp1); }
        [Fact] public void CleanInstallWebApp1()    { Run(cli => cli.CleanInstallWebApp1); }


        private void Run(Func<CiCli, CiCliCommand> commande)
        {
            RunSync.Run<CiCli>(getDI(), cli =>  {
                    var cmd = commande(cli);
                    // put break point here
                    return cmd.Action();
                }
            );
        }

        public IContainer getDI()
        {
            var builder = new ContainerBuilder();
            
            builder.RegisterType<VBoxInfrastructure>().As<IInfrastructure>();           
            builder.RegisterModule<CiCliModuleCiExeTest>();

            var container = builder.Build();
            return container;
            
        }
    }
}
