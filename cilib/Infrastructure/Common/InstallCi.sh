
set -e -x

#recupérer les sources du programme d'integration continue
sudo apt-get -qq --yes install git

cd ~/
rm -rf ~/ci
git --quiet clone https://github.com/emallard/ci.git
cd ~/ci
git --quiet pull
cd ~/ci/ciexe

#docker build -t ciexe .

#docker run ciexe InstallPilote