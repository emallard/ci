
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
using ciinfra;

// http://doc.rpc.gandi.net/hosting/usage.html#create-the-vm
// http://doc.rpc.gandi.net/hosting/reference.html

public class GandiXmlRPC
{

    public void AccountInfo(string apikey)
    {
        var call = new PythonCall();
        var info = call.GetObject(apikey, "printjson(api.version.info(apikey))");
        Console.WriteLine(info.api_version);
    }

    public dynamic VmList(string apikey)
    {
        var code = 
@"
    printjson(api.hosting.vm.list(apikey))
";
        var d = new PythonCall().GetArray(apikey, code);
        return d;
    }

    public int VmId(string apikey, string vmName)
    {
        var id = TryVmId(apikey, vmName);
        if (id == -1)
            throw new Exception("vm not found " + vmName);
        return id;
    }

    public int TryVmId(string apikey, string vmName)
    {
        var list = VmList(apikey);
        for (var i = 0; i < list.Count; ++i)
        {
            if (list[i]["hostname"] == vmName)
                return list[i]["id"];
        }
        return -1;
    }

    public dynamic TryVmInfo(string apikey, string vmName)
    {
        var vmId = TryVmId(apikey, vmName);
        if (vmId >= 0)
        {
            return VmInfo(apikey, vmId);
        }
        return null;
    }

    public dynamic VmInfo(string apikey, int vmId)
    {
        var code = 
@"
    vmId = %VMID%
    printjson(api.hosting.vm.info(apikey, vmId))
";
        var d = new PythonCall().GetObject(apikey, code.Replace("%VMID%", vmId.ToString()));
        return d;
    }

    public void CreateVm(string apikey, string vmName, int diskSizeInMb, string rootPassword, string user, string userPassword)
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
        'name': vmName,
        'size' : %diskSizeInMb%}
    vm_spec = {
        'datacenter_id':dc_id,
        'hostname':     vmName,
        'memory':       1024,
        'cores':        1,
        'ip_version':   4,
        'bandwidth':    102400,
        'password':     '%rootPassword%',
        'run':          'useradd -m -p %userPassword% -s /bin/bash %user% && adduser %user% sudo'}

    op = api.hosting.vm.create_from(apikey, vm_spec, disk_spec, src_disk_id)
    printjson(op)
";
        code = code.Replace("%VMNAME%", vmName);
        code = code.Replace("%rootPassword%", rootPassword);
        code = code.Replace("%user%", user);
        code = code.Replace("%userPassword%", userPassword);
        code = code.Replace("%diskSizeInMb%", diskSizeInMb.ToString());
        
        var call = new PythonCall();
        var d = call.GetArray(apikey, code);

    }


    public dynamic VmStop(string apikey, int vmId)
    {
        var code = 
@"
    printjson(hosting.vm.stop(apikey, %VMID%))
";
        var d = new PythonCall().GetObject(apikey, code.Replace("%VMID%", vmId.ToString()));
        return d;
    }


    public dynamic VmDelete(string apikey, int vmId)
    {
        var code = 
@"
    printjson(hosting.vm.delete(apikey, %VMID%))
";
        var d = new PythonCall().GetObject(apikey, code.Replace("%VMID%", vmId.ToString()));
        return d;
    }


    public dynamic IfaceInfo(string apikey, int ifaceId)
    {
        var code = 
@"
    printjson(api.hosting.iface.info(apikey, %IFACEID%))
";
        var d = new PythonCall().GetObject(apikey, code.Replace("%IFACEID%", ifaceId.ToString()));
        return d;
    }
}
