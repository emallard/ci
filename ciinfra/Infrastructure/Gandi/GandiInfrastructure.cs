using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ciinfra;
using Renci.SshNet;

namespace ciinfra
{
    public class GandiInfrastructure : IInfrastructure
    {
        private readonly GandiXmlRPC xmlRPC;
        private readonly VmPilote vmPilote;

        public GandiInfrastructure(
            GandiXmlRPC xmlRPC,
            VmPilote vmPilote)
        {
            this.xmlRPC = xmlRPC;
            this.vmPilote = vmPilote;
            this.vmPilote.PrivateRegistryPort = 5443;
            this.vmPilote.PrivateRegistryDomain = "privateregistry.mynetwork.local";
        }

        public IVmPilote GetVmPilote()
        {
            return this.vmPilote;
        }

        public void CreateVm(InfrastructureKey key, string vmName, string rootPassword, string adminuser, string adminpassword)
        {
            xmlRPC.CreateVm(key.Content,
                vmName,
                8000,
                rootPassword,
                adminuser,
                adminpassword
            );
            Thread.Sleep(30000);
            var vmInfo1 = xmlRPC.TryVmInfo(key.Content, vmName);
            Thread.Sleep(30000);
            var vmInfo2 = xmlRPC.TryVmInfo(key.Content, vmName);
            Thread.Sleep(1000);
        }

        public string GetVmIp(InfrastructureKey key, string vmName)
        {
            var vmInfo = xmlRPC.TryVmInfo(key.Content, vmName);
            if (vmInfo != null)
            {
                int ifaceId = vmInfo["ifaces_id"][0];
                var ifaceInfo = xmlRPC.IfaceInfo(key.Content, ifaceId);
                var ipv4 = ifaceInfo.ips[0]["ip"];
                return ipv4;
            }
            throw new Exception("Vm not found");
        }

        public Uri GetVmSshUri(InfrastructureKey key, string vmName)
        {
            var ip = GetVmIp(key, vmName);
            return new Uri("tcp://" + ip + ":22");
        }

        public void TryToStartVm(InfrastructureKey key, string vmName)
        {
            var vmList = xmlRPC.VmList(key.Content);
        }

        public void DeleteVm(InfrastructureKey key, string vmName)
        {
            /*
            var vmId = xmlRPC.TryVmId(vmName);
            if (vmId > 0)
            {
                xmlRPC.VmStop(vmId);
                xmlRPC.VmDelete(vmId);
            }*/
        }

        public IVmPilote GetVmPilote(SshConnection sshConnection)
        {
            throw new NotImplementedException();
        }

        public IVmWebServer GetVmWebServer(SshConnection sshConnection)
        {
            throw new NotImplementedException();
        }

        public SshClient Ssh(InfrastructureKey key, string vmName, string user, string password)
        {
            var sshUri = this.GetVmSshUri(key, vmName);
            var connectionInfo = new ConnectionInfo(
                sshUri.Host, 
                sshUri.Port, 
                user,
                new PasswordAuthenticationMethod(user, password));

            var sshClient = new SshClient(connectionInfo);
            sshClient.Connect();
            return sshClient;
        }
    }
}