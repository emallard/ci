using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Renci.SshNet;

public interface IVmPilote {

    SshClient Connect();
    
    void InstallDocker();

    void InstallMirrorRegistry();
    
    void InstallCi();
    
    void InstallRegistry();
}