using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Renci.SshNet;

public interface IVmWebServer : IVm {

    void InstallHosts();
    void CleanHosts();
    void InstallDocker();
    void InstallMirrorRegistry();
    
}
