using RSG;

namespace NPG.States
{
	public interface IExitable
	{
		IPromise Exit();
	}
}