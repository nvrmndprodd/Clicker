using System;
using System.Threading.Tasks;
using CodeBase.Common;
using CodeBase.Infrastructure.Fabric;
using CodeBase.Services.LevelServices.BoosterService;
using CodeBase.Services.LevelServices.EnemyService;
using CodeBase.Services.LevelServices.SpeedService;
using CodeBase.Services.Progress;
using CodeBase.Services.SceneManagement;
using CodeBase.Services.Update;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CodeBase.Infrastructure.StateMachine.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private const string InitialPointTag = "InitialPoint";
        
        private readonly IGameStateMachine _gameStateMachine;
        private readonly ISceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly IGameFactory _gameFactory;
        private readonly IPersistentProgressService _progressService;
        private readonly IUpdateService _updateService;
        
        
        private readonly ISpeedService _speedService;
        private readonly IEnemyService _enemyService;
        private readonly IBoosterService _boosterService;
        private PlayerLostPopup _playerLostPopup;

        public LoadLevelState(IGameStateMachine gameStateMachine, 
            ISceneLoader sceneLoader, 
            LoadingCurtain loadingCurtain,
            IGameFactory gameFactory,
            IPersistentProgressService progressService,
            IUpdateService updateService)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _gameFactory = gameFactory;
            _progressService = progressService;
            _updateService = updateService;
        }

        public void Enter(string sceneName)
        {
            _loadingCurtain.Show();
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit() => 
            _loadingCurtain.Hide();

        private async void OnLoaded()
        {
            _playerLostPopup ??= await _gameFactory.CreatePlayerLostPopup();
            var button = _playerLostPopup.GetComponentInChildren<Button>();
            button.onClick = new Button.ButtonClickedEvent();
            button.onClick.AddListener(() => _gameStateMachine.Enter<LoadLevelState, string>("Main"));
            _playerLostPopup.Hide();
            
            CreateServices();

            _gameStateMachine.Enter<GameLoopState>();
        }

        private void CreateServices()
        {
            _speedService?.Clear();
            var speedService = new SpeedService(_progressService);
            _updateService.OnUpdate += speedService.OnUpdate;

            _enemyService?.Clear();
            var enemyService = new EnemyService(_progressService, _gameFactory, _playerLostPopup);
            _updateService.OnUpdate += enemyService.OnUpdate;

            _boosterService?.Clear();
            var boosterService = new BoosterService(_gameFactory, _progressService, enemyService);
            _updateService.OnUpdate += boosterService.OnUpdate;
        }
    }
}