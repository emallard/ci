using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

public class GandiInfrastructure : IInfrastructure
{
    public string DomainName => "company.com";

    public void CreateVmPilote()
    {
        throw new NotImplementedException();
    }

    public void CreateVmWebServer()
    {
        throw new NotImplementedException();
    }

    public void DeleteVmPilote()
    {
        throw new NotImplementedException();
    }

    public void DeleteVmWebServer()
    {
        throw new NotImplementedException();
    }

    public IVmPilote GetVmPilote()
    {
        throw new NotImplementedException();
    }

    public IVmWebServer GetVmWebServer()
    {
        throw new NotImplementedException();
    }

    public void TryToStartVmPilote()
    {
        throw new NotImplementedException();
    }

    public void TryToStartVmWebServer()
    {
        throw new NotImplementedException();
    }
}