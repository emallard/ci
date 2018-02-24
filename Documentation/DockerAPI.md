# Swagger docker API

https://docs.docker.com/develop/sdk/
https://docs.docker.com/engine/api/v1.35/

Run docker daemonon tcp : https://docs.docker.com/config/daemon/#configure-the-docker-daemon

## Essai NSWAG

npm install -g nswag
nswag help /runtime:NetCore20
nswag swagger2csclient /runtime:NetCore20 /input:swagger.yaml /classname:MyServiceClient /namespace:MyNamespace /output:MyServiceClient.cs /InjectHttpClient
https://github.com/RSuter/NSwag/wiki/SwaggerToCSharpClientGenerator

## Essai Autorest

https://medium.com/@pjausovec/creating-c-client-library-for-web-api-projects-be132c831f9c

autorest --input-file=swagger.yaml --output-folder=generated_csharp --csharp

## Essai swagger codegen
https://swagger.io/docs/swagger-tools/

java -jar swagger-codegen-cli-2.2.1.jar generate -i https://docs.docker.com/engine/api/v1.36/swagger.yaml -l csharp

https://github.com/Gronsak/swagger-codegen/tree/master/samples/client/petstore/csharp/SwaggerClient

## essai docker.dotnet
OLD : <PackageReference Include="docker.dotnet" Version="3.125.1" />

```
var client = new DockerClientConfiguration(new Uri("unix:///var/run/docker.sock")).CreateClient();

var parameters = new ImagesListParameters();
var list = await client.Images.ListImagesAsync(parameters);

foreach (var l in list)
{
    if (l.RepoTags != null)
        Console.WriteLine(l.RepoTags[0]);
}

var createParameters = new CreateContainerParameters();
await client.Containers.CreateContainerAsync(new CreateContainerParameters() {
    Image = "microsoft/aspnetcore-build:2.0",
    Name = "ci_build",
    Volumes = new Dictionary<string,EmptyStruct>() {{"sources:/sources", new EmptyStruct()}}
});
```

## Code pour être indépendant de Microsoft/Docker.Dotnet

DockerClient.cs

``` cs
    public static MyNamespace.MyServiceClient Create()
    {
        var pipeString = "/var/run/docker.sock";
        var handler = new ManagedHandler(async (string host, int port, CancellationToken cancellationToken) =>
                    {
                        var sock = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified);
                        await sock.ConnectAsync(new Microsoft.Net.Http.Client.UnixDomainSocketEndPoint(pipeString));
                        return sock;
                    });
        var uri = new Uri("http://docker.sock");

        var _client = new HttpClient(handler, true);
        var client = new MyNamespace.MyServiceClient("http://docker.sock", _client);
        _client.Timeout = s_InfiniteTimeout;
        return client;
    }
```

``` cs
var client = DockerClient.Create();

        var auth = 
@"{
  ""username"": ""---"",
  ""password"": ""---"",
  ""email"": ""---""
}";
//,  ""serveraddress"": ""string""
byte[] encodedBytes = System.Text.Encoding.Unicode.GetBytes(auth);
string encodedTxt = Convert.ToBase64String(encodedBytes);

        var searchImages = await client.ImageSearchAsync("registry", 1, "");
        var registryImage = searchImages[0];
        await client.ImageCreateAsync(
            fromImage:registryImage.Name, 
            repo:"registry", 
            tag:"2", 
            fromSrc:null, 
            inputImage:null, 
            x_Registry_Auth:encodedTxt, 
            platform:null );



        while (true)
        {
            Thread.Sleep(1000);
            var images = await client.ImageListAsync(true, "", false); 
            if (images.Any(i => i.Id == "registry"))
             
```


``` cs
    var client = DockerClient.Create();
    var liste = await client.ContainerListAsync(null, null, null, @"{""name"":[""private-repository""]}");//, `{name:"private-repository"}`);
    if (liste.Count > 0) 
    {
        await client.ContainerDeleteAsync(liste[0].Id, null, null, null);
    }
```