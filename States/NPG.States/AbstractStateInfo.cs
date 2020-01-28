using System;

namespace NPG.States
{
	public abstract class AbstractStateInfo : IStateInfo
	{
		public abstract Type StateType { get; }
		
		private IExitState _exitState;
		private IUpdatable _updatable;
		
		public abstract void Enter();
		
		public void Update()
		{
			_updatable?.Update();
		}

		public void Exit()
		{
			_exitState?.OnExit();
		}

		protected void Initialize(IExitState state)
		{
			_exitState = state;
			_updatable = state as IUpdatable;
		}
	}
}