using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading;
using Renci.SshNet;

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
        
        this.vmPilote.VmName = "pilote";
        this.vmPilote.Ip = new IPAddress(new byte[]{10,0,2,5});
        this.vmPilote.PortForward = 22005;
        this.vmPilote.PrivateRegistryPort = 5443;
        this.vmPilote.PrivateRegistryDomain = "privateregistry.mynetwork.local";
        this.vmPilote.SshUri = new Uri($"tcp://127.0.0.1:{vmPilote.PortForward}");
        this.vmPilote.SshUser = "test";
        this.vmPilote.SshPassword = "test";
        
        this.vmWebServer.Configure(new Uri($"tcp://127.0.0.1:{WebServerPortForward}")/*, WebServerIp*/);
        this.vmWebServer.SetVmPilote(this.vmPilote); 
        
    }


    //
    //  Pilote
    //

    public void TryToStartVmPilote()
    {
        CheckVmDirExists();
        vBoxHelper.TryToStartVm(vmPilote.VmName);
    }

    public void DeleteVmPilote()
    {
        vBoxHelper.DeleteVm(vmPilote.VmName);
    }

    public void CreateVmPilote()
    {
        this.CreateVm(vmPilote.VmName, vmPilote.Ip.ToString(), vmPilote.PortForward);
    }

    public IVmPilote GetVmPilote()
    {
        return this.vmPilote;
    }

    //
    //  WebServer
    //

    public void TryToStartVmWebServer()
    {
        CheckVmDirExists();
        vBoxHelper.TryToStartVm(WebServerVmName);
    }

    public void DeleteVmWebServer()
    {
        vBoxHelper.DeleteVm(WebServerVmName);
    }

    public void CreateVmWebServer()
    {
        this.CreateVm(WebServerVmName, WebServerIp, WebServerPortForward);
    }

    public IVmWebServer GetVmWebServer()
    {
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

    private void CreateVm(string vmName, string ip, int portForward)
    {
        
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


}