using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using citools;

namespace ciinfra
{
    public class GandiInfrastructure : IInfrastructure
    {
        private readonly GandiXmlRPC xmlRPC;

        public GandiInfrastructure(
            GandiXmlRPC xmlRPC)
        {
            this.xmlRPC = xmlRPC;
            //this.vmPilote.PrivateRegistryPort = 5443;
            //this.vmPilote.PrivateRegistryDomain = "privateregistry.mynetwork.local";
        }

        public void CreateVm(string key, string vmName, string rootPassword, string adminuser, string adminpassword)
        {
            xmlRPC.CreateVm(key,
                vmName,
                8000,
                rootPassword,
                adminuser,
                adminpassword
            );
            Thread.Sleep(30000);
            var vmInfo1 = xmlRPC.TryVmInfo(key, vmName);
            Thread.Sleep(30000);
            var vmInfo2 = xmlRPC.TryVmInfo(key, vmName);
            Thread.Sleep(1000);
        }

        public string GetVmIp(string key, string vmName)
        {
            var vmInfo = xmlRPC.TryVmInfo(key, vmName);
            if (vmInfo != null)
            {
                int ifaceId = vmInfo["ifaces_id"][0];
                var ifaceInfo = xmlRPC.IfaceInfo(key, ifaceId);
                var ipv4 = ifaceInfo.ips[0]["ip"];
                return ipv4;
            }
            throw new Exception("Vm not found");
        }

        public Uri GetVmSshUri(string key, string vmName)
        {
            var ip = GetVmIp(key, vmName);
            return new Uri("tcp://" + ip + ":22");
        }

        public void TryToStartVm(string key, string vmName)
        {
            var vmList = xmlRPC.VmList(key);
        }

        public void DeleteVm(string key, string vmName)
        {
            /*
            var vmId = xmlRPC.TryVmId(vmName);
            if (vmId > 0)
            {
                xmlRPC.VmStop(vmId);
                xmlRPC.VmDelete(vmId);
            }*/
        }

        public bool VmExists(string key, string vmName)
        {
            var vmInfo = xmlRPC.TryVmInfo(key, vmName);
            return vmInfo != null;
        }

    }
}