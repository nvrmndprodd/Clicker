using CodeBase.Infrastructure.StateMachine.States;
using CodeBase.Services;

namespace CodeBase.Infrastructure.StateMachine
{
    public interface IGameStateMachine : IService
    {
        void Enter<TState>() where TState : class, IState;
        void Enter<TState, TPayload>(TPayload sceneName) where TState : class, IPayloadedState<TPayload>;
    }
}