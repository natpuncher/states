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
			
			InitializeNewState(state);
		}

		public override void Enter()
		{
			_stateMachine.RequestEnter<TState>()
				.Done(InitializeNewState);
		}
		
		public override string ToString() => $"{StateType.Name}";
	}
}