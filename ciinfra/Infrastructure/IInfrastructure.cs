using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using ciinfra;
using Renci.SshNet;

public interface IInfrastructure {


    void CreateVm(InfrastructureKey key, string vmName, string rootPassword, string adminuser, string adminpassword);
    string GetVmIp(InfrastructureKey key, string vmName);
    Uri GetVmSshUri(InfrastructureKey key, string vmName);
    SshClient Ssh(InfrastructureKey key, string vmName, string user, string password);

    void TryToStartVm(InfrastructureKey key, string vmName);
    void DeleteVm(InfrastructureKey key, string vmName);

    IVmPilote GetVmPilote(SshConnection sshConnection);
    IVmWebServer GetVmWebServer(SshConnection sshConnection);

}