namespace NPG.States
{
	public abstract class PayloadState<TPayload, TStateMachine> : State<TStateMachine> 
		where TPayload : class
		where TStateMachine : StateMachine
	{
		protected TPayload Payload { get; private set; }

		protected PayloadState(TStateMachine stateMachine) : base(stateMachine)
		{
		}

		public void Enter(TPayload payload)
		{
			Payload = payload;
			Enter();
		}

		internal override void InternalEnter()
		{
			OnEnter(Payload);
		}

		internal override void InternalExit()
		{
			base.InternalExit();
			Payload = null;
		}

		protected abstract void OnEnter(TPayload payload);

		protected sealed override void OnEnter()
		{
		}
	}
}