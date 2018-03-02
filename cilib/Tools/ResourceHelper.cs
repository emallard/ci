using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Text;

public class EmbeddedResource
{
    public readonly string Name;

    public EmbeddedResource(string name)
    {
        Name = name;
    }

    public Stream Stream()
    {
        return this.GetType().Assembly.GetManifestResourceStream("cilib." + Name);
    }

    public string ReadAsText()
    {
        var resourceStream = this.GetType().Assembly.GetManifestResourceStream("cilib." + Name);
        using (var reader = new StreamReader(resourceStream, Encoding.UTF8))
        {
            return reader.ReadToEnd();
        }
    }

}

public class EmbeddedResources
{

    public static EmbeddedResource VBoxClone = new EmbeddedResource("Infrastructure.VBox.VBoxClone.sh");
    public static EmbeddedResource InstallDocker = new EmbeddedResource("Infrastructure.Common.InstallDocker.sh");
    public static EmbeddedResource InstallCi = new EmbeddedResource("Infrastructure.InstallCi.sh");
}


public class ResourceHelper 
{
    
    // static constructor that checks that resources exists
    static ResourceHelper() 
    {
        var assembly = typeof(ResourceHelper).Assembly;
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