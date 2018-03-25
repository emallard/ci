# Creation de la machine pilote

Compiler la solution en clonant le depot github.

La création de VMs passe par l'éxécutable ci-infra.

```
ci infra pilote-create
token for devop-infra  ?
infrastructure key ? 
root password ?
admin user ?
admin password ?
```

Réponse:
``` 
Following values were written:
infra/apikey
infra/rootpassword
admin/pilote/user
admin/pilote/password
admin/pilote/ip
```

Vérifier que l'installation s'est bien passée:
```
ci infra pilote-create test
token for devop-infra  ?
```