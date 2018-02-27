sudo apt-get update

sudo apt-get install \
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

sudo apt-get update

sudo apt-get install docker-ce -y

sudo usermod -aG docker $USER

sudo reboot

#docker run hello-world

#recupérer les sources du programme d'init
sudo apt-get install git

cd ~/
git clone https://github.com/emallard/ci.git
cd ~/ci
git pull
cd ~/ci/ciexe
docker build -t ciexe .


# récuperer le runtime
# microsoft/dotnet:2.0-sdk
# microsoft/dotnet




