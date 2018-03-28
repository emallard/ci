using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using citools;

namespace ciinfra
{
    public interface IInfrastructure {


        void CreateVm(InfrastructureKey key, string vmName, string rootPassword, string adminuser, string adminpassword);
        string GetVmIp(InfrastructureKey key, string vmName);
        Uri GetVmSshUri(InfrastructureKey key, string vmName);
        
        bool VmExists(InfrastructureKey key, string vmName);

        void TryToStartVm(InfrastructureKey key, string vmName);
        void DeleteVm(InfrastructureKey key, string vmName);
    }

    public static class InfrastructureExtension
    {
        public static SshConnection GetVmSshConnection(this IInfrastructure infrastructure, InfrastructureKey key, string vmName, string user, string password)
        {
            var sshUri = infrastructure.GetVmSshUri(key, vmName);

            return new SshConnection()
            {
                SshUri = sshUri,
                User = user,
                Password = password
            };
        }
    }
}