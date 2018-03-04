
set -e -x

#recupérer les sources du programme d'integration continue
sudo apt-get -qq --yes install git

cd ~/
if [ -d "~/ci" ]; then
    cd ~/ci
    GITPULL=$(git pull)
    UPTODATE=$(echo "Already up-to-date.")
    if [[ $GITPULL = $UPTODATE ]]; then
        echo "up to date"
    else
        docker rm -f ciexe
        docker build --force-rm -t ciexe .
        docker run --name ciexe -it ciexe bash
    fi
    
else
    #rm -rf ~/ci
    git clone --quiet https://github.com/emallard/ci.git
    cd ~/ci
    docker build --rm -t ciexe .
fi
