using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using citools;

namespace cisteps
{   
    public class PipelineDevOpOnlyFull : IPipeline
    {
        private readonly ListAsk listAsk;
        private readonly ListResources listResources;
        Func<Task> run;

        public PipelineDevOpOnlyFull(
            IStepRunner runner,
            ListAsk listAsk,
            ListResources listResources,
            IShellCommandExecute shellCommandExecute,
            InstallVaultCmd installVaultCmd,
            DevOpConfigureVault devOpConfigureVault,
            InstallTraefikCmd installTraefikSsh,
            AddGitToBuild addGitToBuild,
            GitCloneOrPull gitCloneOrPull,
            DockerBuildCmd dockerBuildCmd
            //,
            //DockerRunCmd dockerRunCmd,
            //TraefikConfigure dockerRunCmd,  
            )
        {
            installVaultCmd.SetCommandExecute(shellCommandExecute);
            installTraefikSsh.SetCommandExecute(shellCommandExecute);
            
            gitCloneOrPull.SetCommandExecute(shellCommandExecute);
            dockerBuildCmd.SetCommandExecute(shellCommandExecute);

            this.run = async () => {
                await runner.Run(installVaultCmd);
                await runner.Run(devOpConfigureVault);
                await runner.Run(installTraefikSsh);
                await runner.Run(addGitToBuild);
                await runner.Run(gitCloneOrPull);
                await runner.Run(dockerBuildCmd);
            };
            this.listAsk = listAsk;
            this.listResources = listResources;
        }
        
        public async Task Run()
        {
            await this.run();
        }
        
    }
}