using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.IO;

public class SecretStore {

    public static string GetSecret(string key)
    {
        var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        var fileContent = File.ReadAllText(path + "/.microsoft/usersecrets/secrets.json");
        var secrets = JsonConvert.DeserializeObject<Dictionary<string, string>>(fileContent);
        return secrets[key];
    }
}