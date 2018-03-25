using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Renci.SshNet;

namespace ciinfra
{
    public interface IVmPilote : IVm {

        string PrivateRegistryDomain {get ;}
        int PrivateRegistryPort {get ;}

        void InstallHosts();
        void CleanHosts();
        void InstallDocker();
        void InstallMirrorRegistry();

        void CloneOrPullCiSources();
        void CleanCiSources();

        void BuildCiImage();
        void CleanCiImage();

        // Alternative Build using installed dotnetcore
        void InstallDotNetCoreSdk();
        void BuildCiUsingSdk();
    }
}