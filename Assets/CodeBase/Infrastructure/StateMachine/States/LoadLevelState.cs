using System.Threading.Tasks;
using CodeBase.Common;
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

        public LoadLevelState(IGameStateMachine gameStateMachine, 
            ISceneLoader sceneLoader, 
            LoadingCurtain loadingCurtain)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
        }

        public void Enter(string sceneName)
        {
            _loadingCurtain.Show();
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit() => 
            _loadingCurtain.Hide();

        private void OnLoaded()
        {
            _gameStateMachine.Enter<GameLoopState>();
        }
    }
}