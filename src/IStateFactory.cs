namespace npg.states
{
	public interface IStateFactory
	{
		T GetState<T>() where T : class, IExitable;
	}
}