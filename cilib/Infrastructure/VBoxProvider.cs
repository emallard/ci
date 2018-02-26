using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

public class VBoxProvider : IPaasProvider {

    //https://nakkaya.com/2012/08/30/create-manage-virtualBox-vms-from-the-command-line/
    //https://www.howtoforge.com/tutorial/running-virtual-machines-with-virtualbox-5.1-on-a-headless-ubuntu-16.04-lts-server/

    // Download de l'extension pack : wget https://download.virtualbox.org/virtualbox/5.2.6/Oracle_VM_VirtualBox_Extension_Pack-5.2.6-120293.vbox-extpack

    public void CreateVm(string name, string login, string password)
    {
        var name2 = "\"" + name + "\"";
        ShellHelper.Bash($"VBoxManage createvm --name {name2} --register");
        ShellHelper.Bash($"VBoxManage modifyvm {name2} --memory 512 --acpi on --boot1 dvd");
        ShellHelper.Bash($"VBoxManage modifyvm {name2} --nic1 bridged --bridgeadapter1 eth0");
        ShellHelper.Bash($"VBoxManage modifyvm {name2} --macaddress1 XXXXXXXXXXXX");
        ShellHelper.Bash($"VBoxManage modifyvm {name2} --ostype Debian");
    }

    public void AddStorage(string name)
    {
        var iso = "~/ubuntu-16.04.3-server-amd64.iso";
        var name2 = "\"" + name + "\"";
        ShellHelper.Bash($"VBoxManage createhd --filename ./{name}.vdi --size 10000");
        ShellHelper.Bash($"VBoxManage storagectl {name2} --name \"IDE Controller\" --add ide");
        ShellHelper.Bash($"VBoxManage storageattach {name2} --storagectl \"IDE Controller\" --port 0 --device 0 --type hdd --medium ./${name}.vdi");
        ShellHelper.Bash($"VBoxManage storageattach {name2} --storagectl \"IDE Controller\" --port 1 --device 0 --type dvddrive --medium {iso}");
    }

    public void Start(string name)
    {
        var name2 = "\"" + name + "\"";
        ShellHelper.Bash($"VBoxHeadless --startvm {name2} &");
    }
}