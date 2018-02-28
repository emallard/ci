using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class VBoxInfrastructure : IInfrastructure
{

    VBoxVmPilote vmPilote;

    string vmDir = "/media/etienne/LinuxData/vm/";
    //string vmIso = "/media/etienne/LinuxData/ubuntu-16.04.3-server-amd64.iso";
    string clonableVm = "clonable";
    string clonableVmOvf => Path.Combine(vmDir, clonableVm + ".ovf");

    public VBoxInfrastructure(VBoxVmPilote vmPilote)
    {
        this.vmPilote = vmPilote;
        this.vmPilote.Configure(new Uri("tcp://127.0.0.1:22005"), "10.0.2.5");
    }

    public void DeleteVmPilote()
    {
        "VBoxManage controlvm pilote poweroff".Bash();
        "VBoxManage unregistervm pilote --delete".Bash();
    }

    public void CreateVmPilote()
    {
        CheckClonableVm();
        CheckVmDoesNotExists();
        var bashArgs = 
          $"export VMDIR={vmDir} \n"
        + $"export CLONABLEVM={clonableVm} \n"
        + $"export NEWVM=pilote \n"
        + $"export IP=10.0.2.5 \n"
        + $"export PORTFORWARD=22005 \n";

        var output = ShellHelper.Bash(bashArgs + EmbeddedResources.VBoxClone.ReadAsText());
        Console.WriteLine(output);
    }

    public IVmPilote GetVmPilote()
    {
        CheckVmExists();
        return this.vmPilote;
    }

    private void CheckClonableVm()
    {
        // check if clonable vm machine exists
        if (!File.Exists(clonableVmOvf))
            throw new Exception($"{clonableVmOvf} doesn't exists. Please run VBoxClonableVm.sh");
    }

    private void CheckVmExists()
    {
        var list = "VBoxManage list vms".Bash();
        if (!list.Contains("\"pilote\""))
            throw new Exception("Vm pilote doesn't exists");  
    }

    private void CheckVmDoesNotExists()
    {
        var list = "VBoxManage list vms".Bash();
        if (list.Contains("\"pilote\""))
            throw new Exception("Vm pilote already exists");  
    }

}