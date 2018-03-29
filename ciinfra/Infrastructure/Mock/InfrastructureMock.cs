using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using citools;

namespace ciinfra
{
    public class InfrastructureMock : IInfrastructure
    {

        public List<VmMock> Vms = new List<VmMock>();
        private readonly Func<VmMock> createVmMock;

        public InfrastructureMock(Func<VmMock> createVmMock)
        {
            this.createVmMock = createVmMock;
        }

        public void CreateVm(InfrastructureKey key, string vmName, string rootPassword, string adminuser, string adminpassword)
        {
            var mock = createVmMock();
            mock.Name = vmName;
            mock.SshUri = new Uri("ssh://" + vmName + "sshuri");
            mock.RootPassword = rootPassword;
            mock.AdminUser = adminuser;
            mock.AdminPassword = adminpassword;
            Vms.Add(mock);
        }

        public void DeleteVm(InfrastructureKey key, string vmName)
        {
            var found = GetVmMockByName(vmName);
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
            var found = Vms.FirstOrDefault(vm => vm.Name == name);
            if (found == null)
                throw new Exception("VmMock not found by Name");
            return found;
        }

        public VmMock GetVmMockBySshConnection(SshConnection connection)
        {
            var found = Vms.FirstOrDefault(vm => vm.SshUri.ToString() == connection.SshUri.ToString());
            if (found == null)
                throw new Exception("VmMock not found by Uri");

            if (connection.User != found.AdminUser || connection.Password != found.AdminPassword)
                throw new Exception("Ssh Bad username / password");

            return found;
        }
    }
}