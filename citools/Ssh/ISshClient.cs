using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace citools
{
    public interface ISshClient : ICommandExecute
    {
        ISshClient Connect(SshConnection sshConnection);

        void SudoReboot();
    }
}