using System;
using NPG.States;

namespace Tests
{
	public class ExampleState : IState, IUpdatable
	{
		public void OnExit()
		{
		}

		public void OnEnter()
		{
		}

		public void Update()
		{
			Console.WriteLine("ExampleState.Update");
		}
	}
}