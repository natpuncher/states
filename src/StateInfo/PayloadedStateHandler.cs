using System;
using npg.states.Infrastructure;

namespace npg.states.StateInfo
{
	internal class PayloadedStateHandler<TState, TStateType, TPayload> : BaseStateHandler<TStateType> 
		where TState : class, TStateType, IPayloadedState<TPayload>
	{
		private TPayload _payload;
		
		public void Initialize(TState state, TPayload payload)
		{
			_payload = payload;
			InternalInitialize(state);
		}

		public override void ReEnter(StateMachine<TStateType> stateMachine, Type lastStateType)
		{
			stateMachine.InternalEnter<TState, TPayload>(lastStateType, _payload, false);
		}
	}
}