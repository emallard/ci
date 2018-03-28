using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Renci.SshNet;
using System.IO;
using System.Text;

namespace citools
{
    public class SshClient2
    {
        SshConnection sshConnection;

        public SshClient2()
        {
        }

        public SshClient2 SetConnection(SshConnection sshConnection)
        {
            this.sshConnection = sshConnection;
            return this;
        }

        public SshClient Ssh()
        {
            var sshClient = new SshClient(GetConnectionInfo(sshConnection));
            sshClient.Connect();
            return sshClient;
        }

        public ScpClient Scp()
        {
            var scpClient = new ScpClient(GetConnectionInfo(sshConnection));
            scpClient.Connect();
            return scpClient;
        }

        protected ConnectionInfo GetConnectionInfo(SshConnection sshConnection)
        {
            var connectionInfo = new ConnectionInfo(
                sshConnection.SshUri.Host, 
                sshConnection.SshUri.Port, 
                sshConnection.User,
                new PasswordAuthenticationMethod(sshConnection.User, sshConnection.Password));
            return connectionInfo;
        }


        public string SshCommand(string command)
        {
            using (var client = Ssh())
            {
                var cmd = client.RunCommand(command);
                if (cmd.ExitStatus != 0)
                    throw new Exception($"Ssh error ({cmd.ExitStatus}) :" + cmd.Error);
                return cmd.Result;
            }
        }

        public string SshSudoBashCommand(string command)
        {
            using (var client = Ssh())
            {
                var result = new SshClientWrapper(client).RunSudoBash(command);
                return result;
            }
        }

        public string RunEmbeddedResourceWithSudo(IEmbeddedResource resource) 
        {
            
            using (var scpClient = Scp())
            {
                scpClient.Upload(resource.Stream(), resource.Name);
            }

            using (var client = Ssh())
            {
                return new SshClientWrapper(client).RunSudo("sh " + resource.Name);
            }
        }

        public string RunEmbeddedResource(IEmbeddedResource resource) 
        {
            
            using (var scpClient = Scp())
            {
                scpClient.Upload(resource.Stream(), resource.Name);
            }

            using (var client = Ssh())
            {
                var cmd = client.RunCommand("sh " + resource.Name);
                if (cmd.ExitStatus != 0)
                    throw new Exception($"Ssh error ({cmd.ExitStatus}) :" + cmd.Error);
                return cmd.Result;
            }
        }

        public string SshScript(string scriptContent, string scriptName) 
        {
            using (var scpClient = Scp())
            {
                var stream = new MemoryStream(Encoding.UTF8.GetBytes(scriptContent));
                scpClient.Upload(stream, scriptName);
            }

            using (var client = Ssh())
            {
                var cmd = client.RunCommand("sh " + scriptName);
                if (cmd.ExitStatus != 0)
                    throw new Exception($"Ssh error ({cmd.ExitStatus}) :" + cmd.Error);
                return cmd.Result;
            }
        }

        public string SshScriptWithStdIn(string scriptContent, string scriptName, string[] inputs) 
        {
            using (var scpClient = Scp())
            {
                var stream = new MemoryStream(Encoding.UTF8.GetBytes(scriptContent));
                scpClient.Upload(stream, scriptName);
            }

            using (var client = Ssh())
            {
                var wrapper = new SshClientWrapper(client);
                var result = wrapper.RunWithStdIn("sh " + scriptName, inputs);
                return result;
            }
        }
    }
}