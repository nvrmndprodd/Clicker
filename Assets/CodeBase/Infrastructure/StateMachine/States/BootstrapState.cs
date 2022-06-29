using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Fabric;
using CodeBase.Services;
using CodeBase.Services.SceneManagement;
using CodeBase.Services.SpawnService;
using CodeBase.Services.StaticData;
using UnityEngine;

namespace CodeBase.Infrastructure.StateMachine.States
{
    public class BootstrapState : IState
    {
        private const string Initial = "Initial";
        
        private readonly IGameStateMachine _stateMachine;
        private readonly ISceneLoader _sceneLoader;
        private readonly AllServices _services;

        public BootstrapState(IGameStateMachine gameStateMachine, ISceneLoader sceneLoader, AllServices services)
        {
            _stateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _services = services;
            
            RegisterServices();
        }

        public void Enter()
        {
            _sceneLoader.Load("Main", onLoaded: EnterLoadLevel);
        }

        public void Exit()
        {
        }

        private void EnterLoadLevel() => 
            _stateMachine.Enter<LoadLevelState, string>("Main");

        private void RegisterServices()
        {
            RegisterAssetProvider();
            RegisterStaticDataService();

            _services.RegisterSingle<IGameStateMachine>(_stateMachine);
            
            _services.RegisterSingle<IGameFactory>(new GameFactory
                (
                    _services.Single<IStaticDataService>(), 
                    _services.Single<IAssetProvider>())
                );

            var spawnService = new GameObject("SpawnService").AddComponent<SpawnService>();
            spawnService.Construct(_services.Single<IGameFactory>());
            _services.RegisterSingle(spawnService);
        }

        private void RegisterStaticDataService()
        {
            var staticDataService = new StaticDataService();
            staticDataService.Load();
            _services.RegisterSingle<IStaticDataService>(staticDataService);
        }

        private void RegisterAssetProvider()
        {
            var assetProvider = new AssetProvider();
            _services.RegisterSingle<IAssetProvider>(assetProvider);
            assetProvider.Initialize();
        }
    }
}