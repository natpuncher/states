namespace NPG.States
{
	public abstract class PayloadState<TPayload> : State where TPayload : class
	{
		private TPayload _tmpPayload;
		
		protected PayloadState(StateMachine stateMachine) : base(stateMachine)
		{
		}
		
		public void Enter(TPayload payload)
		{
			_tmpPayload = payload;
			Enter();
		}
		
		internal override void InternalEnter()
		{
			OnEnter(_tmpPayload);
		}

		internal override void InternalExit()
		{
			_tmpPayload = null;
			base.InternalExit();
		}

		protected abstract void OnEnter(TPayload payload);
		protected sealed override void OnEnter()
		{
		}
	}
}