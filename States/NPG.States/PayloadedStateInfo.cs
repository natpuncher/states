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
			
			InitializeNewState(state);
		}

		public override void Enter()
		{
			_stateMachine.RequestEnter<TState, TPayload>(_payload)
				.Done(InitializeNewState);
		}

		public override string ToString() => $"{StateType.Name}";
	}
}