using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Renci.SshNet;
using System.Threading;
using System.Text.RegularExpressions;
using System.IO;

public class VBoxVmWebServer : VBoxVmCommon, IVmWebServer {

    public VBoxVmWebServer()
    {
    }

    public void InstallTraefik()
    {
        throw new NotImplementedException();
    }

    public void AddOrUpdateContainer(ContainerConf conf)
    {
        throw new NotImplementedException();
    }

    public void RemoveContainer(ContainerConf conf)
    {
        throw new NotImplementedException();
    }
}