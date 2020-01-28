using System;

namespace NPG.States
{
	public class PayloadedStateInfo<TState, TPayload> : AbstractStateInfo where TState : class, IPayloadedState<TPayload>
	{
		public override Type StateType { get; }

		private readonly StateMachine _stateMachine;
		private readonly TPayload _payload;
		
		public PayloadedStateInfo(StateMachine stateMachine, TState state, TPayload payload)
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