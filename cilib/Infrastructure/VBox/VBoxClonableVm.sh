#
# Create a clonable vm with static IP 10.0.2.200 under natnetwork named "natnet"
#
vboxmanage natnetwork add --netname natnet --network "10.0.2.0/24" --enable --dhcp off

VBoxManage controlvm "$CLONABLEVM" poweroff
VBoxManage unregistervm "$CLONABLEVM" --delete


VBoxManage createvm --name "$CLONABLEVM" --register
VBoxManage modifyvm "$CLONABLEVM" --memory 2048 --acpi on --boot1 dvd
vboxmanage modifyvm "$CLONABLEVM" --nic1 natnetwork --nat-network1 natnet
VBoxManage createhd --filename "/media/etienne/LinuxData/vm/$CLONABLEVM.vdi" --size 10000
VBoxManage storagectl "$CLONABLEVM" --name "IDE Controller" --add ide
VBoxManage storageattach "$CLONABLEVM" --storagectl "IDE Controller" --port 0 --device 0 --type hdd --medium "/media/etienne/LinuxData/vm/$CLONABLEVM.vdi"
VBoxManage storageattach "$CLONABLEVM" --storagectl "IDE Controller" --port 1 --device 0 --type dvddrive --medium "/media/etienne/LinuxData/ubuntu-16.04.3-server-amd64.iso"

VBoxManage modifyvm "$CLONABLEVM" --vrde on
VBoxManage startvm "$CLONABLEVM" --type headless
#VBoxHeadless --startvm "$CLONABLEVM" --vrde on

# Install interactively with user : test/test
# Don't forget in packages : OpenSSH Server

VBoxManage controlvm "$CLONABLEVM" poweroff
VBoxManage storageattach "$CLONABLEVM" --storagectl "IDE Controller" --port 1 --device 0 --type dvddrive --medium none



# Now change with static IP:
# Log with remmina and:

sudo chown test /etc/network/interfaces 
nano /etc/network/interfaces
# remplacer 
# auto enp0s3
# iface enp0s3 inet dhcp
# par
auto enp0s3
iface enp0s3 inet static
    address 10.0.2.200
    netmask 255.255.255.0
    network 10.0.2.0
    broadcast 10.0.2.255
    gateway 10.0.2.1

# change dns
sudo nano /etc/resolvconf/resolv.conf.d/base
nameserver "10.0.2.1"

# Export
VBoxManage export "$CLONABLEVM" --output "/media/etienne/LinuxData/vm/$CLONABLEVM.ovf"