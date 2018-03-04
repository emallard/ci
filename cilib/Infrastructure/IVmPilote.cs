using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Renci.SshNet;

public interface IVmPilote : IVm {

    void InstallDocker();
    void InstallMirrorRegistry();
    void InstallRegistry();

    void InstallCiSources();
    void CleanCiSources();

    void BuildCiImage();
    void CleanCiImage();

    void RunCiContainer();
    void CleanCiContainer();

    void CreateBuildContainer();
    void SetSourcesInBuildContainer();
    void RunBuildContainer();

    void CreateAppContainer();
    void PublishToAppRegistry();
}
