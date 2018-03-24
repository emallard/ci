using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using cisystem;

public class TestWebApp {

    CiSystemConfig ciSystemConfig;
    CiSystem ciSystem;
    
    public void Run()
    {
        ciSystem = new CiSystem();
        ciSystem.Configure(ciSystemConfig);
        ciSystem.Build("ervad-master");
        ciSystem.Deploy("ervad-recette");
    }

    void TestOk() 
    {
        // Run http request
    }

    void Prerequisite() 
    {
        // Check that VM call "webserver is setup
        ciSystem.CheckVmSsh(ciSystem.Pilote);
        ciSystem.CheckVmSsh(ciSystem.WebServer);

        ciSystem.CheckCiexe(ciSystem.Pilote);
        ciSystem.CheckCiexe(ciSystem.WebServer);

        ciSystem.CheckPrivateRegistry(ciSystem.Pilote);
        ciSystem.CheckPrivateRegistryConnection(ciSystem.WebServer, ciSystem.Pilote);

        ciSystem.CheckTraefik(ciSystem.WebServer);
    }

    void Setup()
    {
        ciSystemConfig = new CiSystemConfig();
        ciSystemConfig.ListBuildConfig.Add(new CiBuildConfig() {
            Name = "dotnetcore_0-master",
            GitUrl = "https://github.com/emallard/dotnetcore_0.git",
            Branch = "master"
        });

        ciSystemConfig.ListBuildConfig.Add(new CiBuildConfig() {
            Name = "mantisbt-master",
            GitUrl = "https://github.com/emallard/mantisbt.git",
            Branch = "master"
        });

        ciSystemConfig.ListDeploymentConfig.Add(new CiDeploymentConfig() {
            Name = "dotnetcore_0-prod",
            BuildConfig = "ervad-master",
            WebServer = "MainWebServer",
            Url = "www.dotnetcore_0.mycompany.com"
        });

        ciSystemConfig.ListDeploymentConfig.Add(new CiDeploymentConfig() {
            Name = "dotnetcore_0-recette",
            BuildConfig = "ervad-master",
            WebServer = "MainWebServer",
            Url = "recette.dotnetcore_0.mycompany.com"
        });

        ciSystemConfig.ListDeploymentConfig.Add(new CiDeploymentConfig() {
            Name = "mantisbt-prod",
            BuildConfig = "mantisbt-master",
            WebServer = "MainWebServer",
            Url = "www.bugtracker.mycompany.com"
        });

    }
}