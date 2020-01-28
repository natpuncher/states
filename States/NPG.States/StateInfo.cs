using System;

namespace NPG.States
{
	public class StateInfo<TState, TBaseState> : AbstractStateInfo where TState : class, TBaseState, IState
	{
		public override Type StateType { get; }
		
		private readonly StateMachine<TBaseState> _stateMachine;
		
		public StateInfo(StateMachine<TBaseState> stateMachine, TState state)
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