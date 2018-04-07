#Traefik

## first run

```
curl https://raw.githubusercontent.com/containous/traefik/master/traefik.sample.toml
docker run -d -p 8080:8080 -p 80:80 -v $PWD/traefik.toml:/etc/traefik/traefik.toml traefik
```
http://127.0.0.1:8080 => admin


ou:
```
docker run -d -p 8080:8080 -p 80:80 traefik --api
```

# file configuration

With a [file] backend, add in traefik.toml:
``` toml
[file]
  filename = "rules.toml"
```

then in rules.toml:
```
[frontends]
  [frontends.frontend1]
    backend = "backend1"
    passHostHeader = true
    [frontends.frontend1.routes]
        [frontends.frontend1.routes.route0]
            rule = "Host:recette.myclient.com"
[backends]
  [backends.backend1]
      [backends.backend1.servers]
        [backends.backend1.servers.server0]
          url = "http://10.2.0.6:9001"
          weight = 1
```

# docker configuration

https://docs.traefik.io/user-guide/docker-and-lets-encrypt/