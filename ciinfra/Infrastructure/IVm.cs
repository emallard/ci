using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Renci.SshNet;
using citools;

namespace ciinfra
{
    public interface IVm {

        void SetSshConnection(SshConnection sshConnection);
        SshClient Ssh();
        ScpClient Scp();
    }
}