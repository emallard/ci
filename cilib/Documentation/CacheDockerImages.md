
# Cache docker images

The idea is to use a proxyin development stage, so the ssame docker images don't need to be downloaded again and again.
Ex: microsoft/aspnet-build.

## Registry V2, mirror

- http://planet.jboss.org/post/deploy_and_configure_a_local_docker_caching_proxy
- https://docs.docker.com/registry/recipes/mirror/#what-about-my-disk
- https://blog.docker.com/2015/10/registry-proxy-cache-docker-open-source/

## SQUID

- http://jpetazzo.github.io/2014/06/17/transparent-squid-proxy-docker/
- http://unpoucode.blogspot.fr/2014/11/use-proxy-for-speeding-up-docker-images.html

