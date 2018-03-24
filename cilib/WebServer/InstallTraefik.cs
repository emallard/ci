using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Docker.DotNet.Models;

public class InstallTraefik {

    private readonly DockerWrapper dockerWrapper;
    private readonly ICiLibCiDataDirectory cidataDir;
    private readonly IInfrastructure infrastructure;
    string traefikRepoTag = "traefik:1.5";
    string containerName = "traefik";

    public InstallTraefik(
        DockerWrapper dockerWrapper,
        ICiLibCiDataDirectory cidataDir,
        IInfrastructure infrastructure)
    {
        this.dockerWrapper = dockerWrapper;
        this.cidataDir = cidataDir;
        this.infrastructure = infrastructure;
    }

    public async Task Install() 
    {
        //docker run -d -p 8080:8080 -p 80:80 -v $PWD/traefik.toml:/etc/traefik/traefik.toml traefik

        var traefikDir = cidataDir + "/traefik"; 
        if (!Directory.Exists(traefikDir))
            Directory.CreateDirectory(traefikDir);
        File.WriteAllText(Path.Combine(traefikDir, "traefik.toml"), EmbeddedResourcesCiLib.TraefikToml.ReadAsText());


        
        await dockerWrapper.CreateImageIfNotFound(traefikRepoTag);
        var traefikImage = await dockerWrapper.FindImage(traefikRepoTag);


        using (var client = this.dockerWrapper.GetClient())
        {
            var infraCidata = "/cidata";
            var p = new CreateContainerParameters();
            p.Image = traefikImage.ID;

            p.ExposedPorts = new Dictionary<string, EmptyStruct>();
            p.ExposedPorts.Add("8080/tcp", new EmptyStruct());
            
            p.Name = containerName;
            
            p.HostConfig = new DockerHostConfig()
                .Bind(infraCidata + "/traefik/traefik.toml:/etc/traefik/traefik.toml")
                .PortBinding("0.0.0.0", "8080", "8080/tcp")
                .PortBinding("0.0.0.0", "80", "80/tcp")
                .RestartAlways()
                .GetConfig();
            
            var response = await client.Containers.CreateContainerAsync(p);


            var p2 = new ContainerStartParameters();
            await client.Containers.StartContainerAsync(response.ID, p2);
        }

        // Traffic network so that traefic can communicate with other containers
        using (var client = this.dockerWrapper.GetClient())
        {
            //client.Networks.CreateNetworkAsync();
        }
    }


    public async Task Clean() 
    {
        await dockerWrapper.DeleteContainerIfExistsByName(containerName);
        //await dockerWrapper.DeleteImageIfExists(traefikRepoTag);
    }

}