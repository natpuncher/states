namespace NPG.States
{
	public class StateMachine
	{
		public AbstractState CurrentState { get; private set; }

		public void ChangeState(AbstractState abstractState)
		{
			CurrentState?.InternalExit();

			CurrentState = abstractState;
			abstractState.InternalEnter();
		}
	}
}