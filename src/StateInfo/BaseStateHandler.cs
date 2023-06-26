using System;
using npg.states.Infrastructure;

namespace npg.states.StateInfo
{
	internal abstract class BaseStateHandler<TStateType> : IStateHandler<TStateType>
	{
		public Type StateType => _exitState?.GetType();

		private IUpdatable _updatable;
		private ILateUpdatable _lateUpdatable;
		private IFixedUpdatable _fixedUpdatable;
		private IExitable _exitState;

		public abstract void ReEnter(StateMachine<TStateType> stateMachine, Type lastStateType);

		public void Update()
		{
			_updatable?.Update();
		}

		public void LateUpdate()
		{
			_lateUpdatable?.LateUpdate();
		}

		public void FixedUpdate()
		{
			_fixedUpdatable?.FixedUpdate();
		}

		public void Exit()
		{
			_exitState?.Exit();
		}

		public void Dispose()
		{
			_exitState = null;
			_updatable = null;
			_lateUpdatable = null;
			_fixedUpdatable = null;
		}

		protected void InternalInitialize(IExitable state)
		{
			_exitState = state;
			_updatable = state as IUpdatable;
			_lateUpdatable = state as ILateUpdatable;
			_fixedUpdatable = state as IFixedUpdatable;
		}
	}
}