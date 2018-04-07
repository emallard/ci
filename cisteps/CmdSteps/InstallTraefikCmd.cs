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
            await Task.CompletedTask;
            cmdTraefik.InstallTraefik(commandExecute, "/home/etienne/");
        }

        public async Task Check()
        {
            await Task.CompletedTask;
            cmdTraefik.Check(commandExecute);
        }
    }
}