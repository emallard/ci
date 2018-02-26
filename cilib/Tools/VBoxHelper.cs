using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

public class VBoxHelper {

    public static bool IsVmStarted(string vmName)
    {
        var r = ShellHelper.Bash($"vboxmanage showvminfo \"{vmName}\" | grep -c \"running (sinc\"");
        return r.StartsWith("1");
    }

    public static void CheckVmStarted(string vmName)
    {
        if (!IsVmStarted(vmName))
            throw new Exception($"VBox {vmName} not started, use : VBoxHeadless --startvm \"{vmName}\"");
    }
}