namespace NPG.States
{
	public interface IState : IExitState
	{
		void OnEnter();
	}
}