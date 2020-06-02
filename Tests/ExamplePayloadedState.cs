using NPG.States;
using RSG;

namespace Tests
{
	public class ExamplePayloadedState : IGameState, IPayloadedState<int>
	{
		public int Payload { get; private set; }
		
		public void Enter(int payload)
		{
			Payload = payload;
		}

		public IPromise Exit()
		{
			return Promise.Resolved();
		}
	}
}