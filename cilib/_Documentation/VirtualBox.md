
# Virtual box

# Install
```
sudo apt-get install virtualbox
curl http://releases.ubuntu.com/16.04.3/ubuntu-16.04.3-server-amd64.iso
```

https://www.howtoforge.com/tutorial/running-virtual-machines-with-virtualbox-5.1-on-a-headless-ubuntu-16.04-lts-server/

// Download de l'extension pack : 
wget https://download.virtualbox.org/virtualbox/5.2.6/Oracle_VM_VirtualBox_Extension_Pack-5.2.6-120293.vbox-extpack


```
VBoxManage controlvm "vma" poweroff
VBoxManage unregistervm "vma" --delete


VBoxManage createvm --name "vma" --register
VBoxManage modifyvm "vma" --memory 2048 --acpi on --boot1 dvd --nic1 nat --natpf1 "ssh,tcp,,10022,,22"
VBoxManage createhd --filename /media/etienne/LinuxData/vm/vma.vdi --size 10000
VBoxManage storagectl "vma" --name "IDE Controller" --add ide
VBoxManage storageattach "vma" --storagectl "IDE Controller" --port 0 --device 0 --type hdd --medium /media/etienne/LinuxData/vm/vma.vdi
VBoxManage storageattach "vma" --storagectl "IDE Controller" --port 1 --device 0 --type dvddrive --medium /media/etienne/LinuxData/ubuntu-16.04.3-server-amd64.iso
VBoxManage modifyvm "vma" --vrde on

VBoxManage startvm "vma" --type headless

# installation interactive
# Attention Pendant l'installation : Choisir dans les packages à installer "OpenSSH Server" pour activer l'accès SSH.

VBoxManage controlvm vma poweroff
VBoxManage storageattach "vma" --storagectl "IDE Controller" --port 1 --device 0 --type dvddrive --medium none

```

Remplacer eth0 par le wifi
```
VBoxManage modifyvm "vma" --memory 2048 --acpi on --boot1 dvd --nic1 bridged --bridgeadapter1 ens33
VBoxManage modifyvm "a" --nic1 bridged --bridgeadapter1 wlp5s0
```

Lancer
```
VBoxManage startvm "$VIRTUALMACHINE_NAME" --type headless
VBoxHeadless --startvm "$VIRTUALMACHINE_NAME"
```

Pour remmina, entrer l'URL de l'hote, et la connexion s'effectuera grâce à la ligne: VBoxManage modifyvm "vma" --vrde on


# Export / Import

https://nakkaya.com/2012/08/30/create-manage-virtualBox-vms-from-the-command-line/

Export:
```
VBoxManage export "vma" --output /media/etienne/LinuxData/vm/vma.ovf
```

Import: -n pour avoir les options de clone
```
VBoxManage import /media/etienne/LinuxData/vm/vma.ovf -n
VBoxManage import /media/etienne/LinuxData/vm/vma.ovf --vsys 0 --vmname pilote --unit 9 --disk /media/etienne/LinuxData/vm/pilote.vmdk
VBoxManage modifyvm pilote --natpf1 "sshpilote,tcp,,10023,,22"


vboxmanage  natnetwork add --netname natwebsec --network "172.16.76.0/24" --enable --dhcp on
vboxmanage modifyvm vma --nic1 natnetwork --nat-network1 natwebsec
vboxmanage modifyvm pilote --nic1 natnetwork --nat-network1 natwebsec


vboxmanage natnetwork modify --netname natwebsec --port-forward-4 "sshvma:tcp:[127.0.0.1]:22001:[172.16.76.4]:22"
vboxmanage natnetwork modify --netname natwebsec --port-forward-4 "sshpilote:tcp:[127.0.0.1]:22002:[172.16.76.5]:22"
```
VBoxHeadless --startvm "pilote" --vrde off &
This starts the VM without remote desktop support.

Delete the VM,
VBoxManage unregistervm pilote --delete

# Remmina

Il est possible de changer l'IP de connection
```
VBoxManage modifyvm "VM name" --vrde on
VBoxManage modifyvm "VM name" --vrdeport 3391
VBoxManage modifyvm "VM name" --vrdeaddress 127.0.0.2
```


# nat network
http://fc.isima.fr/~mazenod/zz2-f5-websec.html




## oops avec le dhcpserver
vboxmanage hostonlyif create
vboxmanage hostonlyif ipconfig vboxnet0 --ip 192.168.1.1
vboxmanage modifyvm "guest" --nic2 hostonly --nictype2 Am79C973 --hostonlyadapter2 vboxnet0
vboxmanage list hostonlyifs
vboxmanage dhcpserver add --ifname vboxnet0 --ip 192.168.1.1 --netmask 255.255.255.0 --lowerip 192.168.1.100 --upperip 192.168.1.200
vboxmanage dhcpserver modify --ifname vboxnet0 --enable