namespace NPG.States
{
	public abstract class State<TStateMachine> : AbstractState where TStateMachine : StateMachine
	{
		private readonly StateMachine _stateMachine;

		protected State(TStateMachine stateMachine)
		{
			_stateMachine = stateMachine;
		}

		public void Enter()
		{
			_stateMachine.ChangeState(this);
		}

		protected abstract void OnEnter();
		protected abstract void OnExit();

		internal override void InternalEnter()
		{
			OnEnter();
		}

		internal override void InternalExit()
		{
			OnExit();
		}
	}
}