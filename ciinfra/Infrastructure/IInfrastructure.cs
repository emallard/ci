using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using citools;

namespace ciinfra
{
    public interface IInfrastructure {


        void CreateVm(string key, string vmName, string rootPassword, string adminuser, string adminpassword);
        string GetVmIp(string key, string vmName);
        Uri GetVmSshUri(string key, string vmName);
        
        bool VmExists(string key, string vmName);

        void TryToStartVm(string key, string vmName);
        void DeleteVm(string key, string vmName);
    }

    public static class InfrastructureExtension
    {
        public static SshConnection GetVmSshConnection(this IInfrastructure infrastructure, string key, string vmName, string user, string password)
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