using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Docker.DotNet.Models;
using VaultSharp;
using VaultSharp.Backends.Authentication.Models;
using VaultSharp.Backends.Authentication.Models.Token;

public class InstallVault
{
    private readonly DockerWrapper dockerWrapper;
    private string repoTag = "vault:0.9.4";
    private string containerName = "my-vault";

    public InstallVault(DockerWrapper dockerWrapper)
    {
        this.dockerWrapper = dockerWrapper;
    }

    public async Task CleanVaultImage() 
    {
        await dockerWrapper.DeleteImageIfExists(repoTag);
    }

    public async Task CleanVaultContainer() 
    {
        await dockerWrapper.DeleteContainerIfExistsByName(containerName);
    }

    public async Task Init()
    {
        await InitVaultImage();
        await InitVaultContainer();
    }

    public async Task InitVaultImage() 
    {
        await dockerWrapper.CreateImageIfNotFound(repoTag);
    }

    public async Task InitVaultContainer() 
    {
        var registryImage = await dockerWrapper.FindImage(repoTag);
        using (var client = dockerWrapper.GetClient())
        {
            var p = new CreateContainerParameters();
            p.Image = registryImage.ID;
            //p.Volumes = new Dictionary<string, EmptyStruct>();
            //p.Volumes.Add("/certs:/certs", new EmptyStruct());
            p.ExposedPorts = new Dictionary<string, EmptyStruct>();
            
            p.HostConfig = new HostConfig();
            p.HostConfig.CapAdd = new List<string> {"IPC_LOCK"};
            p.HostConfig.PortBindings = new Dictionary<string, IList<PortBinding>>();
            p.HostConfig.PortBindings.Add("8200/tcp", new List<PortBinding> {
                new PortBinding() {
                    HostIP = "0.0.0.0",
                    HostPort = "8200"
                }
            });
            p.Env = new List<string>()
            {
                "VAULT_DEV_ROOT_TOKEN_ID=myroot"
            };
            p.Name = containerName;
            var containerResponse = await client.Containers.CreateContainerAsync(p);

            var startP = new ContainerStartParameters();
            await client.Containers.StartContainerAsync(containerResponse.ID, startP);
        }
    }
}