using System;

namespace NPG.States
{
	public abstract class StateMachine
	{
		protected abstract IStateFactory Factory { get; }

		private IExitState _currentExitState;
		private Type _currentType;

		public void Enter<TState>() where TState : class, IState
		{
			if (!ChangeState(out TState state))
			{
				return;
			}

			state.OnEnter();
		}

		public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
		{
			if (!ChangeState(out TState state))
			{
				return;
			}
			
			state.OnEnter(payload);
		}

		public bool IsActive(IExitState state)
		{
			return _currentExitState == state;
		}

		public bool IsActive(Type stateType)
		{
			return _currentType == stateType;
		}

		private bool ChangeState<TState>(out TState state) where TState : class, IExitState
		{
			var type = typeof(TState);
			if (_currentType == type)
			{
				state = null;
				return false;
			}

			_currentExitState?.OnExit();

			state = Factory.GetState<TState>();
			_currentExitState = state;
			_currentType = type;
			return true;
		}
	}
}