using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

public class CreateServerDev {

    public void Create()
    {
        ShellHelper.Bash("VBoxManage startvm \"Ubuntu Server\" --type headless");
    }
}