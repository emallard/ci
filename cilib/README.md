# cilib

It contains :
- Infrastructure : functions to be called to create VM and initialize it when ciexe is not yet installed.
    - Create VM, assign IP
    - Install Docker
    - Install mirror registry
    - Build a ciexe image.

- Pilote : functions called inside a container in pilote VM
    - Install CA
    - Install private registry
    - Build app from docker file

- WebServer : functions called inside a container in webserver VM
    - Install Traefik
    - Install a new app


