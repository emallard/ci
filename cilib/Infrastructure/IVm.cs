using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Renci.SshNet;

public interface IVm {

    SshClient Ssh();
    ScpClient Scp();
}
