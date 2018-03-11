#Traefik

## first run

```
curl https://raw.githubusercontent.com/containous/traefik/master/traefik.sample.toml
docker run -d -p 8080:8080 -p 80:80 -v $PWD/traefik.toml:/etc/traefik/traefik.toml traefik
```