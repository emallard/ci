using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Renci.SshNet;

public interface IVmPilote : IVm {

    string VmName {get; }
    IPAddress Ip { get; }
    int PortForward {get ;}

    string PrivateRegistryDomain {get ;}
    int PrivateRegistryPort {get ;}

    void InstallHosts();
    void CleanHosts();
    void InstallDocker();

    void InstallCA();
    void CleanCA();
    
    void InstallMirrorRegistry();

    void InstallPrivateRegistry();
    void CleanPrivateRegistry();

    void InstallCiSources();
    void CleanCiSources();

    void BuildCiImage();
    void CleanCiImage();

    void Build();
    void Publish();
}
