using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cicli
{
    public class CiCli {

        // Vm pilotes is bound to 2 volumes
        // 1) to be able to use the docker socket ans thus the docker engine api
        // 2) a directory to store data.
        
        IVm vm;

        string volume1;
        string volume2;

        public CiCliCommand InstallCA;
        public CiCliCommand CleanCA;
        public CiCliCommand InstallRegistry;
        public CiCliCommand CleanRegistry;
        public CiCliCommand InstallVault;
        public CiCliCommand BuildWebApp1;
        public CiCliCommand CleanWebApp1;
        public CiCliCommand PublishWebApp1;
        public CiCliCommand UnpublishWebApp1;
        
        // WebServer
        public CiCliCommand InstallTraefik;
        public CiCliCommand CleanTraefik;
        public CiCliCommand InstallWebApp1;
        public CiCliCommand CleanInstallWebApp1;


        public CiCli(

            IInfrastructure infrastructure,

            InstallCA installCA,
            InstallRegistry installRegistry,
            InstallVault installVault,
            BuildWebApp1 buildWebApp1,

            // WebServer
            InstallTraefik installTraefik,
            InstallWebApp installWebApp
            )
        {

            volume1 = "--volume /var/run/docker.sock:/var/run/docker.sock ";
            volume2 = "--volume " + infrastructure.CidataDirectory + ":/cidata ";

            this.InstallCA = Create<InstallCA>("install-ca", async () => await installCA.Install());
            this.CleanCA = Create<InstallCA>("clean-ca", async () => await installCA.Clean());

            this.InstallRegistry = Create<InstallRegistry>("install-registry", async () => await installRegistry.Install());
            this.CleanRegistry = Create<InstallRegistry>("clean-registry", async () => await installRegistry.Clean());

            this.InstallVault = Create<InstallVault>("install-vault", async () => await installVault.Init());

            this.BuildWebApp1 = Create<BuildWebApp1>("build-webapp1", async () => await buildWebApp1.Build());
            this.CleanWebApp1 = Create<BuildWebApp1>("clean-webapp1", async () => await buildWebApp1.CleanBuild());

            this.PublishWebApp1 = Create<BuildWebApp1>("publish-webapp1", async () => await buildWebApp1.Publish());
            this.UnpublishWebApp1 = Create<BuildWebApp1>("cleanpublish-webapp1", async () => await buildWebApp1.CleanPublish());

            // Webserver
            this.InstallTraefik = Create<InstallTraefik>("webserver-install-webapp1", async () => await installTraefik.Install());
            this.CleanTraefik = Create<InstallTraefik>("clean-webapp1", async () => await installTraefik.Clean());

            this.InstallWebApp1 = Create<InstallWebApp>("webserver-install-webapp1", async () => await installWebApp.Install());
            this.CleanInstallWebApp1 = Create<InstallWebApp>("clean-webapp1", async () => await installWebApp.CleanInstall());
        }

        public CiCli SetVm(IVm vm)
        {
            this.vm = vm;
            return this;
        }

        public string SshDockerRun(string arg)
        {
            return vm.SshCommand(DockerRun(arg));
        }

        public void SshCall(CiCliCommand command)
        {
            vm.SshScript(DockerRun(command.CommandLine), command.CommandLine + ".sh");
        }

        public Task ExecuteFromCommandLine(string commandLine)
        {
            var fields = this.GetType().GetFields();
            foreach (var field in fields)
            {
                if (field.FieldType == typeof(CiCliCommand))
                {
                    var cmd = (CiCliCommand) field.GetValue(this);
                    if (cmd.CommandLine == commandLine)    
                    {
                        return cmd.Action();
                    }
                }
            }
            throw new Exception("No command found for command line : " + commandLine);
        }

        public string CommandList()
        {
            var sb = new StringBuilder();
            var fields = this.GetType().GetFields();
            foreach (var field in fields)
            {
                if (field.FieldType == typeof(CiCliCommand))
                {
                    var cmd = (CiCliCommand) field.GetValue(this);
                    sb.AppendLine(cmd.CommandLine);
                }
            }
            return sb.ToString();
        }

        private string DockerRun(string arg)
        {
            return "docker run --name ciexe --rm " + volume1 + " " + volume2 + " ciexe " + arg;
        }

        private CiCliCommand<T> Create<T>(string commandLine, Func<Task> action)
        {
            var cliCmd = new CiCliCommand<T>(this, commandLine, action);
            return cliCmd;
        }
    }
}