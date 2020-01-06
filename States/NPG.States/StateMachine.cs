using System;

namespace NPG.States
{
	public abstract class StateMachine : IUpdatable, IDisposable
	{
		protected abstract IStateFactory Factory { get; }

		private IUpdatable _currentUpdatable;
		private IExitable _currentExitable;
		private Type _currentType;

		public TState Enter<TState>() where TState : class, IState
		{
			if (!ChangeState(out TState state))
			{
				return null;
			}

			state.Enter();
			return state;
		}

		public TState Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
		{
			if (!ChangeState(out TState state))
			{
				return null;
			}

			state.Enter(payload);
			return state;
		}

		public bool IsActive(IExitable state)
		{
			return _currentExitable == state;
		}

		public bool IsActive(Type stateType)
		{
			return _currentType == stateType;
		}

		public void Update()
		{
			_currentUpdatable?.Update();
		}
		
		public virtual void Dispose()
		{
			_currentExitable?.Exit();
			_currentExitable = null;
			_currentUpdatable = null;
			_currentType = null;
		}
		
		protected virtual void StateChanged(IExitable oldState, IExitable newState)
		{
		}

		private bool ChangeState<TState>(out TState state) where TState : class, IExitable
		{
			var type = typeof(TState);
			if (IsActive(type))
			{
				state = null;
				return false;
			}

			_currentExitable?.Exit();
			
			state = Factory.GetState<TState>();
			
			_currentUpdatable = state as IUpdatable;

			StateChanged(_currentExitable, state);

			_currentExitable = state;
			_currentType = type;
			return true;
		}
	}
}