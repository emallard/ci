using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using Microsoft.Net.Http.Client;
using System.Net.Sockets;
using System.Text;
using System.Diagnostics;


public class NSwagDockerClientCreate
{
    public static MyNamespace.NSwagDockerClient Create()
    {
        var pipeString = "/var/run/docker.sock";
        var handler = new ManagedHandler(async (string host, int port, CancellationToken cancellationToken) =>
                    {
                        var sock = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified);
                        await sock.ConnectAsync(new UnixDomainSocketEndPoint(pipeString));
                        return sock;
                    });
        var uri = new Uri("http://docker.sock");

        var _client = new HttpClient(handler, true);
        var client = new MyNamespace.NSwagDockerClient("http://docker.sock", _client);
        _client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);;
        return client;
    }





    public static async Task<string> BuildImageAsync(
        System.IO.Stream tarStream,
        string tag,
        string remote,
        bool rm,
        bool forcerm
    )
    {
        var client = NSwagDockerClientCreate.Create();

        //System.IO.Stream inputStream = tarStream;
        /*
        string dockerfile = null;
        */string t = tag;
        /*string extrahosts;
        */
        //string remote = "https://github.com/emallard/dotnetcore_0.git";
        /*
        bool? q = null;
        bool? nocache, 
        string cachefrom, 
        string pull, 
        bool? rm, 
        bool? forcerm, 
        int? memory, 
        int? memswap, 
        int? cpushares, 
        string cpusetcpus, 
        int? cpuperiod, 
        int? cpuquota, 
        int? buildargs, 
        int? shmsize, 
        bool? squash, 
        string labels, 
        string networkmode, 
        */
        MyNamespace.ContentType? content_type = MyNamespace.ContentType.Application_xTar;
        /*
        string x_Registry_Config, 
        string platform
        */
        return await client.ImageBuildAsync(tarStream, null, t,null,remote,null,null,null,null,rm,forcerm,null,null,null,null,null,null,null,null,null,null,null,content_type,null,null, CancellationToken.None);

    }
}



