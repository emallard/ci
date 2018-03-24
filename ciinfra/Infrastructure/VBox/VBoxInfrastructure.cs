using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading;
using Renci.SshNet;
using ciinfra;

public class VBoxInfrastructure : IInfrastructure
{
    private readonly VBoxHelper vBoxHelper;
    private readonly VmPilote vmPilote;
    private readonly VBoxVmWebServer vmWebServer;
    string vmDir = "/media/etienne/LinuxData/vm/";
    //string vmIso = "/media/etienne/LinuxData/ubuntu-16.04.3-server-amd64.iso";
    string clonableVm = "clonable";
    string clonableVmOvf => Path.Combine(vmDir, clonableVm + ".ovf");

    string WebServerVmName = "webserver";
    string WebServerIp = "10.0.2.6";
    int WebServerPortForward = 22006;


    public string DomainName => "mynetwork.local";
    public string CidataDirectory => "/home/test/cidata";

    public VBoxInfrastructure(
        VBoxHelper vBoxHelper,
        VmPilote vmPilote,
        VBoxVmWebServer vmWebServer)
    {
        this.vBoxHelper = vBoxHelper;
        this.vmPilote = vmPilote;
        this.vmWebServer = vmWebServer;

        this.vmPilote.PrivateRegistryPort = 5443;
        this.vmPilote.PrivateRegistryDomain = "privateregistry.mynetwork.local";       
    }


    public void TryToStartVm(InfrastructureKey key, string vmName)
    {
         vBoxHelper.TryToStartVm(vmName);
    }

    public void DeleteVm(InfrastructureKey key, string vmName)
    {
        vBoxHelper.DeleteVm(vmName);
    }

    public IVmPilote GetVmPilote(SshConnection sshConnection)
    {
        this.vmPilote.SetSshConnection(sshConnection);
        return this.vmPilote;
    }

    public IVmWebServer GetVmWebServer(SshConnection sshConnection)
    {
        this.vmWebServer.SetSshConnection(sshConnection);
        return this.vmWebServer;
    }

    // 
    //  Private
    //

    private void CheckVmDirExists()
    {
        if (!Directory.Exists(vmDir))
            throw new Exception($"VM directory doesn't exists : {vmDir}. Mount it or create it");
    }

    private void CheckClonableVm()
    {
        // check if clonable vm machine exists
        if (!File.Exists(clonableVmOvf))
            throw new Exception($"{clonableVmOvf} doesn't exists. Please run VBoxClonableVm.sh");
    }

    private void CheckVmExists(string vmName)
    {
        vBoxHelper.CheckVmExists(vmName);
    }

    private void CheckVmDoesNotExists(string vmName)
    {
        vBoxHelper.CheckVmDoesNotExists(vmName);
    }


    public string GetVmIp(InfrastructureKey key, string vmName)
    {
        if (vmName == "pilote")
            return "10.0.2.5";
        if (vmName == "webserver")
            return "10.0.2.6";

        throw new Exception("Unknown VM");
    }

    public Uri GetVmSshUri(InfrastructureKey key, string vmName)
    {
        if (vmName == "pilote")
            return new Uri("tcp://10.0.2.5:22005");
        if (vmName == "webserver")
            return new Uri("tcp://10.0.2.6:22006");

        throw new Exception("Unknown VM");
    }

    public void CreateVm(InfrastructureKey key, string vmName, string rootPassword, string adminuser, string adminpassword)
    {
        var ip = GetVmIp(key, vmName);
        var portForward = GetVmSshUri(key, vmName).Port;

        CheckClonableVm();
        CheckVmDoesNotExists(vmName);

        // Clone clonableVm
        vBoxHelper.CloneVm(vmDir, clonableVm, vmName);

        // Start vm, it has been cloned from an image that runs on IP 10.0.2.200
        vBoxHelper.StartVm(vmName);
        Thread.Sleep(20000);
        
        vBoxHelper.NatLocalSshPortForwarding("10.0.2.200", $"{portForward}");
        var connectionInfo = new ConnectionInfo(
            "127.0.0.1", 22200, "test",
            new PasswordAuthenticationMethod("test", "test"));
        using (var sshClient = new SshClient(connectionInfo))
        {
            sshClient.Connect();
            var command = $"sed 's/10.0.2.200/{ip}/g' /etc/network/interfaces > sed.tmp && cat sed.tmp > /etc/network/interfaces";
            Console.WriteLine(command);
            var cmd = sshClient.RunCommand(command);
            Console.WriteLine("result : " + cmd.Result);
        }

        Console.WriteLine("restart vm");
        vBoxHelper.RestartVm(vmName);
        Thread.Sleep(20000);
        vBoxHelper.NatLocalSshPortForwarding(ip, $"{portForward}");
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