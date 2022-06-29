using System.Threading.Tasks;
using CodeBase.Common;
using CodeBase.Infrastructure.Fabric;
using CodeBase.Services.SceneManagement;
using CodeBase.Services.SpawnService;
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

        public LoadLevelState(IGameStateMachine gameStateMachine, 
            ISceneLoader sceneLoader, 
            LoadingCurtain loadingCurtain,
            IGameFactory gameFactory)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _gameFactory = gameFactory;
        }

        public void Enter(string sceneName)
        {
            _loadingCurtain.Show();
            _sceneLoader.Load(sceneName, OnLoaded);
            
            CreateSpawnService();
        }

        public void Exit() => 
            _loadingCurtain.Hide();

        private void OnLoaded() => 
            _gameStateMachine.Enter<GameLoopState>();

        private void CreateSpawnService()
        {
            var spawnService = new GameObject("SpawnService").AddComponent<SpawnService>();
            spawnService.Construct(_gameFactory);
        }
    }
}