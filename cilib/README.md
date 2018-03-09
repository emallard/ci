# cilib

It contains :
- Infrastructure : functions to be called by the developer machine
- Pilote : functions called inside a container in pilote VM
- WebServer : functions called inside a container in webserver VM
- Cli : Remote client to call ciexe functions contained in Pilote et WebServer directories

# Examples to show differences between Infrastructure and Cli 

## Infrastucture
To be called from developer machine to:
- Create VM, assign IP
- Install Docker
- Install mirror registry
- Build a ciexe image.

## Cli
Call ciexe on a SSH to:
- generate a Credential Authority
- install a private registry
- build a webapp project and publish it to private registry
