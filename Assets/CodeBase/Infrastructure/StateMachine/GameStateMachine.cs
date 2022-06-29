using System;
using System.Collections.Generic;
using CodeBase.Common;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Fabric;
using CodeBase.Infrastructure.StateMachine.States;
using CodeBase.Services;
using CodeBase.Services.SceneManagement;

namespace CodeBase.Infrastructure.StateMachine
{
    public class GameStateMachine : IGameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;

        public GameStateMachine(SceneLoader sceneLoader, LoadingCurtain curtain, AllServices services)
        {
            _states = new Dictionary<Type, IExitableState>()
            {
                [typeof(BootstrapState)] =
                    new BootstrapState(this, sceneLoader, services),

                [typeof(LoadLevelState)] = 
                    new LoadLevelState(this, sceneLoader, curtain, services.Single<IGameFactory>()),
                
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