using System;

namespace NPG.States.Test
{
	public class ChainState : PayloadState<SimplePayload, StateMachine>
	{
		private readonly ChainState _nextState;

		public ChainState(StateMachine stateMachine, ChainState nextState = null) : base(stateMachine)
		{
			_nextState = nextState;
		}

		protected override void OnEnter(SimplePayload payload)
		{
			Console.WriteLine("ChainState:OnEnter");
			
			Console.WriteLine(payload.Name);

			if (_nextState == null)
			{
				Console.WriteLine("Chain end");
				return;
			}
			
			_nextState.Enter(payload);
		}

		protected override void OnExit()
		{
			Console.WriteLine("ChainState:OnExit");
		}
	}
}