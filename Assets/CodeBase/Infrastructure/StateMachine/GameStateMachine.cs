using System;
using System.Collections.Generic;
using CodeBase.Common;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Fabric;
using CodeBase.Infrastructure.StateMachine.States;
using CodeBase.Services;
using CodeBase.Services.LevelServices.BoosterService;
using CodeBase.Services.LevelServices.EnemyService;
using CodeBase.Services.LevelServices.SpeedService;
using CodeBase.Services.Progress;
using CodeBase.Services.SceneManagement;
using CodeBase.Services.Update;

namespace CodeBase.Infrastructure.StateMachine
{
    public class GameStateMachine : IGameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;

        public GameStateMachine(ISceneLoader sceneLoader, LoadingCurtain curtain, AllServices services)
        {
            _states = new Dictionary<Type, IExitableState>()
            {
                [typeof(BootstrapState)] =
                    new BootstrapState(this, sceneLoader, services, curtain),

                [typeof(LoadLevelState)] = 
                    new LoadLevelState(
                        this, 
                        sceneLoader, 
                        curtain, 
                        services.Single<IGameFactory>(), 
                        services.Single<IPersistentProgressService>(),
                        services.Single<IUpdateService>()
                        ),
                
                [typeof(GameLoopState)] = 
                    new GameLoopState()
            };
        }

        public void Enter<TState>() where TState : class, IState
        {
            var state = ChangeState<TState>();
            state.Enter();
        }
        
        public void Enter<TState, TPayload>(TPayload sceneName) where TState : class, IPayloadedState<TPayload>
        {
            var state = ChangeState<TState>();
            state.Enter(sceneName);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();

            var state = GetState<TState>();
            _activeState = state;

            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState => 
            _states[typeof(TState)] as TState;
    }
}