using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Fabric;
using CodeBase.Services;
using CodeBase.Services.LevelServices.BoosterService;
using CodeBase.Services.LevelServices.EnemyService;
using CodeBase.Services.LevelServices.SpeedService;
using CodeBase.Services.Progress;
using CodeBase.Services.SceneManagement;
using CodeBase.Services.StaticData;
using CodeBase.Services.Update;
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
            
            _services.RegisterSingle<IPersistentProgressService>(new PersistentProgressService());

            var updateService = new GameObject("UpdateService").AddComponent<UpdateService>();
            _services.RegisterSingle<IUpdateService>(updateService);

            var speedService = new SpeedService(_services.Single<IPersistentProgressService>());
            updateService.OnUpdate += speedService.OnUpdate;
            _services.RegisterSingle<ISpeedService>(speedService);

            var enemyService = new EnemyService(_services.Single<IPersistentProgressService>(),
                _services.Single<IGameFactory>());
            updateService.OnUpdate += enemyService.OnUpdate;
            _services.RegisterSingle<IEnemyService>(enemyService);

            var boosterService = new BoosterService(_services.Single<IGameFactory>(),
                _services.Single<IPersistentProgressService>(), enemyService);
            updateService.OnUpdate += boosterService.OnUpdate;
            _services.RegisterSingle<IBoosterService>(boosterService);
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