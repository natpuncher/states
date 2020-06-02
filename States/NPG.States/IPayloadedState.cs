namespace NPG.States
{
	public interface IPayloadedState<TPayload> : IExitable
	{
		void Enter(TPayload sceneName);
	}
}