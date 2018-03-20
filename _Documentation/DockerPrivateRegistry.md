# Run private registry

```
docker run \
    --name privateregistry \
    -v ${HOME}/cidata/privateregistry/var/lib/registry:/var/lib/registry \
    -v ${HOME}/cidata/privateregistry/certs:/certs \
    -e REGISTRY_HTTP_ADDR=0.0.0.0:443 \
    -e REGISTRY_HTTP_TLS_CERTIFICATE=/certs/privateregistry.mynetwork.local.crt \
    -e REGISTRY_HTTP_TLS_KEY=/certs/privateregistry.mynetwork.local.key \
    -p 5443:443 \
    registry:2
```

# Remove images from private registry :

https://github.com/burnettk/delete-docker-registry-image

## Alternatives

Docker is building or has built much of this functionality in newer versions of docker and the registry.

The ability to delete the metadata for a manifest was added in registry:2.2. Make sure you give the registry the environment variable REGISTRY_STORAGE_DELETE_ENABLED=true. Follow the instructions at https://github.com/docker/docker-registry/issues/988#issuecomment-224280919 to delete a tag by name. Once the metadata is deleted, follow the instructions at https://github.com/docker/distribution/blob/master/docs/configuration.md to run garbage collection, which will clean up the binary data (the big stuff).