using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

public class GandiProvider : IPaasProvider{

    //https://nakkaya.com/2012/08/30/create-manage-virtualBox-vms-from-the-command-line/

    public void CreateVm(string name, string login, string password)
    {
        var name2 = "\"" + name + "\"";
        ShellHelper.BashAndStdIn($"gandi vm create --vlan vlan --hostname {name} --login {login} --password ", password + "\r\n" + password);
    }

    
    public void Start(string name)
    {

    }
}