
set -e -x

#recupérer les sources du programme d'integration continue
sudo apt-get -qq --yes install git

cd ~/
mkdir -p ~/ci
if [ -d "~/ci" ]; then
    cd ~/ci
    GITPULL=$(git pull)
    UPTODATE=$(echo "Already up-to-date.")
    if [[ $GITPULL = $UPTODATE ]]; then
        echo "up to date"
    else
        docker rm -f ciexe
        # build and remove temporary containers
        docker build --force-rm -t ciexe .
        # removoe <none> images
        docker image rm $(docker images -f "dangling=true" -q)
        # run ciexe with bash 
        docker run --name ciexe ciexe bash
    fi
    
else
    cd ~/ci
    git clone --quiet https://github.com/emallard/ci.git
    

    # build and remove temporary containers
    docker build --force-rm -t ciexe .
    # removoe <none> images
    docker image rm $(docker images -f "dangling=true" -q)
    # run ciexe with bash 
    docker run --name ciexe ciexe bash
fi
