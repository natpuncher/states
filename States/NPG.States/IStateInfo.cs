using System;

namespace NPG.States
{
	public interface IStateInfo : IUpdatable, IFixedUpdatable
	{
		Type StateType { get; }
		void Enter();
		void Exit();
	}
}