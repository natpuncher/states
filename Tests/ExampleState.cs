using System;
using NPG.States;

namespace Tests
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