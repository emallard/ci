using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Renci.SshNet;

public interface IVmWebServer {

    SshClient Ssh();
    
    void InstallDocker();
    void InstallMirrorRegistry();

    void InstallTraefik();
    void AddOrUpdateContainer(ContainerConf conf);
    void RemoveContainer(ContainerConf conf);
}

public class ContainerConf
{
    /*
    string containerName;
    string hostName;
    string dockerComposeXml;
    */
}