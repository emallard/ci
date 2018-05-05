
## Uncomplicated Firewall

https://wiki.ubuntu.com/UncomplicatedFirewall

https://www.digitalocean.com/community/tutorials/how-to-setup-a-firewall-with-ufw-on-an-ubuntu-and-debian-cloud-server

```
sudo ufw allow ssh/tcp
sudo ufw allow 80/tcp
sudo ufw allow 443/tcp
sudo ufw allow from 127.0.0.1 8080/tcp
#sudo ufw allow from 127.0.0.1 to 127.0.0.1 port 8080 proto tcp

sudo ufw logging on
sudo ufw enable
sudo ufw status numbered
```


## DOCKER AND UFW 
https://blog.viktorpetersson.com/2014/11/03/the-dangers-of-ufw-docker.html
https://linuxconfig.org/how-to-disable-docker-s-iptables-on-systemd-linux-systems