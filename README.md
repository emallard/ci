# CI

Bootstrap a complete CI/CD system based on 2 VMs :
- pilote : build production docker images, and host them in a local registry
- webserver : run traefik, and get docker images from "pilote" registry

Can run on VirtualBox locally, or using PAAS Gandi.net

# Projects

- [ciinfra](ciinfra/README.md) : VM functions, and SSH commands before ciexe is installed
- [cilib](cilib/README.md) : CI functions to be runned inside a container in VMs

- [cicli](cicli/README.md) : Define commands forwarded to cilib
- [ciexe](ciexe/README.md) : Command line tool that can run cilib functions (used to run ssh on VMs)

- [ciexetest](ciexetest/README.md) : Allow to debug ciexe on local machine
- [citest](citest/README.md) : Full scenario from nothing to VMs fully running webapps 



```
   ciinfra
    /   \
   |    cilib
   |      \
   |     cicli 
    \    /   \      
    citest    ciexe, ciexetest   
```

# Dev / VBox infrastructure

- Install Virtual box
- Download Ubuntu Server ISO.
- Create a clonable machine of UbuntuServer with VirtualBox. cf clib/Infrastructure/VBoxClonableVm.md
- Run citest executable

# How to debug ciexe

In order to debug locally, create a /home/test/cidata directory (the same one as the one inside cloned VMs)
Then debug panel in Visual Studio Code, select Run CI EXE

