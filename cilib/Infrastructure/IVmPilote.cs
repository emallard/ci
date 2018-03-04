using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Renci.SshNet;

public interface IVmPilote {

    SshClient Ssh();
    
    void InstallCi();
    void InstallDocker();
    void InstallMirrorRegistry();
    void InstallRegistry();

    void CreateBuildContainer();
    void SetSourcesInBuildContainer();
    void RunBuildContainer();
    void CreateAppContainer();
    void PublishToAppRegistry();
}