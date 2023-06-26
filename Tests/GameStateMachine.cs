using npg.states.Infrastructure;

namespace npg.states.tests
{
	public class GameStateMachine : StateMachine<IGameState>
	{
		public GameStateMachine(IStateFactory factory) : base(factory)
		{
		}
	}
}