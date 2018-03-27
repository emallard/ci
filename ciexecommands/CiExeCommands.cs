using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ciexecommands;
using citools;
using cilib;

namespace ciexecommands
{
    public class CiExeCommands {

        // Vm pilotes is bound to 2 volumes
        // 1) to be able to use the docker socket ans thus the docker engine api
        // 2) a directory to store data.
        
        SshConnection connection;
        Uri vaultUri;
        string vaultToken;

        string volume1;
        string volume2;

        public CiExeCommand InstallCA;
        public CiExeCommand CleanCA;
        public CiExeCommand InstallRegistry;
        public CiExeCommand CleanRegistry;
        public CiExeCommand InstallVault;
        public CiExeCommand BuildWebApp1;
        public CiExeCommand CleanWebApp1;
        public CiExeCommand PublishWebApp1;
        public CiExeCommand UnpublishWebApp1;
        
        // WebServer
        public CiExeCommand InstallTraefik;
        public CiExeCommand CleanTraefik;
        public CiExeCommand InstallWebApp1;
        public CiExeCommand CleanInstallWebApp1;

        public CiExeCommands(
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
            volume2 = "--volume " + "/cidata" + ":/cidata ";

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

        public CiExeCommands Configure(SshConnection connection, Uri vaultUri, string vaultToken)
        {
            this.connection = connection;
            this.vaultToken = vaultToken;
            this.vaultUri = vaultUri;
            return this;
        }


        public string SshCall(CiExeCommand command)
        {
            return new SshClient2()
                .SetConnection(this.connection)
                .SshScriptWithStdIn(
                    DockerRun(command.CommandLine), 
                    command.CommandLine + ".sh", new string[]{
                        "uri ", vaultUri.ToString(),
                        "token : ", vaultToken});
        }

        public string SshCommand(string command)
        {
            return new SshClient2()
                .SetConnection(this.connection)
                .SshCommand(command);
        }

        public Task ExecuteFromCommandLine(string commandLine)
        {
            var fields = this.GetType().GetFields();
            foreach (var field in fields)
            {
                if (field.FieldType == typeof(CiExeCommand))
                {
                    var cmd = (CiExeCommand) field.GetValue(this);
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
                if (field.FieldType == typeof(CiExeCommand))
                {
                    var cmd = (CiExeCommand) field.GetValue(this);
                    sb.AppendLine(cmd.CommandLine);
                }
            }
            return sb.ToString();
        }

        private string DockerRun(string arg)
        {
            return "docker run --name ciexe --rm " + volume1 + " " + volume2 + " ciexe " + arg;
        }

        private CiExeCommand<T> Create<T>(string commandLine, Func<Task> action)
        {
            var cliCmd = new CiExeCommand<T>(this, commandLine, action);
            return cliCmd;
        }
    }
}