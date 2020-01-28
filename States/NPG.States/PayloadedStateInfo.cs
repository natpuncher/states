using System;

namespace NPG.States
{
	public class PayloadedStateInfo<TState, TBaseState, TPayload> : AbstractStateInfo where TState : class, TBaseState, IPayloadedState<TPayload>
	{
		public override Type StateType { get; }

		private readonly StateMachine<TBaseState> _stateMachine;
		private readonly TPayload _payload;
		
		public PayloadedStateInfo(StateMachine<TBaseState> stateMachine, TState state, TPayload payload)
		{
			_stateMachine = stateMachine;
			_payload = payload;
			StateType = typeof(TState);
			
			Initialize(state);
		}

		public override void Enter()
		{
			var state = _stateMachine.Enter<TState, TPayload>(_payload);
			Initialize(state);
		}
	}
}