namespace NPG.States
{
	public interface IPayloadedState<TPayload> : IExitState
	{
		void OnEnter(TPayload payload);
	}
}