namespace NPG.States
{
	public abstract class StateMachine
	{
		protected abstract IStateFactory Factory { get; }

		private IExitState _currentExitState;

		public void Enter<TState>() where TState : IState
		{
			_currentExitState?.OnExit();
			
			var state = Factory.GetState<TState>();
			state.OnEnter();
		}
		
		public void Enter<TState, TPayload>(TPayload payload) where TState : IPayloadedState<TPayload>
		{
			var state = Factory.GetState<TState>();
			state.OnEnter(payload);
		}

		public bool IsActive(IExitState state)
		{
			return state == _currentExitState;
		}
	}
}