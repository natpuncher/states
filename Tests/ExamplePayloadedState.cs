namespace npg.states.tests
{
	public class ExamplePayloadedState : IGameState, IPayloadedState<int>
	{
		public int Payload { get; private set; }
		
		public void Enter(int payload)
		{
			Payload = payload;
		}

		public void Exit()
		{
		}
	}
}