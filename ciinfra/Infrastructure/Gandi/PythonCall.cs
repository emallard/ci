using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;
using Newtonsoft.Json.Linq;

public enum PythonResultType
{
    Object,
    Array
}

public class PythonCall
{
    public dynamic GetObject(string commandLine)
    {
        return Run(PythonResultType.Object, commandLine);
    }

    public dynamic GetArray(string commandLine)
    {
        return Run(PythonResultType.Array, commandLine);
    }

    private dynamic Run(PythonResultType resultType, string commandLine)
    {
        var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        var filename = Path.Combine(path, "tmp.py");
        var filecode = PyStart();
        filecode += "    " + commandLine;
        File.WriteAllText(filename, filecode);

        var apiKey = SecretStore.GetSecret("apikey");
        var process = new Process()
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = $"{filename} {apiKey}",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };
        process.Start();
        string result = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        if (process.ExitCode != 0)
            throw new Exception($"Python error ({process.ExitCode})");

        Console.WriteLine(result);

        dynamic d;
        if (resultType == PythonResultType.Object)
            d = JObject.Parse(result);
        else
            d = JArray.Parse(result);
        return d;
    } 

    public string PyStart()
    {
return @"
import xmlrpclib
import sys
import json

def printjson(obj):
    print json.dumps(obj, default=str)

if __name__ == '__main__':
    api = xmlrpclib.ServerProxy('https://rpc.gandi.net/xmlrpc/')
    apikey = sys.argv[1]
";
    }
}