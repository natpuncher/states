using npg.states.Infrastructure;

namespace npg.states.tests
{
	public class TestGameStateMachine : StateMachine<ITestGameState>
	{
		public TestGameStateMachine(IStateFactory factory) : base(factory)
		{
		}
	}

	public interface ITestGameState
	{
	}
}