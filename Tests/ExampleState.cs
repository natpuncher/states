using System;

namespace npg.states.tests
{
	public class ExampleState : IGameState, IState, IUpdatable
	{
		public void Exit()
		{
		}

		public void Enter()
		{
		}

		public void Update()
		{
			Console.WriteLine("ExampleState.Update");
		}
	}
}