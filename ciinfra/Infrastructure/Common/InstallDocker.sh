
set -e -x

sudo apt-get -qq update

sudo apt-get -qq --yes install \
    apt-transport-https \
    ca-certificates \
    curl \
    software-properties-common

sudo add-apt-repository \
    "deb [arch=amd64] https://download.docker.com/linux/ubuntu \
    $(lsb_release -cs) \
    stable"

curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo apt-key add -

sudo apt-key fingerprint 0EBFCD88

sudo apt-get -qq update

sudo apt-get -qq --yes install docker-ce

sudo usermod -aG docker test

#sudo usermod -aG docker $USER

#sudo reboot
