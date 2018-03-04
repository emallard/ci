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
        var stream = this.GetType().Assembly.GetManifestResourceStream("cilib." + Name);
        if (stream == null)
            throw new Exception("No stream for resource : " + "cilib." + Name);
        return stream;
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
