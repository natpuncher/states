using System;

namespace NPG.States
{
	public interface IStateInfo : IUpdatable
	{
		Type StateType { get; }
		void Enter();
		void Exit();
	}
}