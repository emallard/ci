# CI

Bootstrap a complete CI/CD system based on 2 VMs :
- pilote : build production docker images, and host them in a local registry
- webserver : run traefik, and get docker images from "pilote" registry

Can run on VirtualBox locally, or using PAAS Gandi.net

# Projects

- [cilib](cilib/README.md) : Main Project
- [ciexe](ciexe/README.md) : Command line tool that can run cilib functions (used to run ssh on VMs)
- [cicli](cicli/README.md) : Lib to call ciexe from ssh
- [citest](citest/README.md) : Full pipeline test



```
ciinfra
    |  \
    |   cilib
    |      \
    |     cicli __ ciexetest
    |    /     \      
    citest       ciexe   
```

# Dev / VBox infrastructure

- Install Virtual box
- Download Ubuntu Server ISO.
- Create a clonable machine of UbuntuServer with VirtualBox. cf clib/Infrastructure/VBoxClonableVm.md
- Run citest executable

# How to debug ciexe

In order to debug locally, create a /home/test/cidata directory (the same one as the one inside cloned VMs)
Then debug panel in Visual Studio Code, select Run CI EXE

