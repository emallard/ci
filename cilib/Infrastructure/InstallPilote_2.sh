
#docker run hello-world

#recupérer les sources du programme d'integration continue
sudo apt-get install git

cd ~/
git clone https://github.com/emallard/ci.git
cd ~/ci
git pull
cd ~/ci/ciexe

#docker build -t ciexe .

#docker run ciexe InstallPilote