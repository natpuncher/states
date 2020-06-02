using System;
using NPG.States;
using RSG;

namespace Tests
{
	public class ExampleState : IGameState, IState, IUpdatable
	{
		public IPromise Exit()
		{
			return Promise.Resolved();
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