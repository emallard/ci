using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

public interface IInfrastructure {

    void TryToStartVmPilote();
    void DeleteVmPilote();
    void CreateVmPilote();
    IVmPilote GetVmPilote();

    void TryToStartVmWebServer();
    void DeleteVmWebServer();
    void CreateVmWebServer();
    IVmWebServer GetVmWebServer();
}