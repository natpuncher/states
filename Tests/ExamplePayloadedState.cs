using NPG.States;

namespace Tests
{
	public class ExamplePayloadedState : IPayloadedState<int>
	{
		public int Payload { get; private set; }
		
		public void OnEnter(int payload)
		{
			Payload = payload;
		}

		public void OnExit()
		{
		}
	}
}