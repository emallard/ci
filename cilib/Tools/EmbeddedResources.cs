using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Text;

public class EmbeddedResources
{

    public static EmbeddedResource InstallDocker = new EmbeddedResource("Infrastructure.Common.InstallDocker.sh");

    static EmbeddedResources() 
    {
        var assembly = typeof(EmbeddedResources).Assembly;
        var names = assembly.GetManifestResourceNames();
        var fields = typeof(EmbeddedResources).GetFields();

        foreach (var f in fields)
        {
            var fvalue = (EmbeddedResource) f.GetValue(null);
            if (!names.Any(n => n == "cilib." + fvalue.Name))
                throw new Exception($"Resource not found : {fvalue}");
        }
    }
}