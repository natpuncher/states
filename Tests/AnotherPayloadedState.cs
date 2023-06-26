using npg.states.Infrastructure;

namespace npg.states.tests
{
	public class AnotherPayloadedState : IAnotherState, IPayloadedState<string>
	{
		public void Enter(string payload)
		{
		}

		public void Exit()
		{
		}
	}
}