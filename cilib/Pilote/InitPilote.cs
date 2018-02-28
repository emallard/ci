


using System;
using System.Net;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;

public class InitPilote {
    private readonly InitVault initVault;
    private readonly InitCA initCA;
    private readonly InitRegistry initRegistry;
    private readonly IConfigInit configInit;

    public InitPilote(
        InitVault initVault,
        InitCA initCA, 
        InitRegistry initRepository,
        IConfigInit configInit) 
    {
        this.initVault = initVault;
        this.initCA = initCA;
        this.initRegistry = initRepository;
        this.configInit = configInit;
    }

    public async Task Run()
    {
        await this.Clean();
        await this._Init();
    }

    public async Task Clean()
    {

        if (this.configInit.CleanVaultContainer)
            await this.initVault.CleanVaultContainer();

        if (this.configInit.CleanVaultImage)
            await this.initVault.CleanVaultImage();

        
        
        //this.initCA.Clean();

        if (this.configInit.CleanRegistryContainer)
            await this.initRegistry.CleanRegistryContainer();

        if (this.configInit.CleanRegistryImage)
            await this.initRegistry.CleanRegistryImage();
    }


    public async Task _Init()
    {
        if (this.configInit.CreateVaultContainer)
            await this.initVault.Init();

        if (this.configInit.CreateRootCA)
            await this.initCA.CreateRootCA();

        if (this.configInit.CreateRegistryContainer)
            await this.initRegistry.Init();

        // installer pipeline
    }
}

