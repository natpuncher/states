namespace NPG.States
{
	public class StateMachine
	{
		public State CurrentState { get; private set; }

		public void ChangeState(State state)
		{
			CurrentState?.InternalExit();

			CurrentState = state;
			state.InternalEnter();
		}
	}
}