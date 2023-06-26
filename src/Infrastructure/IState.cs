namespace npg.states.Infrastructure
{
	public interface IState : IExitable
	{
		void Enter();
	}
}