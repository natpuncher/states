using System;
using RSG;

namespace NPG.States
{
	public interface IStateInfo : IUpdatable
	{
		Type StateType { get; }
		void Enter();
		IPromise Exit();
	}
}