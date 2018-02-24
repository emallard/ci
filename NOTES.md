



# ~jpetazzo/Using Docker-in-Docker for your CI or testing environment? Think twice.

http://jpetazzo.github.io/2015/09/03/do-not-use-docker-in-docker-for-ci/


## The solution
Let’s take a step back here. Do you really want Docker-in-Docker? Or do you just want to be able to run Docker (specifically: build, run, sometimes push containers and images) from your CI system, while this CI system itself is in a container?

I’m going to bet that most people want the latter. All you want is a solution so that your CI system like Jenkins can start containers.

And the simplest way is to just expose the Docker socket to your CI container, by bind-mounting it with the -v flag.

Simply put, when you start your CI container (Jenkins or other), instead of hacking something together with Docker-in-Docker, start it with:
```
docker run -v /var/run/docker.sock:/var/run/docker.sock ...
```
Now this container will have access to the Docker socket, and will therefore be able to start containers. Except that instead of starting “child” containers, it will start “sibling” containers.

Try it out, using the docker official image (which contains the Docker binary):
```
docker run -v /var/run/docker.sock:/var/run/docker.sock \
           -ti docker
```
This looks like Docker-in-Docker, feels like Docker-in-Docker, but it’s not Docker-in-Docker: when this container will create more containers, those containers will be created in the top-level Docker. You will not experience nesting side effects, and the build cache will be shared across multiple invocations.

⚠️ Former versions of this post advised to bind-mount the docker binary from the host to the container. This is not reliable anymore, because the Docker Engine is no longer distributed as (almost) static libraries.

If you want to use e.g. Docker from your Jenkins CI system, you have multiple options:

installing the Docker CLI using your base image’s packaging system (i.e. if your image is based on Debian, use .deb packages),
using the Docker API.
