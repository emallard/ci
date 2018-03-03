using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

public class VBoxHelper {

    private readonly ShellHelper shellHelper;

    public VBoxHelper(
        ShellHelper shellHelper)
    {
        this.shellHelper = shellHelper;
    }

    public bool IsVmStarted(string vmName)
    {
        //var r = shellHelper.Bash($"vboxmanage showvminfo \"{vmName}\" | grep -c \"running (sinc\"");
        //return r.StartsWith("1");
        var list = shellHelper.Bash("vboxmanage list runningvms");
        return list.Contains("\"" + vmName +"\"");
    }

    public void CheckVmStarted(string vmName)
    {
        if (!IsVmStarted(vmName))
            throw new Exception($"VBox {vmName} not started, use : vboxmanage startvm \"{vmName}\" --type headless");
    }

    public void DeleteVm(string vmName)
    {
        shellHelper.Bash($"vboxmanage controlvm {vmName} poweroff");
        shellHelper.Bash($"vboxmanage unregistervm {vmName} --delete");
    }

    public void TryToStartVm(string vmName)
    {
        if (VmExists(vmName))
        {
            if (!IsVmStarted(vmName)) 
            {
                StartVm(vmName);
                Thread.Sleep(20000);
            }
        }
        
    }

    public void StartVm(string vmName)
    {
        shellHelper.Bash($"vboxmanage startvm \"{vmName}\" --type headless");
    }

    public void RestartVm(string vmName) 
    {
        shellHelper.Bash($"vboxmanage controlvm \"{vmName}\" acpipowerbutton");
        shellHelper.Bash($"vboxmanage startvm \"{vmName}\" --type headless");
    }

    public void CloneVm(string vmDir, string clonableVm, string newVm)
    {
        shellHelper.Bash($"vboxmanage import \"{vmDir}/{clonableVm}.ovf\" --vsys 0 --vmname \"{newVm}\" --unit 9 --disk \"{vmDir}/{newVm}.vmdk\"");
    }

    public bool VmExists(string vmName)
    {
        var list = shellHelper.Bash("vboxmanage list vms");
        return list.Contains("\"" + vmName +"\"");
    }

    public void CheckVmExists(string vmName)
    {
        if (!VmExists(vmName))
            throw new Exception("Vm " + vmName + " doesn't exists");  
    }

    public void CheckVmDoesNotExists(string vmName)
    {
        if (VmExists(vmName))
            throw new Exception("Vm " + vmName + " already exists");  
    }

    public void NatLocalSshPortForwarding(string ip, string port)
    {
        shellHelper.Bash($"vboxmanage natnetwork modify --netname natnet --port-forward-4 delete \"ssh{port}\"");
        shellHelper.Bash($"vboxmanage natnetwork modify --netname natnet --port-forward-4 \"ssh{port}:tcp:[127.0.0.1]:{port}:[{ip}]:22\"");
    }

    public void NatLocalRestart(string ip, string port)
    {
        shellHelper.Bash($"vboxmanage natnetwork stop --netname natnet");
        shellHelper.Bash($"vboxmanage natnetwork remove --netname natnet");
        shellHelper.Bash($"vboxmanage natnetwork add --netname natnet --network \"10.0.2.0/24\" --enable --dhcp off");
    }
}