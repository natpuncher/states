using NPG.States;

namespace Tests
{
	public class GameStateMachine : StateMachine<IGameState>
	{
		public GameStateMachine(IStateFactory factory) : base(factory)
		{
		}
	}
}