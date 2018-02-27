vboxmanage natnetwork stop --netname natnet
vboxmanage natnetwork remove --netname natnet

VBoxManage controlvm pilote poweroff
VBoxManage unregistervm pilote --delete

VBoxManage controlvm webserver poweroff
VBoxManage unregistervm webserver --delete
