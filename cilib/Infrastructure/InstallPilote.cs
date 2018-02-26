using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

public class InstallPilote {
    private readonly ResourceHelper resourceHelper;
    private readonly InfraConfig infraConfig;

    public InstallPilote(
        ResourceHelper resourceHelper,
        InfraConfig infraConfig)
    {
        this.resourceHelper = resourceHelper;
        this.infraConfig = infraConfig;
    }

    public void Install()
    {
        // clone aClone.ovf
        // rdp to it and ifconfig.

        // ssh to fresh install
        // install docker
        // install git
    }

    public void CheckInstall()
    {
        CheckInstallDocker();
    }

    public async Task InstallDocker()
    {
        var sh = await resourceHelper.ReadAsTextAsync(this.GetType().Assembly, "Infrastructure.InstallPilote.sh");
        var result = ShellHelper.BashAndStdIn($"ssh test@{infraConfig.PiloteIp} 'bash -s' < /dev/stdin", sh);
    }

    public void CheckInstallDocker()
    {
        string err;
        var result = ShellHelper.BashAndStdErr(Ssh("docker run hello-world"), out err);
        if (!result.Contains("Hello World"))
        {
            throw new Exception("Docker not installed properly : " + err);
        }
    }

    private string Ssh(string cmdLine)
    {
        return $"ssh test:test@{infraConfig.PiloteIp} 'bash -s {cmdLine}'";
    }
        /*
        sudo apt-get update

        sudo apt-get install \
            apt-transport-https \
            ca-certificates \
            curl \
            software-properties-common

        sudo add-apt-repository \
            "deb [arch=amd64] https://download.docker.com/linux/ubuntu \
            $(lsb_release -cs) \
            stable"

        sudo apt-get update

        sudo apt-get install docker-ce

        sudo usermod -aG docker $USER

        reboot

        docker run hello-world
        */

        /*
        Installer git
        Installer un build et un runtime dotnetcore pour pouvoir éxécuter le code d'installation du CI
        docker pull microsoft/aspnetcore-build:2.0
        
        Build github/emallard/ci.git
        run it to install the registry
        run it to build the project github/emallard/aspdotnet_0.git

        Create a webserver
        Install docker
        Get WebApp from pilote registry

        Install traefik and register new webapp
         */
    
}