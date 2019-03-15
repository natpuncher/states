namespace NPG.States
{
	public abstract class PayloadState<TPayload> : State where TPayload : class
	{
		protected TPayload Payload { get; private set; }
		
		protected PayloadState(StateMachine stateMachine) : base(stateMachine)
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