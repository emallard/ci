using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Text;

public class ResourceHelper {
    
    public async Task<string> ReadAsTextAsync(Assembly assembly, string name)
    {
        var resourceStream = assembly.GetManifestResourceStream("EmbeddedResource." + name);
        using (var reader = new StreamReader(resourceStream, Encoding.UTF8))
        {
            return await reader.ReadToEndAsync();
        }
    }
}