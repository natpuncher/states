using NPG.States;

namespace Tests
{
	public class ExampleStateMachine : StateMachine
	{
		public ExampleStateMachine(IStateFactory stateFactory) : base(stateFactory)
		{
		}
	}
}