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
    VBoxVmPilote vmPilote;

    string vmDir = "/media/etienne/LinuxData/vm/";
    //string vmIso = "/media/etienne/LinuxData/ubuntu-16.04.3-server-amd64.iso";
    string clonableVm = "clonable";
    string clonableVmOvf => Path.Combine(vmDir, clonableVm + ".ovf");

    string PiloteVmName = "pilote";
    string PiloteIp = "10.0.2.5";
    int PilotePortForward = 22005;

    public VBoxInfrastructure(
        VBoxHelper vBoxHelper,
        VBoxVmPilote vmPilote)
    {
        this.vBoxHelper = vBoxHelper;
        this.vmPilote = vmPilote;
        this.vmPilote.Configure(new Uri($"tcp://127.0.0.1:{PilotePortForward}"), PiloteIp);

        // Check that directory with vms exists 
        CheckVmDirExists();
    }

    public void DeleteVmPilote()
    {
        vBoxHelper.DeleteVm(PiloteVmName);
    }

    public void CreateVmPilote()
    {
        
        CheckClonableVm();
        CheckVmDoesNotExists();

        // Clone clonableVm
        vBoxHelper.CloneVm(vmDir, clonableVm, PiloteVmName);

        // Start pilote, it has been cloned from an image that runs on IP 10.0.2.200
        vBoxHelper.StartVm(PiloteVmName);
        Thread.Sleep(20000);
        
        vBoxHelper.NatLocalSshPortForwarding("10.0.2.200", $"{PilotePortForward}");
        var connectionInfo = new ConnectionInfo(
            "127.0.0.1", 22200, "test",
            new PasswordAuthenticationMethod("test", "test"));
        using (var sshClient = new SshClient(connectionInfo))
        {
            sshClient.Connect();
            var command = $"sed 's/10.0.2.200/{PiloteIp}/g' /etc/network/interfaces > sed.tmp && cat sed.tmp > /etc/network/interfaces";
            Console.WriteLine(command);
            var cmd = sshClient.RunCommand(command);
            Console.WriteLine("result : " + cmd.Result);
        }

        Console.WriteLine("restart vm");
        vBoxHelper.RestartVm(PiloteVmName);
        Thread.Sleep(20000);
        vBoxHelper.NatLocalSshPortForwarding(PiloteIp, $"{PilotePortForward}");
    }

    public IVmPilote GetVmPilote()
    {
        CheckVmExists();
        return this.vmPilote;
    }

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

    private void CheckVmExists()
    {
        vBoxHelper.CheckVmExists(PiloteVmName);
    }

    private void CheckVmDoesNotExists()
    {
        vBoxHelper.CheckVmDoesNotExists(PiloteVmName);
    }

}