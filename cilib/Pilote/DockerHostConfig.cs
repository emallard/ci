

using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Docker.DotNet.Models;

public class DockerHostConfig {

    HostConfig hostConfig;

    public DockerHostConfig()
    {
        hostConfig = new HostConfig();
        hostConfig.Binds = new List<string>();
        hostConfig.PortBindings = new Dictionary<string, IList<PortBinding>>();
    }
    
    public DockerHostConfig Bind(string mount)
    {
        hostConfig.Binds.Add(mount);
        return this;
    }

    public DockerHostConfig PortBinding(string hostIp, string hostPort, string containerPort)
    {
        var portBinding = new PortBinding();
        portBinding.HostIP = hostIp;
        portBinding.HostPort = hostPort;
        hostConfig.PortBindings = new Dictionary<string, IList<PortBinding>>();
        hostConfig.PortBindings.Add(containerPort, new List<PortBinding>() {portBinding});
        
        return this;
    }

    public DockerHostConfig RestartAlways()
    {
        hostConfig.RestartPolicy = new RestartPolicy();
        hostConfig.RestartPolicy.Name = RestartPolicyKind.Always;
        return this;
    }

    public HostConfig GetConfig()
    {
        return hostConfig;
    }
}