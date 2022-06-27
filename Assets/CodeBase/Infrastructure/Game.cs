using CodeBase.Common;
using CodeBase.Infrastructure.StateMachine;
using CodeBase.Services;
using CodeBase.Services.SceneManagement;

namespace CodeBase.Infrastructure
{
    public class Game
    {
        public GameStateMachine StateMachine;

        public Game(ICoroutineRunner coroutineRunner, LoadingCurtain curtain) => 
            StateMachine = new GameStateMachine(new SceneLoader(coroutineRunner), curtain, AllServices.Container);
    }
}