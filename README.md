
# Accédere au daemon docker

Commandes:
```
sudo systemctl status docker.service
sudo systemctl stop docker
sudo journalctl -fu docker.service
sudo nano /lib/systemd/system/docker.service
```

# impossible de changer hosts via daemon.json

Pour passer dockerd en TCP avec systemd, il ne faut pas passer par /etc/docker/daemon.json,
Il faut éditer /lib/systemd/system/docker.service.

The default location of the configuration file on Linux is /etc/docker/daemon.json. The --config-file flag can be used to specify a non-default location.

This is a full example of the allowed configuration options on Linux:
```
{
    "hosts": ["unix:///var/run/docker.sock","tcp://0.0.0.0:2375"]
}
```
