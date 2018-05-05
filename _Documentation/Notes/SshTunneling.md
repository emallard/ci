
## ssh tunneling

https://serverfault.com/questions/530787/ssh-tunnel-access-remote-server-with-private-ip-through-a-different-server

```
You haven't show us what you've tried so far, but something as simple as this should work:

ssh -L 8080:private.remoteserver:8080 remoteserver
Which would then let you run:

curl http://localhost:8080/
...which due to the port forwarding we just set up would actually connect to port 8080 on private.remoteserver.

If you want to be able to directly access http://private.remoteserver:8080/ from your client, you'll need to (a) set up some sort of proxy and (b) configure curl (or other software) to use the proxy. You can set up a SOCKS5 proxy with ssh using the -D option:

ssh -D 1080 remoteserver
And then you can:

curl --socks5-hostname http://private.remoteserver:8080/
Most web browsers (Firefox, Chrome) can also be configured to operate with a SOCKS5 proxy. If you search for "ssh dynamic forwarding" you'll find lots of good documentation, including this article from Ubuntu.

```