using System.Threading.Tasks;
using CodeBase.Common;
using CodeBase.Infrastructure.Fabric;
using CodeBase.Services.LevelServices.BoosterService;
using CodeBase.Services.LevelServices.EnemyService;
using CodeBase.Services.LevelServices.SpeedService;
using CodeBase.Services.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.StateMachine.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private const string InitialPointTag = "InitialPoint";
        
        private readonly IGameStateMachine _gameStateMachine;
        private readonly ISceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly IGameFactory _gameFactory;
        private readonly ISpeedService _speedService;
        private readonly IEnemyService _enemyService;
        private readonly IBoosterService _boosterService;

        public LoadLevelState(IGameStateMachine gameStateMachine, 
            ISceneLoader sceneLoader, 
            LoadingCurtain loadingCurtain,
            IGameFactory gameFactory,
            ISpeedService speedService,
            IEnemyService enemyService,
            IBoosterService boosterService)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _gameFactory = gameFactory;
            _speedService = speedService;
            _enemyService = enemyService;
            _boosterService = boosterService;
        }

        public void Enter(string sceneName)
        {
            _loadingCurtain.Show();
            _sceneLoader.Load(sceneName, OnLoaded);
            
            ClearServices();
        }

        public void Exit() => 
            _loadingCurtain.Hide();

        private void OnLoaded() => 
            _gameStateMachine.Enter<GameLoopState>();

        private void ClearServices()
        {
            _speedService.Clear();
            _enemyService.Clear();
            _boosterService.Clear();
        }
    }
}