REDPLOY='n'
DOWN='n'

if [ $# -eq 0 ]
then
    SPINUP_ALL='y'
else
    for arg in "$@"
    do

        if [ "$arg" == "--redeploy" ] || [ "$arg" == "-r" ]
        then
            REDPLOY='y'
        elif [ "$arg" == "--down" ] || [ "$arg" == "-d" ]
        then
            DOWN='y'
        fi
    done
fi

if [ $REDPLOY == 'y' ]
then
    docker-compose up -d --build recognizer
    echo -e "\e[1;33mRedeploy Recognizer \e[0m"
    exit 0
fi

if [ $DOWN == 'y' ]
then
    docker-compose down
    echo -e "\e[1;32mSuccessfully Downed all images \e[0m"
    exit 0
fi