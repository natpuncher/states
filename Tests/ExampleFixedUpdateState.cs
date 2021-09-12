using NPG.States;

namespace Tests
{
	public class ExampleFixedUpdateState : IGameState, IState, IFixedUpdatable
	{
		public int testValue = 0;

		public void Enter()
		{
		}

		public void Exit()
		{
		}

		public void FixedUpdate()
		{
			testValue++;
		}
	}
}