using CodeBase.Common;
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
using UnityEngine.UI;

namespace CodeBase.Infrastructure.StateMachine.States
{
    public class BootstrapState : IState
    {
        private const string Initial = "Initial";
        
        private readonly IGameStateMachine _stateMachine;
        private readonly ISceneLoader _sceneLoader;
        private readonly AllServices _services;
        private readonly LoadingCurtain _curtain;

        public BootstrapState(IGameStateMachine gameStateMachine, ISceneLoader sceneLoader, AllServices services,
            LoadingCurtain curtain)
        {
            _stateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _services = services;
            _curtain = curtain;

            RegisterServices();
        }

        public void Enter()
        {
            _curtain.Show();
            _sceneLoader.Load("Initial", onLoaded: EnterLoadLevel);
        }

        public void Exit()
        {
        }

        private void EnterLoadLevel()
        {
            _curtain.Hide();
            GameObject.FindWithTag("StartButton").GetComponent<Button>().onClick
                .AddListener(() => _stateMachine.Enter<LoadLevelState, string>("Main"));
        }

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

            var inputService = new GameObject("InputService").AddComponent<InputService>();
            inputService.Construct(_services.Single<IPersistentProgressService>());
            _services.RegisterSingle(inputService);
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