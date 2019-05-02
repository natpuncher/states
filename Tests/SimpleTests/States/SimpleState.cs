using System;

namespace NPG.States.Test
{
	public class SimpleState : State<StateMachine>
	{
		public SimpleState(StateMachine stateMachine) : base(stateMachine)
		{
		}

		protected override void OnEnter()
		{
			Console.WriteLine("SimpleState:OnEnter");
		}

		protected override void OnExit()
		{
			Console.WriteLine("SimpleState:OnExit");
		}
	}
}