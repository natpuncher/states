using NPG.States;

namespace Tests
{
	public class ExampleStateFactory : IStateFactory
	{
		public T GetState<T>() where T : IExitState, new()
		{
			return new T();
		}
	}
}