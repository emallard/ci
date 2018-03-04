
# Génération d'appels à l'API docker en utilisant NSwag
```
npm install -g nswag
```
```
nswag help /runtime:NetCore20
nswag swagger2csclient /runtime:NetCore20 /input:swagger2.yaml /classname:NSwagDockerClient /namespace:MyNamespace /output:NSwagDockerClient.cs /InjectHttpClient
```
https://github.com/RSuter/NSwag/wiki/SwaggerToCSharpClientGenerator