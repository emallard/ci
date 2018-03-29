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

        public SshClientMock(IInfrastructure infrastructure)
        {
            if (!(infrastructure is InfrastructureMock))
                throw new Exception("SshClientMock must be used with InfrastructureMock");
            this.infrastructure = (InfrastructureMock) infrastructure;
        }

        public string Command(string command)
        {
            GetVmMock().Command(command);
            return "";
        }

        public ISshClient Connect(SshConnection sshConnection)
        {
            this.sshConnection = sshConnection;
            return this;
        }

        public string Script(string scriptContent, string scriptName)
        {
            GetVmMock().Command(scriptContent);
            return "";
        }

        public string ScriptWithStdIn(string scriptContent, string scriptName, string[] inputs)
        {
            GetVmMock().Command(
                "script with inputs :" + string.Join(",", inputs) + "\n" + scriptContent);
            return "";
        }

        public string SudoBash(string command)
        {
            GetVmMock().Command("sudo bash : " + command);
            return "";
        }

        public void SudoReboot()
        {
            GetVmMock().Reboot();
        }

        public string SudoScript(string scriptContent, string scriptName)
        {
            GetVmMock().Command("sudo script\n " + scriptContent);
            return "";
        }

        private VmMock GetVmMock()
        {
            if (sshConnection == null)
                throw new Exception("Don't forget to use SshClient.Connect to set SshConnectionInfo");
            return this.infrastructure.GetVmMockBySshConnection(sshConnection);
        }
    }
}