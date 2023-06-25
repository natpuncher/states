namespace npg.states
{
	public interface IPayloadedState<TPayload> : IExitable
	{
		void Enter(TPayload payload);
	}
}