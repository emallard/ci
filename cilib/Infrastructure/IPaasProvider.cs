using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

public interface IPaasProvider  {

    void CreateVm(string name, string login, string password);
}