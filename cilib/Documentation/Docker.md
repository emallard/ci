


## Reuse a container :


### Start

Run container with bash:
```
docker run --name ciexe ciexe /bin/bash
```
-i interactive mode
-t TTY
```
docker run --name ciexe -it ciexe /bin/bash
```

### Restart with an interactive shell

```
docker start ciexe && docker exec -it ciexe bash
```
or (-a attach to container, -i interactive mode)
```
docker start -a -i ciexe
```
or
```
docker start ciexe && docker attach ciexe
```

### Attach to a running container
```
docker exec -it ciexe bash
```

### Run dotnet command ina running container
```
docker exec ciexe dotnet ciexe.dll
```