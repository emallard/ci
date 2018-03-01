# CI

Bootstrap a complete CI/CD system based on 2 VMs :
- pilote : build production docker images
- webserver : run traefik, and get docker images from "pilote" VM

Can run on VirtualBox locally, or using PAAS Gandi.net

# Development

- Create a clonable machine of UbuntuServer with VirtualBox. cf clib/Infrastructure/VBoxClonableVm.md
- Run unit tests
