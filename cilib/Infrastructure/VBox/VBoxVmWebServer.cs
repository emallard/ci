using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Renci.SshNet;
using System.Threading;
using System.Text.RegularExpressions;
using System.IO;

public class VBoxVmWebServer : VBoxVm, IVmWebServer {

    public string VmName => "webserver";
    public IPAddress Ip => new IPAddress(new byte[]{10,0,2,6});
    public int PortForward => 22006;
    
    public VBoxVmWebServer()
    {
    }

    public void InstallHosts()
    {
        throw new NotImplementedException();
    }

    public void CleanHosts()
    {
        throw new NotImplementedException();
    }
}