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
    public class RenciSshClient : ISshClient
    {
        SshConnection sshConnection;

        public string Command(string command)
        {
            using (var client = Ssh())
            {
                var cmd = client.RunCommand(command);
                if (cmd.ExitStatus != 0)
                    throw new Exception($"Ssh error ({cmd.ExitStatus}) :" + cmd.Error);
                return cmd.Result;
            }
        }

        public ISshClient Connect(SshConnection sshConnection)
        {
            this.sshConnection = sshConnection;
            return this;
        }

        public string Script(string scriptContent, string scriptName)
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

        public string ScriptWithStdIn(string scriptContent, string scriptName, string[] inputs)
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

        public string SudoBash(string command)
        {
            using (var client = Ssh())
            {
                var result = new SshClientWrapper(client).RunSudoBash(command);
                return result;
            }
        }

        public void SudoReboot()
        {
            using (var client = Ssh())
            {
                var wrapper = new SshClientWrapper(client);
                wrapper.SudoReboot();
            }
        }

        public string SudoScript(string scriptContent, string scriptName)
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

        // private 

        private SshClient Ssh()
        {
            var sshClient = new SshClient(GetConnectionInfo(sshConnection));
            sshClient.Connect();
            return sshClient;
        }

        private ScpClient Scp()
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
    }
}