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

    public Stream Read()
    {
        return this.GetType().Assembly.GetManifestResourceStream("EmbeddedResource." + Name);
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
    public static EmbeddedResource InstallPilote_1 = new EmbeddedResource("Infrastructure.InstallPilote_1.sh");
    public static EmbeddedResource InstallPilote_2 = new EmbeddedResource("Infrastructure.InstallPilote_2.sh");
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

    public Stream Read(EmbeddedResource res)
    {
        return this.GetType().Assembly.GetManifestResourceStream("EmbeddedResource." + res.Name);
    }

    public async Task<string> ReadAsTextAsync(Assembly assembly, string name)
    {
        
        var resourceStream = assembly.GetManifestResourceStream("EmbeddedResource." + name);
        using (var reader = new StreamReader(resourceStream, Encoding.UTF8))
        {
            return await reader.ReadToEndAsync();
        }
    }

    public string ReadAsText(Assembly assembly, string name)
    {
        var resourceStream = assembly.GetManifestResourceStream("EmbeddedResource." + name);
        using (var reader = new StreamReader(resourceStream, Encoding.UTF8))
        {
            return reader.ReadToEnd();
        }
    }
}