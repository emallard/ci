# ciexe

A program to be called on command line. It forwards arguments to cilib.

When a VM is created, it must be built, and then run with 'docker run'

# Build

Use Dockerfile on .sln root:
```
docker build --force-rm -t ciexe .
```

# Run and destroy container
```
docker run --rm --name ciexe ciexe hello
```