using NPG.States;

namespace Tests
{
	public class WrongPayloadedState : IPayloadedState<string>
	{
		public void Enter(string payload)
		{
		}

		public void Exit()
		{
		}
	}
}