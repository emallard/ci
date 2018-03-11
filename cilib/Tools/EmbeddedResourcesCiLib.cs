using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Text;

public class EmbeddedResourcesCiLib
{

    public static EmbeddedResourceCiLib TraefikToml = new EmbeddedResourceCiLib("WebServer.traefik.toml");

    static EmbeddedResourcesCiLib() 
    {
        var assembly = typeof(EmbeddedResourcesCiLib).Assembly;
        var names = assembly.GetManifestResourceNames();
        var fields = typeof(EmbeddedResourcesCiLib).GetFields();

        foreach (var f in fields)
        {
            var fvalue = (EmbeddedResourceCiLib) f.GetValue(null);
            if (!names.Any(n => n == "cilib." + fvalue.Name))
                throw new Exception($"Resource not found : {fvalue}");
        }
    }
}