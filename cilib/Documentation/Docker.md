


## Reuse a container :


### Start
Run container with bash:
```
docker run --name ciexe -it ciexe /bin/bash
```

### Restart

Restart a shell :
-a attach to container
-i interactive mode
```
docker start -a -i ciexe
```
ou
```
docker start ciexe && docker attach ciexe
```

### Run dotnet command
```
docker exec ciexe dotnet ciexe.dll
```