using System;

namespace NPG.States
{
	public class StateInfo<TState> : AbstractStateInfo where TState : class, IState
	{
		public override Type StateType { get; }
		
		private readonly StateMachine _stateMachine;
		
		public StateInfo(StateMachine stateMachine, TState state)
		{
			_stateMachine = stateMachine;
			StateType = typeof(TState);
			
			Initialize(state);
		}

		public override void Enter()
		{
			var state = _stateMachine.Enter<TState>();
			Initialize(state);
		}
	}
}