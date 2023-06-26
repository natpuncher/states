namespace npg.states.Infrastructure
{
	public interface IStateFactory
	{
		TState GetState<TState>() where TState : class, IExitable;
	}
}