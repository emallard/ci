using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace ciinfra
{
    public class InfrastructureMock : IInfrastructure
    {

        public List<VmMock> Vms = new List<VmMock>();

        public void CreateVm(InfrastructureKey key, string vmName, string rootPassword, string adminuser, string adminpassword)
        {
            Vms.Add(new VmMock() {
                Name = vmName,
                SshUri = new Uri("ssh://" + vmName + "sshuri")});
        }

        public void DeleteVm(InfrastructureKey key, string vmName)
        {
            var found = Vms.FirstOrDefault(vm => vm.Name == vmName);
            Vms.Remove(found);
        }

        public string GetVmIp(InfrastructureKey key, string vmName)
        {
            return "192.168." + vmName;
        }

        public Uri GetVmSshUri(InfrastructureKey key, string vmName)
        {
            return GetVmMockByName(vmName).SshUri;
        }

        public void TryToStartVm(InfrastructureKey key, string vmName)
        {

        }

        public bool VmExists(InfrastructureKey key, string vmName)
        {
            return null != GetVmMockByName(vmName);
        }

        public VmMock GetVmMockByName(string name)
        {
            return Vms.FirstOrDefault(vm => vm.Name == name);
        }

        public VmMock GetVmMockBySshUri(Uri uri)
        {
            return Vms.FirstOrDefault(vm => vm.SshUri.OriginalString == uri.OriginalString);
        }
    }
}