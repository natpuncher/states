namespace NPG.States
{
	public abstract class State
	{
		private readonly StateMachine _stateMachine;

		protected State(StateMachine stateMachine)
		{
			_stateMachine = stateMachine;
		}

		public void Enter()
		{
			_stateMachine.ChangeState(this);
		}

		protected abstract void OnEnter();
		protected abstract void OnExit();

		internal virtual void InternalEnter()
		{
			OnEnter();
		}

		internal virtual void InternalExit()
		{
			OnExit();
		}
	}
}