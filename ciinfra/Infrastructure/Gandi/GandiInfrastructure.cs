using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

public class GandiInfrastructure : IInfrastructure
{
    private readonly GandiXmlRPC xmlRPC;
    private readonly VmPilote vmPilote;

    public string DomainName => "company.com";
    public string CidataDirectory => "/home/admin/cidata";


    public GandiInfrastructure(
        GandiXmlRPC xmlRPC,
        VmPilote vmPilote)
    {
        this.xmlRPC = xmlRPC;
        this.vmPilote = vmPilote;

        this.vmPilote.VmName = "pilote";
        //this.vmPilote.Ip = new IPAddress(new byte[]{10,0,2,5});
        this.vmPilote.PortForward = 22;
        this.vmPilote.PrivateRegistryPort = 5443;
        this.vmPilote.PrivateRegistryDomain = "privateregistry.mynetwork.local";
        //this.vmPilote.SshUri = new Uri($"tcp://127.0.0.1:{vmPilote.PortForward}");
        this.vmPilote.SshUser = SecretStore.GetSecret("piloteUser");
        this.vmPilote.SshPassword = SecretStore.GetSecret("piloteUserPassword");

        var vmInfo = xmlRPC.TryVmInfo(vmPilote.VmName);
        if (vmInfo != null)
        {
            int ifaceId = vmInfo["ifaces_id"][0];
            var ifaceInfo = xmlRPC.IfaceInfo(ifaceId);
            var ipv4 = ifaceInfo.ips[0]["ip"];
            this.vmPilote.Ip = IPAddress.Parse(ipv4);
        }
        //vmPilote.Ip
    }

    public void TryToStartVmPilote()
    {
        var vmList = xmlRPC.VmList();
        //var vmInfo = xmlRPC.VmInfo(vmPilote.VmName);
        
    }

    public void CreateVmPilote()
    {
        var vmInfo = xmlRPC.TryVmInfo(vmPilote.VmName);

        //CheckVmDoesNotExists(vmName);
        xmlRPC.CreateVm(this.vmPilote.VmName);
        Thread.Sleep(30000);
        var vmInfo1 = xmlRPC.TryVmInfo(vmPilote.VmName);
        Thread.Sleep(30000);
        var vmInfo2 = xmlRPC.TryVmInfo(vmPilote.VmName);
        Thread.Sleep(1000);
    }

    public IVmPilote GetVmPilote()
    {
        return this.vmPilote;
    }

    public void DeleteVmPilote()
    {
        /*
        var vmId = xmlRPC.TryVmId(vmPilote.VmName);
        if (vmId > 0)
        {
            xmlRPC.VmStop(vmId);
            xmlRPC.VmDelete(vmId);
        }*/
    }




    public void CreateVmWebServer()
    {
        throw new NotImplementedException();
    }

    
    public void DeleteVmWebServer()
    {
        throw new NotImplementedException();
    }

    

    public IVmWebServer GetVmWebServer()
    {
        throw new NotImplementedException();
    }

    

    public void TryToStartVmWebServer()
    {
        throw new NotImplementedException();
    }
}