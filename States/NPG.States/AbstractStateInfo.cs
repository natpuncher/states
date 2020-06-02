using System;
using RSG;

namespace NPG.States
{
	public abstract class AbstractStateInfo : IStateInfo
	{
		public abstract Type StateType { get; }
		
		private IExitable _exitState;
		private IUpdatable _updatable;
		
		public abstract void Enter();
		
		public void Update()
		{
			_updatable?.Update();
		}

		public IPromise Exit() => 
			_exitState?.Exit();

		protected void InitializeNewState(IExitable state)
		{
			_exitState = state;
			_updatable = state as IUpdatable;
		}
	}
}