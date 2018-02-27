# export CLONABLEVM=clonable
# export NEWVM=pilote
# export IP=10.0.2.5
# export PORTFORWARD=22005
# vboxmanage unregistervm pilote --delete 

# get options with:
# VBoxManage import "/media/etienne/LinuxData/vm/$CLONABLEVM.ovf" -n

VBoxManage import "/media/etienne/LinuxData/vm/$CLONABLEVM.ovf" --vsys 0 --vmname "$NEWVM" --unit 9 --disk "/media/etienne/LinuxData/vm/$NEWVM.vmdk"

# Not needed if correctly done
# vboxmanage natnetwork add --netname natnet --network "10.0.2.0/24" --enable --dhcp off
# vboxmanage modifyvm pilote --nic1 natnetwork --nat-network1 natnet


# ssh to 10.0.2.200 as defined in VBoxClonableVm.sh
vboxmanage natnetwork modify --netname natnet --port-forward-4 delete sshcloned
vboxmanage natnetwork modify --netname natnet --port-forward-4 "sshcloned:tcp:[127.0.0.1]:22200:[10.0.2.200]:22"

# replace ip in new vm
vboxmanage startvm "$NEWVM" --type headless
sleep 20s
ssh test@127.0.0.1 -p 22200 "sed 's/10.0.2.200/$IP/g' /etc/network/interfaces > sed.tmp && cat sed.tmp > /etc/network/interfaces"
vboxmanage controlvm "$NEWVM" poweroff
vboxmanage startvm "$NEWVM" --type headless

vboxmanage natnetwork modify --netname natnet --port-forward-4 delete "ssh$NEWVM"
vboxmanage natnetwork modify --netname natnet --port-forward-4 "ssh$NEWVM:tcp:[127.0.0.1]:$PORTFORWARD:[$IP]:22"