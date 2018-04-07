using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Autofac.Core;
using Autofac;
using ciinfra;
using citools;

namespace citest
{
    public class VBoxIntegrationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Resources
            builder.RegisterType<StoreResolverVault>().As<IStoreResolver>().SingleInstance();            
            builder.RegisterType<AskMock>().As<IAsk>().SingleInstance();
            
            // Infrastructure
            builder.RegisterType<VBoxInfrastructure>().As<IInfrastructure>().SingleInstance();
            builder.RegisterType<RenciSshClient>().As<ISshClient>();

            // Utils
            builder.RegisterType<OpenSslShell>().As<IOpenSsl>().SingleInstance();
            builder.RegisterType<GitShell>().As<IGit>().SingleInstance();
            builder.RegisterType<ShellCommandExecute>().As<IShellCommandExecute>();

            // Vault seal keys
            builder.RegisterType<VaultSealKeysFile>().As<IVaultSealKeys>().SingleInstance();
        }
    }
}