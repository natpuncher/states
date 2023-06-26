namespace npg.states.Infrastructure
{
	public interface IPayloadedState<TPayload> : IExitable
	{
		void Enter(TPayload payload);
	}
}