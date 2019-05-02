namespace NPG.States
{
	public abstract class AbstractState
	{
		internal abstract void InternalEnter();
		internal abstract void InternalExit();
	}
}