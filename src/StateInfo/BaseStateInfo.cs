using System;
using npg.states.Infrastructure;

namespace npg.states.StateInfo
{
	internal abstract class BaseStateInfo<TStateType> : IStateInfo<TStateType>
	{
		public Type StateType => _exitState?.GetType();
		public virtual Type PayloadType => null;

		private IExitable _exitState;
		private IUpdatable _updatable;
		private IFixedUpdatable _fixedUpdatable;

		public abstract void ReEnter(StateMachine<TStateType> stateMachine);

		public void Update()
		{
			_updatable?.Update();
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
			_fixedUpdatable = null;
		}

		protected void InternalInitialize(IExitable state)
		{
			_exitState = state;
			_updatable = state as IUpdatable;
			_fixedUpdatable = state as IFixedUpdatable;
		}
	}
}