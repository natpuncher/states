using System;
using RSG;

namespace NPG.States
{
  public abstract class StateMachine<TBaseState> : IUpdatable, IDisposable
  {
    public Type ActiveStateType => _currentStateInfo?.StateType;

    private readonly IStateFactory _stateFactory;

    private IStateInfo _currentStateInfo;
    private IStateInfo _lastStateInfo;

    protected StateMachine(IStateFactory stateFactory)
    {
      _stateFactory = stateFactory;
    }

    public void Enter<TState>() where TState : class, TBaseState, IState =>
      RequestEnter<TState>()
        .Done();

    public void Enter<TState, TPayload>(TPayload payload) where TState : class, TBaseState, IPayloadedState<TPayload> =>
      RequestEnter<TState, TPayload>(payload)
        .Done();
    
    public IPromise<TState> RequestEnter<TState>() where TState : class, TBaseState, IState =>
      RequestChangeState<TState>()
        .Then(EnterSimpleState);

    public IPromise<TState> RequestEnter<TState, TPayload>(TPayload payload) where TState : class, TBaseState, IPayloadedState<TPayload> =>
      RequestChangeState<TState>()
        .Then(state => EnterPayloadedState(state, payload));

    public bool Back()
    {
      if (_lastStateInfo == null)
      {
        return false;
      }

      _lastStateInfo.Enter();
      return true;
    }

    public void Update()
    {
      _currentStateInfo?.Update();
    }

    public void Dispose()
    {
      _currentStateInfo?.Exit();
      _currentStateInfo = null;
      _lastStateInfo = null;
    }

    protected virtual void StateChanged(Type oldStateType, Type newStateType)
    {
    }

    private TState EnterSimpleState<TState>(TState state) where TState : class, TBaseState, IState
    {
      _lastStateInfo = _currentStateInfo;
      _currentStateInfo = new StateInfo<TState, TBaseState>(this, state);

      state.Enter();
      return state;
    }

    private TState EnterPayloadedState<TState, TPayload>(TState state, TPayload payload) where TState : class, TBaseState, IPayloadedState<TPayload>
    {
      _lastStateInfo = _currentStateInfo;
      _currentStateInfo = new PayloadedStateInfo<TState, TBaseState, TPayload>(this, state, payload);

      state.Enter(payload);
      return state;
    }

    private IPromise<TState> RequestChangeState<TState>() where TState : class, IExitable
    {
      if (_currentStateInfo != null)
      {
        return _currentStateInfo
          .Exit()
          .Then(ChangeState<TState>);
      }
      
      return ChangeState<TState>();
    }

    private IPromise<TState> ChangeState<TState>() where TState : class, IExitable
    {
      var state = _stateFactory.GetState<TState>();
      StateChanged(ActiveStateType, typeof(TState));

      return Promise<TState>.Resolved(state);
    }
  }
}