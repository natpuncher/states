using NPG.States;
using RSG;

namespace Tests
{
	public class AnotherPayloadedState : IAnotherState, IPayloadedState<string>
	{
		public void Enter(string payload)
		{
		}

		public IPromise Exit()
		{
			return Promise.Resolved();
		}
	}
}