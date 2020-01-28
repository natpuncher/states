using NPG.States;

namespace Tests
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