using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using citools;
using ciinfra;
using cilib;

namespace cisteps
{
    public class InstallTraefikCmd : IStep
    {
        private readonly SshStep pstep;
        private readonly CmdTraefik cmdTraefik;
        private ICommandExecute commandExecute;

        public InstallTraefikCmd(
            SshStep pstep,
            CmdTraefik cmdTraefik)
        {
            this.pstep = pstep;
            this.cmdTraefik = cmdTraefik;
        }

        public void SetCommandExecute(ICommandExecute commandExecute)
        {
            this.commandExecute = commandExecute;
        }

        public async Task Clean()
        {
            await Task.CompletedTask;
            cmdTraefik.Clean(commandExecute);
        }

        public async Task Run()
        {
            var traefikConfigPath = await pstep.listAsk.TraefikConfigPath.Ask();
            await pstep.listResources.TraefikConfigPath.Write(await GetAuthentication(), traefikConfigPath);
            cmdTraefik.InstallTraefik(commandExecute, traefikConfigPath);
        }

        public async Task Check()
        {
            await Task.CompletedTask;
            cmdTraefik.Check(commandExecute);
        }

        private async Task<IAuthenticationInfo> GetAuthentication()
        {
            IAuthenticationInfo auth = new UserPasswordAuthenticationInfo(
                await pstep.listAsk.LocalVaultDevopUser.Ask(),
                await pstep.listAsk.LocalVaultDevopPassword.Ask());
            return auth;
        }
    }
}