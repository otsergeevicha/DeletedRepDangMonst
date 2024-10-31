using Bank;
using Infrastructure.Assets;
using Infrastructure.Factory;
using Infrastructure.SDK;
using Inputs;
using Plugins.MonoCache;
using Reflex.Core;
using SaveLoadModule;
using Services.Bank;
using Services.Factory;
using Services.Inputs;
using Services.SaveLoad;
using Services.SDK;

namespace Reflex
{
    public class ProjectInstaller : MonoCache, IInstaller
    {
        public void InstallBindings(ContainerBuilder descriptor)
        {
            SDKService sdkService = new SDKService();
            InputService inputService = new InputService();
            GameFactory gameFactory = new GameFactory(new AssetsProvider());
            SaveLoad saveLoad = new SaveLoad();
            Wallet wallet = new Wallet(saveLoad);

            descriptor.AddSingleton(sdkService, typeof(ISDKService));
            descriptor.AddSingleton(inputService, typeof(IInputService));
            descriptor.AddSingleton(gameFactory, typeof(IGameFactory));
            descriptor.AddSingleton(saveLoad, typeof(ISave));
            descriptor.AddSingleton(wallet, typeof(IWallet));
        }
    }
}