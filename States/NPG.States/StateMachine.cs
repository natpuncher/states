using System;

namespace NPG.States
{
	public abstract class StateMachine
	{
		protected abstract IStateFactory Factory { get; }

		private IUpdatable _currentUpdatableState;
		private IExitState _currentExitState;
		private Type _currentType;

		public TState Enter<TState>() where TState : class, IState
		{
			if (!ChangeState(out TState state))
			{
				return null;
			}

			state.OnEnter();
			return state;
		}

		public TState Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
		{
			if (!ChangeState(out TState state))
			{
				return null;
			}

			state.OnEnter(payload);
			return state;
		}

		public bool IsActive(IExitState state)
		{
			return _currentExitState == state;
		}

		public bool IsActive(Type stateType)
		{
			return _currentType == stateType;
		}

		public void Update()
		{
			_currentUpdatableState?.Update();
		}
		
		protected virtual void StateChanged(IExitState oldState, IExitState newState)
		{
		}

		private bool ChangeState<TState>(out TState state) where TState : class, IExitState
		{
			var type = typeof(TState);
			if (IsActive(type))
			{
				state = null;
				return false;
			}

			_currentExitState?.OnExit();
			
			state = Factory.GetState<TState>();
			
			_currentUpdatableState = state as IUpdatable;

			StateChanged(_currentExitState, state);

			_currentExitState = state;
			_currentType = type;
			return true;
		}
	}
}