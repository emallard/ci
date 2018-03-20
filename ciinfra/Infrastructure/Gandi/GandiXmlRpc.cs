
using System.Net.Http;
using System.Text;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.ComponentModel;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// http://doc.rpc.gandi.net/hosting/usage.html#create-the-vm
// http://doc.rpc.gandi.net/hosting/reference.html

public class GandiXmlRPC
{

    public void AccountInfo()
    {
        var call = new PythonCall();
        var info = call.GetObject("printjson(api.version.info(apikey))");
        Console.WriteLine(info.api_version);
    }

    public dynamic VmList()
    {
        var code = 
@"
    printjson(api.hosting.vm.list(apikey))
";
        var d = new PythonCall().GetArray(code);
        return d;
    }

    public int VmId(string vmName)
    {
        var id = TryVmId(vmName);
        if (id == -1)
            throw new Exception("vm not found " + vmName);
        return id;
    }

    public int TryVmId(string vmName)
    {
        var list = VmList();
        for (var i = 0; i < list.Count; ++i)
        {
            if (list[i]["hostname"] == vmName)
                return list[i]["id"];
        }
        return -1;
    }

    public dynamic TryVmInfo(string vmName)
    {
        var vmId = TryVmId(vmName);
        if (vmId >= 0)
        {
            return VmInfo(vmId);
        }
        return null;
    }

    public dynamic VmInfo(int vmId)
    {
        var code = 
@"
    vmId = %VMID%
    printjson(api.hosting.vm.info(apikey, vmId))
";
        var d = new PythonCall().GetObject(code.Replace("%VMID%", vmId.ToString()));
        return d;
    }

    public void CreateVm(string vmName)
    {

        var code = 
@"
    vmName = '%VMNAME%'
    api.hosting.datacenter.list(apikey)

    fr_datacenters = [dc for dc in api.hosting.datacenter.list(apikey)
        if dc['dc_code'] == 'FR-SD5']
    dc_id = fr_datacenters[0]['id']

    images = api.hosting.image.list(apikey, {'datacenter_id': dc_id})

    ubuntu_images = [x for x in images if x['label'] == 'Ubuntu 16.04 LTS']

    src_disk_id = ubuntu_images[0]['disk_id']

    disk_spec = {
        'datacenter_id': dc_id,
        'name': vmName}
    vm_spec = {
        'datacenter_id':dc_id,
        'hostname':     vmName,
        'memory':       512,
        'cores':        1,
        'ip_version':   4,
        'bandwidth':    102400,
        'password':     '%piloteRootPassword%',
        'run':          'useradd -m -p %piloteUserPassword% -s /bin/bash %piloteUser% && adduser test sudo'}

    op = api.hosting.vm.create_from(apikey, vm_spec, disk_spec, src_disk_id)
    printjson(op)
";
        code = code.Replace("%VMNAME%", vmName);
        code = code.Replace("%piloteRootPassword%", SecretStore.GetSecret("piloteRootPassword"));
        code = code.Replace("%piloteUser%", SecretStore.GetSecret("piloteUser"));
        code = code.Replace("%piloteUserPassword%", SecretStore.GetSecret("piloteUserPassword"));
        
        var call = new PythonCall();
        var d = call.GetArray(code);

    }


    public dynamic VmStop(int vmId)
    {
        var code = 
@"
    printjson(hosting.vm.stop(apikey, %VMID%))
";
        var d = new PythonCall().GetObject(code.Replace("%VMID%", vmId.ToString()));
        return d;
    }


    public dynamic VmDelete(int vmId)
    {
        var code = 
@"
    printjson(hosting.vm.delete(apikey, %VMID%))
";
        var d = new PythonCall().GetObject(code.Replace("%VMID%", vmId.ToString()));
        return d;
    }


    public dynamic IfaceInfo(int ifaceId)
    {
        var code = 
@"
    printjson(api.hosting.iface.info(apikey, %IFACEID%))
";
        var d = new PythonCall().GetObject(code.Replace("%IFACEID%", ifaceId.ToString()));
        return d;
    }
}
