using npg.states.Infrastructure;

namespace npg.states.tests
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