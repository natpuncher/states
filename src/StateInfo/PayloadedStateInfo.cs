using System;
using npg.states.Infrastructure;

namespace npg.states.StateInfo
{
	internal class PayloadedStateInfo<TState, TStateType, TPayload> : BaseStateInfo<TStateType> 
		where TState : class, TStateType, IPayloadedState<TPayload>
	{
		public override Type PayloadType => typeof(TPayload);
		private TPayload _payload;
		
		public void Initialize(TState state, TPayload payload)
		{
			_payload = payload;
			InternalInitialize(state);
		}

		public override void ReEnter(StateMachine<TStateType> stateMachine)
		{
			stateMachine.InternalEnter<TState, TPayload>(_payload, false);
		}
	}
}