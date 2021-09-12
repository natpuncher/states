using System;

namespace NPG.States
{
	public abstract class AbstractStateInfo : IStateInfo
	{
		public abstract Type StateType { get; }

		private IExitable _exitState;
		private IUpdatable _updatable;
		private IFixedUpdatable _fixedUpdatable;

		public abstract void Enter();

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

		protected void Initialize(IExitable state)
		{
			_exitState = state;
			_updatable = state as IUpdatable;
			_fixedUpdatable = state as IFixedUpdatable;
		}
	}
}