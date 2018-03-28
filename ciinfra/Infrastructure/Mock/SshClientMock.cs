using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using citools;

namespace ciinfra
{
    public class SshClientMock : ISshClient
    {
        private readonly InfrastructureMock infrastructure;
        SshConnection sshConnection;

        public SshClientMock(InfrastructureMock infrastructure)
        {
            this.infrastructure = infrastructure;
        }

        public string Command(string command)
        {
            this.infrastructure.GetVmMockBySshUri(sshConnection.SshUri).Command(command);
            return "";
        }

        public ISshClient Connect(SshConnection sshConnection)
        {
            this.sshConnection = sshConnection;
            return this;
        }

        public string Script(string scriptContent, string scriptName)
        {
            this.infrastructure.GetVmMockBySshUri(sshConnection.SshUri).Command(scriptContent);
            return "";
        }

        public string ScriptWithStdIn(string scriptContent, string scriptName, string[] inputs)
        {
            this.infrastructure.GetVmMockBySshUri(sshConnection.SshUri).Command(
                "script with inputs :" + string.Join(",", inputs) + "\n" + scriptContent);
            return "";
        }

        public string SudoBash(string command)
        {
            this.infrastructure.GetVmMockBySshUri(sshConnection.SshUri).Command("sudo bash : " + command);
            return "";
        }

        public void SudoReboot()
        {

        }

        public string SudoScript(string scriptContent, string scriptName)
        {
            this.infrastructure.GetVmMockBySshUri(sshConnection.SshUri).Command("sudo script\n " + scriptContent);
            return "";
        }
    }
}